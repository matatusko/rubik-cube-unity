using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCube : MonoBehaviour {

   public Transform cameraPivot;
   public SmallCube smallCubePrefab;
   public float rotationTime;

   private SmallCube[,,] smallCubes;
   private bool currentlyRotating;
   private GameManager gameManager;
   
   private void Awake() {
      currentlyRotating = false;
      cameraPivot = Camera.main.transform.parent;
      gameManager = FindObjectOfType<GameManager>();
   }
   public void GenerateCube() {
      smallCubes = new SmallCube[PlayerSettings.CubeSize, PlayerSettings.CubeSize, PlayerSettings.CubeSize];
      CreateCube();
   }

   private void CreateCube() {
      for (int z = 0; z < PlayerSettings.CubeSize; z++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int x = 0; x < PlayerSettings.CubeSize; x++) {
               SmallCube newSmallCube = Instantiate(smallCubePrefab) as SmallCube;
               smallCubes[x, y, z] = newSmallCube;
               newSmallCube.transform.parent = transform;
               newSmallCube.transform.localPosition = new Vector3(x - PlayerSettings.CubeSize * 0.5f + 0.5f, y - PlayerSettings.CubeSize * 0.5f + 0.5f, z - PlayerSettings.CubeSize * 0.5f + 0.5f);
               SetActiveCubelets(newSmallCube, x, y, z);
               smallCubes[x, y, z].SetMaterials();
            }
         }
      }
   }

   public bool CheckWinCondition() {
      List<Cubelet> cubelets = new List<Cubelet>();
      for (int z = 0; z < PlayerSettings.CubeSize; z++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int x = 0; x < PlayerSettings.CubeSize; x++) {
               smallCubes[x, y, z].GetCubeletsInPlay(cubelets);
            }
         }
      }

      CubeletColors southColor = CubeletColors.Black;
      CubeletColors northColor = CubeletColors.Black;
      CubeletColors eastColor = CubeletColors.Black;
      CubeletColors westColor = CubeletColors.Black;
      CubeletColors topColor = CubeletColors.Black;
      CubeletColors bottomColor = CubeletColors.Black;
      foreach (Cubelet cubelet in cubelets) {
         if (cubelet.direction == CubeletDirection.South)
            southColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.North)
            northColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.East)
            eastColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.West)
            westColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.Top)
            topColor = cubelet.color;
         if (cubelet.direction == CubeletDirection.Bottom)
            bottomColor = cubelet.color;
      }

      foreach (Cubelet cubelet in cubelets) {
         if (cubelet.direction == CubeletDirection.South)
            if (cubelet.color != southColor) 
               return false;
         if (cubelet.direction == CubeletDirection.North)
            if (cubelet.color != northColor)
               return false;
         if (cubelet.direction == CubeletDirection.East)
            if (cubelet.color != eastColor)
               return false;
         if (cubelet.direction == CubeletDirection.West)
            if (cubelet.color != westColor)
               return false;
         if (cubelet.direction == CubeletDirection.Top)
            if (cubelet.color != topColor)
               return false;
         if (cubelet.direction == CubeletDirection.Bottom)
            if (cubelet.color != bottomColor)
               return false;
      }

      return true;
  }

   public IEnumerator ScrambleCube(int scrambleTimes, float scrambleRotationTime) {
      PlayerSettings.Scrambling = true;
      float oldRotationTime = rotationTime;
      rotationTime = scrambleRotationTime;

      for (int i = 0; i < scrambleTimes; i++) {
         PlayerSettings.FaceRotation = true;
         int rotationType = Random.Range(0, 3);
         int rotationIndex = Random.Range(0, PlayerSettings.CubeSize);
         int rotationAngle = Random.Range(-1, 1) < 0 ? -90 : 90;
         switch (rotationType) {
            case 0:
               yield return StartCoroutine(RotateAlongX(rotationAngle, rotationIndex));
               break;
            case 1:
               yield return StartCoroutine(RotateAlongY(rotationAngle, rotationIndex));
               break;
            case 2:
               yield return StartCoroutine(RotateAlongZ(rotationAngle, rotationIndex));
               break;
            default:
               break;
         }
      }

      rotationTime = oldRotationTime;
      PlayerSettings.Scrambling = false;
   }

   public IEnumerator RotateAlongY(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true;
         GameObject newRotation = new GameObject();
         newRotation.transform.position = new Vector3(0f, 0f, 0f);
         float elapsedTime = 0;

         // Unparent the cubes to be rotated
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[x, rotationIndex, z].transform.parent = newRotation.transform;
            }
         }

         // Rotate
         Quaternion quaternion = Quaternion.Euler(0f, angle, 0f);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // Parent back the rotated cubes
         newRotation.transform.rotation = quaternion;
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[x, rotationIndex, z].transform.parent = transform;
            }
         }

         // Fix the location of the rotated cubes in the array and set the directions they now face
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'Y');
         
         Destroy(newRotation);
         currentlyRotating = false;

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon();
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }
   public IEnumerator RotateAlongX(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true;
         GameObject newRotation = new GameObject();
         newRotation.transform.position = new Vector3(0f, 0f, 0f);
         float elapsedTime = 0;

         // Unparent the cubes to be rotated
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[rotationIndex, y, z].transform.parent = newRotation.transform;
            }
         }

         // Rotate
         Quaternion quaternion = Quaternion.Euler(angle, 0f, 0f);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // Parent back the rotated cubes
         newRotation.transform.rotation = quaternion;
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               smallCubes[rotationIndex, y, z].transform.parent = transform;
            }
         }

         // Fix the location of the rotated cubes in the array and set the directions they now face
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'X');

         Destroy(newRotation);
         currentlyRotating = false;

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon();
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }
   public IEnumerator RotateAlongZ(float angle, int rotationIndex) {
      if (!currentlyRotating && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && PlayerSettings.FaceRotation) {
         currentlyRotating = true;
         GameObject newRotation = new GameObject();
         newRotation.transform.position = new Vector3(0f, 0f, 0f);
         float elapsedTime = 0;

         // Unparent the cubes to be rotated
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               smallCubes[x, y, rotationIndex].transform.parent = newRotation.transform;
            }
         }

         // Rotate
         Quaternion quaternion = Quaternion.Euler(0f, 0f, angle);
         while (elapsedTime < rotationTime) {
            newRotation.transform.rotation = Quaternion.Lerp(newRotation.transform.rotation, quaternion, (elapsedTime / rotationTime));
            elapsedTime += Time.deltaTime;
            yield return null;
         }

         // Parent back the rotated cubes
         newRotation.transform.rotation = quaternion;
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               smallCubes[x, y, rotationIndex].transform.parent = transform;
            }
         }

         // Fix the location of the rotated cubes in the array and set the directions they now face
         smallCubes = ResetPositionAfterRotation();
         ChangeCubeletsDirections(angle, rotationIndex, 'Z');

         Destroy(newRotation);
         currentlyRotating = false;

         if (!PlayerSettings.Scrambling && CheckWinCondition()) {
            gameManager.GameWasWon();
         }

         PlayerSettings.FaceRotation = false;
         yield return new WaitForSeconds(0.1f);
      }
   }

   private SmallCube[,,] ResetPositionAfterRotation() {

      float multi = PlayerSettings.CubeSize / 2f - 0.5f;
      SmallCube[,,] newSmallCubes = new SmallCube[PlayerSettings.CubeSize, PlayerSettings.CubeSize, PlayerSettings.CubeSize];

      for (int x = 0; x < PlayerSettings.CubeSize; x++) {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {

               for (int x2 = 0; x2 < PlayerSettings.CubeSize; x2++) {
                  for (int y2 = 0; y2 < PlayerSettings.CubeSize; y2++) {
                     for (int z2 = 0; z2 < PlayerSettings.CubeSize; z2++) {
                        
                        if (smallCubes[x2, y2, z2].transform.position == new Vector3(-multi + x, -multi + y, -multi + z)) {
                           newSmallCubes[x, y, z] = smallCubes[x2, y2, z2];
                        }

                     }
                  }
               }

            }
         }
      }

      return newSmallCubes;
   }
   private void ChangeCubeletsDirections(float angle, int rotationIndex, char rotationAlong) {
      // Rotate along X
      if (rotationAlong == 'X') {
         for (int y = 0; y < PlayerSettings.CubeSize; y++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               if (angle > 0) {
                  smallCubes[rotationIndex, y, z].ChangeDirectionsAfterXRotationClockwise();
               }
               else {
                  smallCubes[rotationIndex, y, z].ChangeDirectionsAfterXRotationCounterClockwise();
               }
            }
         }
      }

      // Rotate along Y
      else if (rotationAlong == 'Y') {
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int z = 0; z < PlayerSettings.CubeSize; z++) {
               if (angle > 0) {
                  smallCubes[x, rotationIndex, z].ChangeDirectionsAfterYRotationClockwise();
               }
               else {
                  smallCubes[x, rotationIndex, z].ChangeDirectionsAfterYRotationCounterClockwise();
               }
            }
         }
      }

      // Rotate along Z
      else if (rotationAlong == 'Z') {
         for (int x = 0; x < PlayerSettings.CubeSize; x++) {
            for (int y = 0; y < PlayerSettings.CubeSize; y++) {
               if (angle > 0) {
                  smallCubes[x, y, rotationIndex].ChangeDirectionsAfterZRotationClockwise();
               }
               else {
                  smallCubes[x, y, rotationIndex].ChangeDirectionsAfterZRotationCounterClockwise();
               }
            }
         }
      }
   }

   private void SetActiveCubelets(SmallCube cube, int x, int y, int z) {
      // Bottom Row
      if (x == 0 && y == 0 && z == 0) {
         cube.SetBottomWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z == 0) {
         cube.SetSouthBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z == 0) {
         cube.SetBottomEastSouthCorner();
      }
      if (x == 0 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestBottomSide();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetBottomMiddle();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastBottomSide();
      }
      if (x == 0 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetBottomWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == 0 && z == PlayerSettings.CubeSize - 1) {
         cube.SetBottomEastNorthCorner();
      }

      // Middle Row
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetMiddleWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetSouthMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetMiddleEastSouthCorner();
      }
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastMiddleSide();
      }
      if (x == 0 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetMiddleWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthMiddleSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y > 0 && y < PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetMiddlemEastNorthCorner();
      }

      // Top Row
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopWestSouthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopBottomSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == 0) {
         cube.SetTopEastSouthCorner();
      }
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetWestTopSide();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetTopMiddle();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z > 0 && z < PlayerSettings.CubeSize - 1) {
         cube.SetEastTopSide();
      }
      if (x == 0 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetTopWestNorthCorner();
      }
      if (x > 0 && x < PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetNorthTopSide();
      }
      if (x == PlayerSettings.CubeSize - 1 && y == PlayerSettings.CubeSize - 1 && z == PlayerSettings.CubeSize - 1) {
         cube.SetTopEastNorthCorner();
      }
   }
   
}


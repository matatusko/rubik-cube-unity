using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
   
   public float orbitDampening = 10f;
   public float cameraDistance = 10f;
   public float perspectiveZoomSpeed = 0.5f;

   private Transform cameraPivot;
   private Vector3 localRotation;

   private BigCube bigCube;
   private GameObject firstHit;
   private Vector3 firstHitNormal;
   private Vector3 firstHitCenter;
   private GameObject secondHit;
   private Vector3 secondHitNormal;
   private Vector3 secondHitCenter;
   private float offset;
   private float rotationAngle = 90f;

   // Use this for initialization
   void Start () {
      PlayerSettings.CameraDisable = true;
      PlayerSettings.CubeRotation = false;
      this.cameraPivot = this.transform.parent;
      bigCube = FindObjectOfType<BigCube>();
      offset = PlayerSettings.CubeSize * 0.5f - 0.5f;
   }
	
	void LateUpdate () {
      if (Input.touchCount > 0) {
         Touch touch = Input.GetTouch(0);
         
         // Check if the cube was touched
         Ray whatCubeTouched = Camera.main.ScreenPointToRay(touch.position);
         RaycastHit initialTest = new RaycastHit();
         bool cubeWasTouched = false;
         if (Physics.Raycast(whatCubeTouched, out initialTest)) {
            cubeWasTouched = initialTest.transform.gameObject.GetComponent<Cubelet>().inPlay;
         }

         if (cubeWasTouched && Input.touchCount == 1) {

            switch (touch.phase) {
               // Record initial touch position.
               case TouchPhase.Began:
                  if (!PlayerSettings.CubeRotation) {
                     PlayerSettings.FaceRotation = true;
                     Ray ray = Camera.main.ScreenPointToRay(touch.position);

                     RaycastHit hit;
                     if (Physics.Raycast(ray, out hit, 100)) {
                        firstHitNormal = hit.normal;
                        firstHitCenter = hit.transform.gameObject.GetComponent<Renderer>().bounds.center;
                        firstHit = hit.transform.parent.gameObject;
                     }
                  }
                  break;

               // Report that a direction has been chosen when the finger is lifted.
               case TouchPhase.Ended:
                  if (PlayerSettings.FaceRotation == true && !PlayerSettings.CubeRotation) {
                     Ray ray = Camera.main.ScreenPointToRay(touch.position);

                     RaycastHit hit;
                     if (Physics.Raycast(ray, out hit, 100)) {
                        secondHitNormal = hit.normal;
                        secondHitCenter = hit.transform.gameObject.GetComponent<Renderer>().bounds.center;
                        secondHit = hit.transform.parent.gameObject;
                     }
                     Vector3 move = secondHitCenter - firstHitCenter;
                     move.Normalize();

                     DoTheRotation(move);
                  }
                  break;
            }
         }

         else {
            // Operate the camera orbit movement
            if (PlayerSettings.CameraDisable && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon && !PlayerSettings.GyroOn) {
               if (Input.touchCount == 1 &&
                  !(Input.touches[0].position.x > Screen.width * 0.80 && Input.touches[0].position.y < Screen.height * 0.20) &&
                  !(Input.touches[0].position.x < Screen.width * 0.20 && Input.touches[0].position.y < Screen.height * 0.20)) {
                  localRotation.x += Input.touches[0].deltaPosition.x;
                  localRotation.y -= Input.touches[0].deltaPosition.y;

                  PlayerSettings.CubeRotation = true;
               }
            }

            // Deal with zoom in and zoom out
            if (Input.touchCount == 2 && !PlayerSettings.SettingsOn && !PlayerSettings.GameWon) {
               Touch touchZero = Input.GetTouch(0);
               Touch touchOne = Input.GetTouch(1);
               Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
               Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
               float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
               float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
               float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
               if (deltaMagnitudeDiff > 5f || deltaMagnitudeDiff < -5f) {
                  Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed * 0.5f;
                  Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 90f);
               }
            }

            // Actual Camera Rig Transformation
            if (PlayerSettings.CubeRotation) {
               Quaternion targetLocation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
               this.cameraPivot.rotation = Quaternion.Slerp(this.cameraPivot.rotation, targetLocation, Time.deltaTime * orbitDampening);
            }

            PlayerSettings.CubeRotation = false;
         }
      }
   }

   private bool ConfirmWhichRotation(Vector3 normal, Vector3 tester, Vector3 move, char axis) {
      //Debug.Log("Normal: " + normal + " | tester: " + tester + " | move: " + move + " | axis: " + axis);
      Vector3 sum = normal + tester;
      if (axis == 'X') {
         sum = new Vector3(Mathf.Abs(move.x), Mathf.Abs(sum.y), Mathf.Abs(sum.z));
      }
      else if (axis == 'Y') {
         sum = new Vector3(Mathf.Abs(sum.x), Mathf.Abs(move.y), Mathf.Abs(sum.z));
      }
      else if (axis == 'Z') {
         sum = new Vector3(Mathf.Abs(sum.x), Mathf.Abs(sum.y), Mathf.Abs(move.z));
      }
      return sum == new Vector3(1, 1, 1);
   }

   private bool CheckForHitOnDifferentPlanes(Vector3 fromNormal, Vector3 fromCompare, Vector3 toNormal, Vector3 toCompare) {
      fromNormal = new Vector3(Mathf.Abs(fromNormal.x), Mathf.Abs(fromNormal.y), Mathf.Abs(fromNormal.z));
      toNormal = new Vector3(Mathf.Abs(toNormal.x), Mathf.Abs(toNormal.y), Mathf.Abs(toNormal.z));
      //Debug.Log("From Vector: " + fromNormal + " | fromCompare: " + fromCompare);
      //Debug.Log("to Normal: " + toNormal + " | toComapre: " + toCompare);
      return (fromNormal == fromCompare && toNormal == toCompare);
   }

   private void DoTheRotation(Vector3 move) {

      if (firstHitNormal == secondHitNormal) {
         if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 0, 1), move, 'Y')) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.x * move.y * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 1, 0), move, 'Z')) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.x * move.z * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
            //Debug.Log("2nd: First hit " + firstHitNormal + " | move.y: " + move);
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 0, 1), move, 'X')) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.y * move.x * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
            //Debug.Log("3rd: First hit " + firstHitNormal + " | move.y: " + move);
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(1, 0, 0), move, 'Z')) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.y * move.z * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
            //Debug.Log("4th: First hit " + firstHitNormal + " | move.y: " + move);
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(0, 1, 0), move, 'X')) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.z * move.x * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
            //Debug.Log("5th: First hit " + firstHitNormal + " | move.y: " + move);
         }
         else if (ConfirmWhichRotation(firstHitNormal, new Vector3(1, 0, 0), move, 'Y')) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.z * move.y * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
            //Debug.Log("6th: First hit " + firstHitNormal + " | move.y: " + move);
         }
      }
      else {
         if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 0, 1), secondHitNormal, new Vector3(0, 1, 0))) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.z * secondHitNormal.y * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
            //Debug.Log("1 ----");
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 1, 0), secondHitNormal, new Vector3(0, 0, 1))) {
            StartCoroutine(bigCube.RotateAlongX(firstHitNormal.y * secondHitNormal.z * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.x + offset)));
            //Debug.Log("2 ----");
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 0, 1), secondHitNormal, new Vector3(1, 0, 0))) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.z * secondHitNormal.x * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
            //Debug.Log("3 ----");
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(1, 0, 0), secondHitNormal, new Vector3(0, 0, 1))) {
            StartCoroutine(bigCube.RotateAlongY(firstHitNormal.x * secondHitNormal.z * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.y + offset)));
            //Debug.Log("4 ----");
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(0, 1, 0), secondHitNormal, new Vector3(1, 0, 0))) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.y * secondHitNormal.x * -rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
            //Debug.Log("5 ----");
         }
         else if (CheckForHitOnDifferentPlanes(firstHitNormal, new Vector3(1, 0, 0), secondHitNormal, new Vector3(0, 1, 0))) {
            StartCoroutine(bigCube.RotateAlongZ(firstHitNormal.x * secondHitNormal.y * rotationAngle, Mathf.RoundToInt(firstHit.transform.position.z + offset)));
            //Debug.Log("6 ----");
         }
      }
   }

   public void toggleCameraSettings() {
      PlayerSettings.CameraDisable = !PlayerSettings.CameraDisable;
   }
}

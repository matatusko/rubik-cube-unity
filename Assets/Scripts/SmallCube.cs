using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SmallCube : MonoBehaviour {

   public Material[] materials;
   private Cubelet[] cubelets;

   private void Awake() {
      cubelets = GetComponentsInChildren<Cubelet>();
      for (int i = 0; i < cubelets.Length; i++) {
         cubelets[i].direction = (CubeletDirection)Enum.Parse(typeof(CubeletDirection), cubelets[i].name, true);
      }
   }

   private void Start() {
   }

   public void GetCubeletsInPlay(List<Cubelet> cubes) {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            cubes.Add(cubelets[i]);
         }
      }
   }

   public void ChangeDirectionsAfterYRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   public void ChangeDirectionsAfterYRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   public void ChangeDirectionsAfterXRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.Bottom;
            else if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   public void ChangeDirectionsAfterXRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.South)
               cubelets[i].direction = CubeletDirection.Bottom;
            else if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.North;
            else if (cubelets[i].direction == CubeletDirection.North)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.South;
         }
      }
   }
   public void ChangeDirectionsAfterZRotationClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.Bottom;
         }
      }
   }
   public void ChangeDirectionsAfterZRotationCounterClockwise() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom)
               cubelets[i].direction = CubeletDirection.West;
            else if (cubelets[i].direction == CubeletDirection.West)
               cubelets[i].direction = CubeletDirection.Top;
            else if (cubelets[i].direction == CubeletDirection.Top)
               cubelets[i].direction = CubeletDirection.East;
            else if (cubelets[i].direction == CubeletDirection.East)
               cubelets[i].direction = CubeletDirection.Bottom;
         }
      }
   }

   public void SetMaterials() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].inPlay) {
            if (cubelets[i].direction == CubeletDirection.Bottom) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.Bottom];
               cubelets[i].color = CubeletColors.Yellow;
            }
            if (cubelets[i].direction == CubeletDirection.Top) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.Top];
               cubelets[i].color = CubeletColors.White;
            }
            if (cubelets[i].direction == CubeletDirection.North) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.North];
               cubelets[i].color = CubeletColors.Green;
            }
            if (cubelets[i].direction == CubeletDirection.South) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.South];
               cubelets[i].color = CubeletColors.Blue;
            }
            if (cubelets[i].direction == CubeletDirection.East) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.East];
               cubelets[i].color = CubeletColors.Red;
            }
            if (cubelets[i].direction == CubeletDirection.West) {
               cubelets[i].GetComponent<MeshRenderer>().material = materials[(int)CubeletDirection.West];
               cubelets[i].color = CubeletColors.Orange;
            }
         }
         else {
            cubelets[i].GetComponent<MeshRenderer>().material = materials[materials.Length - 1];
            cubelets[i].color = CubeletColors.Black;
         }
      }
   }

   // Bottom Row
   public void SetBottomWestSouthCorner() {
      for(int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetBottomEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetBottomWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetBottomEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetSouthBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetEastBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.East) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetWestBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.West) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetNorthBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetBottomMiddle() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Bottom) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   
   // Middle Row
   public void SetMiddleWestSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.South || cubelets[i].direction == CubeletDirection.West) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetSouthMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetMiddleEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetWestMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.West) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetEastMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.East) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetMiddleWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetNorthMiddleSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetMiddlemEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.North || cubelets[i].direction == CubeletDirection.East) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }

   // Top Row
   public void SetTopWestSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetTopEastSouthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetTopWestNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetTopEastNorthCorner() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetTopBottomSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.South) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetEastTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.East) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetWestTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.West) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetNorthTopSide() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top || cubelets[i].direction == CubeletDirection.North) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
   public void SetTopMiddle() {
      for (int i = 0; i < cubelets.Length; i++) {
         if (cubelets[i].direction == CubeletDirection.Top) {
            cubelets[i].inPlay = true;
         }
         else {
            cubelets[i].inPlay = false;
            Destroy(cubelets[i].GetComponent<Collider>());
         }
      }
   }
}


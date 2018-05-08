using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings {

   private static int cubeSize;
   private static bool settingsOn;
   private static bool gameWon;
   private static bool timerOn;
   private static bool cameraDisable;
   private static bool faceRotation;
   private static bool cubeRotation;
   private static bool gyroOn;
   private static bool scrambling;

   public static int CubeSize {
      get {
         return cubeSize;
      }
      set {
         cubeSize = value;
      }
   }
   public static bool SettingsOn {
      get {
         return settingsOn;
      }
      set {
         settingsOn = value;
      }
   }
   public static bool GameWon {
      get {
         return gameWon;
      }
      set {
         gameWon = value;
      }
   }
   public static bool TimerOn {
      get {
         return timerOn;
      }
      set {
         timerOn = value;
      }
   }
   public static bool CameraDisable {
      get {
         return cameraDisable;
      }
      set {
         cameraDisable = value;
      }
   }
   public static bool FaceRotation {
      get {
         return faceRotation;
      }
      set {
         faceRotation = value;
      }
   }
   public static bool CubeRotation {
      get {
         return cubeRotation;
      }
      set {
         cubeRotation = value;
      }
   }
   public static bool GyroOn {
      get {
         return gyroOn;
      }
      set {
         gyroOn = value;
      }
   }
   public static bool Scrambling {
      get {
         return scrambling;
      }
      set {
         scrambling = value;
      }
   }
}

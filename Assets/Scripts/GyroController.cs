using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour {

   public GameObject cameraPivot;

   private bool gyroAvailable;
   private Gyroscope gyro;
   private Quaternion rot;

   private void Start() {
      LookForGyro();
   }

   private void Update() {
      if (gyroAvailable && PlayerSettings.GyroOn) {
         cameraPivot.transform.rotation = gyro.attitude * rot;
      }
   }

   private bool LookForGyro() {
      if (SystemInfo.supportsGyroscope) {
         gyro = Input.gyro;
         gyro.enabled = true;
         gyroAvailable = true;

         rot = new Quaternion(0, 0, 2, 0);

         return true;
      }

      gyroAvailable = false;
      return false;
   }
	
}

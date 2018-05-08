using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChanger : MonoBehaviour {

   public Sprite[] buttonFaces;
   
   private Button button;

   private void Awake() {
      button = GetComponent<Button>();
   }

   public void SwitchRotationButtons() {
      if (!PlayerSettings.SettingsOn && !PlayerSettings.GameWon) {
         if (button.image.sprite == buttonFaces[0]) {
            button.image.sprite = buttonFaces[1];
            PlayerSettings.CameraDisable = false;
         }
         else {
            button.image.sprite = buttonFaces[0];
            PlayerSettings.CameraDisable = true;
         }
      }
   }

   public void SwitchTimerButton() {
      if (PlayerSettings.TimerOn) {
         PlayerSettings.TimerOn = false;
         GetComponentInChildren<Text>().text = "Timer: OFF";
      }
      else {
         PlayerSettings.TimerOn = true;
         GetComponentInChildren<Text>().text = "Timer: ON";
      }
   }

   public void SwitchGyroButton() {
      if (PlayerSettings.GyroOn) {
         PlayerSettings.GyroOn = false;
         GetComponentInChildren<Text>().text = "Gyroscope: OFF";
      }
      else {
         PlayerSettings.GyroOn = true;
         GetComponentInChildren<Text>().text = "Gyroscope: ON";
      }
   }
}

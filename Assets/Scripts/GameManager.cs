using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
   
   public int scrambleTimes;
   public float scrambleRotationTime;
   public BigCube bigCubePrefab;
   public GameObject winMessage;
   public GameObject settings;
   public Image backgroundImage;
   public Text timer;
   public Text backToGame;
   
   private BigCube bigCubeInstance;
   private bool cameraModeBefore;
   private float time;
   private int seconds;
   private int minutes;
   private string timeSoFar;

	// Use this for initialization
	void Awake () {
      //PlayerSettings.CubeSize = 3;
      PlayerSettings.TimerOn = true;
      PlayerSettings.GyroOn = false;
      cameraModeBefore = false;
      BeginGame();
   }

   private void BeginGame() {
      PlayerSettings.SettingsOn = false;
      PlayerSettings.GameWon = false;
      PlayerSettings.FaceRotation = false;
      PlayerSettings.CubeRotation = false;
      PlayerSettings.Scrambling = false;
      bigCubeInstance = Instantiate(bigCubePrefab) as BigCube;
      bigCubeInstance.transform.position = transform.position;
      bigCubeInstance.GenerateCube();
      Invoke("ScrambleCube", 0.5f);
   }
	
	// Update is called once per frame
	void Update () {
      if (!PlayerSettings.SettingsOn && !PlayerSettings.GameWon && !PlayerSettings.Scrambling) {
         time += Time.deltaTime;
      }

      if (PlayerSettings.TimerOn) {
         minutes = Mathf.FloorToInt(time / 60F);
         seconds = Mathf.FloorToInt(time - minutes * 60);
         timeSoFar = string.Format("{0:0}:{1:00}", minutes, seconds);

         timer.text = "Time: " + timeSoFar;
      }
      else {
         timer.text = "";
      }
   }

   public void GameWasWon() {
      winMessage.gameObject.SetActive(true);
      PlayerSettings.GameWon = true;
   }

   public void ToggleSettings() {
      if (!PlayerSettings.GameWon && !PlayerSettings.Scrambling) {
         if (!PlayerSettings.SettingsOn) {
            cameraModeBefore = PlayerSettings.CameraDisable;
            PlayerSettings.SettingsOn = true;
            PlayerSettings.CameraDisable = true;
            settings.SetActive(true);
            backToGame.gameObject.SetActive(true);
            backgroundImage.color = new Color(0.4f, 0.4f, 0.4f);
         }
         else {
            settings.SetActive(false);
            backToGame.gameObject.SetActive(false);
            PlayerSettings.SettingsOn = false;
            PlayerSettings.FaceRotation = false;
            PlayerSettings.CameraDisable = cameraModeBefore;
            backgroundImage.color = new Color(1f, 1f, 1f);
         }
      }
   }

   public void ScrambleCube() {
      if (PlayerSettings.SettingsOn) {
         ToggleSettings();
      }
      StartCoroutine(bigCubeInstance.ScrambleCube(scrambleTimes, scrambleRotationTime));
      time = 0.0f;
   }

   public void RestartGame() {
      StopAllCoroutines();
      winMessage.gameObject.SetActive(false);
      backToGame.gameObject.SetActive(false);
      settings.SetActive(false);
      backgroundImage.color = new Color32(255, 255, 255, 255);
      Destroy(bigCubeInstance.gameObject);
      SceneManager.LoadScene(1);
   }

   public void ReturnToMenu() {
      if (PlayerSettings.SettingsOn) {
         ToggleSettings();
      }
      PlayerSettings.GameWon = false;
      StopAllCoroutines();
      Destroy(bigCubeInstance.gameObject);
      SceneManager.LoadScene(0);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

   public GameObject introScreen;
   public GameObject chooseSizeScreen;

   public void intoChooseSize() {
      introScreen.SetActive(false);
      chooseSizeScreen.SetActive(true);
   }

   public void LoadCube2(int index) {
      PlayerSettings.CubeSize = 2;
      SceneManager.LoadScene(index);
   }
   public void LoadCube3(int index) {
      PlayerSettings.CubeSize = 3;
      SceneManager.LoadScene(index);
   }
   public void LoadCube4(int index) {
      PlayerSettings.CubeSize = 4;
      SceneManager.LoadScene(index);
   }

   public void QuitGame() {
      StopAllCoroutines();
      Application.Quit();
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadScene(string sceneName){
     SceneManager.LoadScene(sceneName);
     Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitGame(){
     Application.Quit();
        Debug.Log("QUIT");
    }
}

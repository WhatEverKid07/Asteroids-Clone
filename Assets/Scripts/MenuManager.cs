using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, 0.3f));
    }
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitGame(){
     Application.Quit();
        Debug.Log("QUIT");
    }
}

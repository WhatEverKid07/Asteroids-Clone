using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool gameIsPaused = false;
    public string mainMenu;

    public EventSystem eventSystem;
    public GameObject selectedButtonInPauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
       
    void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        eventSystem.SetSelectedGameObject(selectedButtonInPauseMenu);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = false;
        StartCoroutine(BackToMenuPause());
    }

    IEnumerator BackToMenuPause()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(mainMenu);
    }
}

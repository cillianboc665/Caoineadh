using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraMovement cameraMovement = FindFirstObjectByType<CameraMovement>();
        if (cameraMovement != null)
            cameraMovement.enabled = true;
    }
    void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CameraMovement cameraMovement = FindFirstObjectByType<CameraMovement>();
        if (cameraMovement != null)
            cameraMovement.enabled = false;
    }

    public void Quit()

    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("placeholder-titlescreen");
    }
    public void Restart()
    {
        isPaused = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

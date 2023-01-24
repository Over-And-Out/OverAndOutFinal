using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGame : MonoBehaviour
{
    public GameObject inGameMenu;
    public GameObject WalkieCanvas;

    public static bool GameIsPaused = false;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (WalkieCanvas.activeSelf)
                WalkieCanvas.SetActive(false);
            if (GameIsPaused)
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

        Cursor.lockState = CursorLockMode.Locked;
        inGameMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        inGameMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

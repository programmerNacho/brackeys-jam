using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu = null;
    [SerializeField]
    private GameObject pauseMenu = null;

    private void Start()
    {
        OpenMainMenu();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && mainMenu.activeSelf == false)
        {
            if(pauseMenu.activeSelf)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
    }

    private void OpenMainMenu()
    {
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject mainMenu = null;
    [SerializeField]
    private GameObject pauseMenu = null;
    [SerializeField]
    private GameObject failMenu = null;
    [SerializeField]
    private GameObject winMenu = null;



    public enum Menu
    {
        play,
        main,
        pause,
        fail,
        win
    }
    private Menu currentMenu = Menu.main;

    private void Start()
    {
        OpenMainMenu();
    }

    public void SetCurrentMenu(Menu currentMenu)
    {
        this.currentMenu = currentMenu;

        ResetMenu();
        switch (currentMenu)
        {
            case Menu.play:
                Time.timeScale = 1f;
                break;
            case Menu.main:
                Time.timeScale = 0f;
                mainMenu.SetActive(true);
                break;
            case Menu.pause:
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                break;
            case Menu.fail:
                Time.timeScale = 0f;
                failMenu.SetActive(true);
                break;
            case Menu.win:
                Time.timeScale = 0f;
                winMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void ResetMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        failMenu.SetActive(false);
        winMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && mainMenu.activeSelf == false)
        {
            if (currentMenu == Menu.pause)
            {
                SetCurrentMenu(Menu.play);
            }
            else
            {
                SetCurrentMenu(Menu.pause);
            }
        }

            //if(Input.GetKeyDown(KeyCode.P) && mainMenu.activeSelf == false)
            //{
            //    if(pauseMenu.activeSelf)
            //    {
            //        ClosePauseMenu();
            //    }
            //    else
            //    {
            //        OpenPauseMenu();
            //    }
            //}
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

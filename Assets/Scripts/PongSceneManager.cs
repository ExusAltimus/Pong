using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class PongSceneManager {

    private const string PONG_LEVEL_NAME = "Pong";
    private const string MAIN_MENU_LEVEL_NAME = "MainMenu";
    // Use this for initialization

    public static void PlayPong()
    {
        SceneManager.LoadScene(PONG_LEVEL_NAME);
    }

    public static void MainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_LEVEL_NAME);
    }

    public static void Exit()
    {
        Application.Quit();
    }
}

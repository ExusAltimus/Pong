using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerMode { HumanVersusAi, HumanVersusHuman, AiVersusAi };
//public enum GameMode { Normal, Sezuire };

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; } //Singleton; ugly, but better than static fields
    public bool Hitmarker;
    public bool SezuireMode;
    public float GameSpeed;
    public float AiDifficulty;
    public float BallSpeedIncrement;
    public PlayerMode PlayerMode;
    public string Message;
    //public GameMode GameMode;

    public void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(this.gameObject); //So I can debug from any scene
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            PongSceneManager.MainMenu();
        }
    }
}

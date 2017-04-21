using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pong : MonoBehaviour {

    public GameObject PaddleModel;
    public GameObject BallModel;

    public List<IPlayer> Players;
    public IBall Ball;
    public IMap Map;
    public bool IsPlaying = false;

    // Use this for initialization
    void Start () {
        IsPlaying = false;
        Map = new StaticCameraMap(Camera.main); //New map
        
        StartCoroutine(StartGame());
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsPlaying)
            return;

        //Input (Could be put in their own update functions as well)
        foreach (var player in Players)
        {
            player.HandleInput();
        }

        //Movement (Could be put in their own update functions as well)
        foreach (var player in Players)
        {
            player.MovementUpdate();
        }
        Ball.MovementUpdate();

        //Collisions
        //Don't go off screen
        foreach (var player in Players)
        {
            player.CheckMapEdgeCollision(Map);
        }

        Vector2 ballDirection = Ball.GetDirection();
        //Check direction
        if (ballDirection.x < 0)
        {
            //Check player 1 collision
            Ball.CheckPlayerCollison(Players[0]);
        }
        else
        {
            //Check player 2 collision
            Ball.CheckPlayerCollison(Players[1]);
        }

        OutOfBounds ballOutOfBounds = Map.CheckOutOfBounds(Ball.Bounds);
        if (ballOutOfBounds == OutOfBounds.Left)
        {
            //Player 2 wins
            EndGame();
        }
        else if (ballOutOfBounds == OutOfBounds.Right)
        {
            //Player 1 wins
            EndGame();
        }
    }

    IEnumerator StartGame()  {
        print("Starting game.");
        yield return new WaitForSeconds(1.5f);
        print("Start!");
        Players.Clear();
        Ball.Initialize();
        IsPlaying = true;
    }

    public void AiVersusAi()
    {
        Players.Add(new AiPlayer());
        Players.Add(new AiPlayer());
    }

    public void HumanVersusAi()
    {
        Players.Add(new HumanPlayer());
        Players.Add(new AiPlayer());
    }

    public void HumanVersusHuman()
    {
        Players.Add(new HumanPlayer());
        Players.Add(new HumanPlayer(KeyCode.UpArrow, KeyCode.DownArrow));
    }

    public void EndGame()
    {
        PongSceneManager.MainMenu();
    }
}

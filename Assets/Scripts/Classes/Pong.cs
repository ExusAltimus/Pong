using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pong : MonoBehaviour {

    public Player Player1;
    public Player Player2;
    public Ball Ball;

    public Bounds CameraBounds;


    // Use this for initialization
    void Start () {
        StartCoroutine(StartGame());
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        CameraBounds = Camera.main.OrthographicBounds();
    }
	
	// Update is called once per frame
	void Update () {

        //Input (Could be put in their own update functions as well)
        HandleInput();

        //Movement (Could be put in their own update functions as well)
        MovementUpdate();

        //Collisions
        HandleCollisions();
    }

    private void HandleInput()
    {
        Player1.HandleInput();
        Player2.HandleInput();

    }

    private void MovementUpdate()
    {
        Player1.MovementUpdate();
        Player2.MovementUpdate();
        Ball.MovementUpdate();
    }

    private void HandleCollisions()
    {
        //Don't go off screen
        Player1.CheckMapCollision(CameraBounds);
        Player2.CheckMapCollision(CameraBounds);

        //Check direction
        if (Ball.Velocity.x < 0)
        {
            //Check player 1 collision
            Ball.CheckPlayerCollison(Player1.Collider.bounds);
        }
        else
        {
            //Check player 2 collision
            Ball.CheckPlayerCollison(Player2.Collider.bounds);
        }

        if (Ball.CheckMapCollision(CameraBounds)) //Ball out of bounds
        {
            EndGame();
        }
    }

    IEnumerator StartGame()  {
        print("Starting game.");
        yield return new WaitForSeconds(1.5f);
        print("Start!");
        Ball.Initilaize();
    }

    public void EndGame()
    {
        PongSceneManager.MainMenu();
    }
}

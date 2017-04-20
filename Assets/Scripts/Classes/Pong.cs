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

        //Input
        Player1.HandleInput();
        Player2.HandleInput();

        //Movement
        Player1.MovementUpdate();
        Player2.MovementUpdate();

        //Don't go off screen
        if (Player1.Collider.bounds.max.y > CameraBounds.max.y)
        {
            Player1.MoveTo(new Vector2(Player1.Collider.bounds.center.x, CameraBounds.max.y - Player1.Collider.bounds.extents.y));
        }
        else if (Player1.Collider.bounds.min.y < CameraBounds.min.y)
        {
            Player1.MoveTo(new Vector2(Player1.Collider.bounds.center.x, CameraBounds.min.y + Player1.Collider.bounds.extents.y));
        }

        if (Player2.Collider.bounds.max.y > CameraBounds.max.y)
        {
            Player2.MoveTo(new Vector2(Player2.Collider.bounds.center.x, CameraBounds.max.y - Player2.Collider.bounds.extents.y));
        }
        else if (Player2.Collider.bounds.min.y < CameraBounds.min.y)
        {
            Player2.MoveTo(new Vector2(Player2.Collider.bounds.center.x, CameraBounds.min.y + Player2.Collider.bounds.extents.y));
        }

        //Collisions
        HandleCollisions();
    }



    private void HandleCollisions()
    {

        //Check direction
        if (Ball.Velocity.x < 0)
        {
            //Check player 1 collision
            Ball.CheckPlayerCollison(Player1.Collider);
        }
        else
        {
            //Check player 2 collision
            Ball.CheckPlayerCollison(Player2.Collider);
        }


        //Check bound collision
        if (Ball.Collider.bounds.max.y > CameraBounds.max.y)
        {
            Ball.Velocity.y = Ball.Velocity.y * -1;
            print("Top collision");
        }
        else if (Ball.Collider.bounds.min.y < CameraBounds.min.y)
        {
            Ball.Velocity.y = Ball.Velocity.y * -1;
            print("Bottom collision");
        }
        else if (Ball.Collider.bounds.max.x < CameraBounds.min.x)
        {
            print("Player 2 wins");
            EndGame();
        }
        else if (Ball.Collider.bounds.min.x > CameraBounds.max.x)
        {
            print("Player 1 wins");
            EndGame();
        }
    }

    IEnumerator StartGame()  {
        print("Starting game.");
        yield return new WaitForSeconds(1.5f);
        print(Time.time);
        print("Start!");
        Ball.Initilaize();
    }

    public void EndGame()
    {
        PongSceneManager.MainMenu();
    }
}

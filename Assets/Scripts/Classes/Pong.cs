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
    public float GlobalSpeedMultiplier = 1.0f;

    public AudioSource Music;
    public bool Spin = false;
    // Use this for initialization
    void Start () {
        IsPlaying = false;
        Spin = false;
        Map = new StaticCameraMap(Camera.main); //New map
        StartCoroutine(StartGame());
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsPlaying)
            return;

        if (Spin)
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 20.0f * Time.deltaTime);
        }

        //Input (Could be put in their own update functions as well)
        foreach (var player in Players)
        {
            player.HandleInput();
        }

        //Movement (Could be put in their own update functions as well)
        foreach (var player in Players)
        {
            player.MovementUpdate(GlobalSpeedMultiplier);
        }
        Ball.MovementUpdate(GlobalSpeedMultiplier);

        //Collisions
        //Don't go off screen
        foreach (var player in Players)
        {
            player.CheckMapEdgeCollision(Map);
        }


        //Check direction
        if (Ball.GetDirection().x < 0)
        {
            //Check player 1 collision
            //Debug.Log("p1 check");
            Ball.CheckPlayerCollison(Players[0]);
        }
        else
        {
            //Check player 2 collision
            //Debug.Log("p2 check");
            Ball.CheckPlayerCollison(Players[1]);
        }

        //Don't go off screen
        Ball.CheckMapEdgeCollision(Map);

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

    IEnumerator Sezuire()
    {
        Music.volume = 0.3f;
        
        yield return new WaitForSeconds(24f);
        Spin = true;
        GlobalSpeedMultiplier = 3.0f;
        Music.volume = 1f;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            Camera.main.backgroundColor = Random.ColorHSV();
        }

       
    }

    IEnumerator StartGame()  {
        print("Starting game.");
        yield return new WaitForSeconds(1f);
        print("Start!");

        Players = new List<IPlayer>();
        Players.Clear();


        AddBall();
        HumanVersusAi();

        
        Ball.Initialize();

        IsPlaying = true;


        StartCoroutine(Sezuire());

    }

    public void AddBall()
    {
        GameObject ballModel = GameObject.Instantiate(BallModel, new Vector3(0, 0), Quaternion.identity);
        Ball = ballModel.AddComponent<Ball>();
        Ball.SetSpeed(5.0f);
    }

    public void AddHumanPlayer(Vector2 position, KeyCode upKey, KeyCode downKey)
    {
        GameObject model = GameObject.Instantiate(PaddleModel, new Vector3(position.x, position.y), Quaternion.identity);
        HumanPlayer player = model.AddComponent<HumanPlayer>();
        player.SetKeys(upKey, downKey);
        player.SetSpeed(5.0f);
        Players.Add(player);
    }
    public void AddAiPlayer(Vector2 position, IMoveableObject target)
    {
        GameObject model = GameObject.Instantiate(PaddleModel, new Vector3(position.x, position.y), Quaternion.identity);
        AiPlayer player = model.AddComponent<AiPlayer>();
        player.SetTarget(target);
        player.SetSpeed(8.0f);
        Players.Add(player);
    }

    public void AiVersusAi()
    {
        AddAiPlayer(new Vector2(-12, 0), Ball);
        AddAiPlayer(new Vector2(12, 0), Ball);
    }

    public void HumanVersusAi()
    {
        AddHumanPlayer(new Vector2(-12, 0), KeyCode.W, KeyCode.S);
        AddAiPlayer(new Vector2(12, 0), Ball);
    }

    public void HumanVersusHuman()
    {
        AddHumanPlayer(new Vector2(-12, 0), KeyCode.W, KeyCode.S);
        AddHumanPlayer(new Vector2(12, 0), KeyCode.UpArrow, KeyCode.DownArrow);
    }

    public void EndGame()
    {
        PongSceneManager.MainMenu();
    }
}

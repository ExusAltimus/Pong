using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: seperate into different game mode classes
public class Pong : MonoBehaviour {

    public GameObject PaddleModel;
    public GameObject BallModel;
    public GameObject Hitmarker;

    public List<IPlayer> Players;
    public IBall Ball;
    public IMap Map;
    
    public bool IsPlaying = false;
    public float GlobalSpeedMultiplier = 1.0f;
    public float PaddleDistance = 8.0f;

    public AudioSource Music;
    public bool Spin = false;
    // Use this for initialization
    void Start () {
        IsPlaying = false;
        Spin = false;
        
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


        Vector2 hit = new Vector2();
        //Check direction
        if (Ball.GetDirection().x < 0)
        {
            //Check player 1 collision
            //Debug.Log("p1 check");
            Ball.CheckPlayerCollison(Players[0], ref hit);
        }
        else
        {
            //Check player 2 collision
            //Debug.Log("p2 check");
            Ball.CheckPlayerCollison(Players[1], ref hit);
        }

        //Don't go off screen
        Ball.CheckMapEdgeCollision(Map);



        OutOfBounds ballOutOfBounds = Map.CheckOutOfBounds(Ball.Bounds);
        if (ballOutOfBounds == OutOfBounds.Left)
        {
            //Player 2 wins
            Debug.Log("Player 2 wins");
            EndGame("Player 2 Wins");
        }
        else if (ballOutOfBounds == OutOfBounds.Right)
        {
            //Player 1 wins
            Debug.Log("Player 1 wins");
            EndGame("Player 1 Wins");
        }
    }

    IEnumerator Sezuire()
    {
        Music.Play();
        Music.volume = 0.3f;
        
        yield return new WaitForSeconds(25f);
        Spin = true;
        GlobalSpeedMultiplier *= 3.0f;
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

        GlobalSpeedMultiplier = GameManager.Instance.GameSpeed;

        Map = new StaticCameraMap(Camera.main); //New map

        Players = new List<IPlayer>();
        Players.Clear();

        if (GameManager.Instance.Hitmarker)
        {
            AddHitmarkerBall(GameManager.Instance.BallSpeedIncrement);
        }
        else
        {
            AddBall(GameManager.Instance.BallSpeedIncrement);
        }

        switch (GameManager.Instance.PlayerMode)
        {
            case PlayerMode.AiVersusAi:
                AiVersusAi();
                break;
            case PlayerMode.HumanVersusAi:
                HumanVersusAi();
                break;
            case PlayerMode.HumanVersusHuman:
                HumanVersusHuman();
                break;
            default:
                HumanVersusAi();
                break;
        }

        Ball.Initialize();

        IsPlaying = true;

        if (GameManager.Instance.SezuireMode)
        {
            StartCoroutine(Sezuire());
        }

    }

    public void AddBall(float ballSpeedIncrement = 1.0f)
    {
        GameObject ballModel = GameObject.Instantiate(BallModel, new Vector3(0, 0), Quaternion.identity);
        Ball ball = ballModel.AddComponent<Ball>();
        ball.SetSpeed(5.0f);
        ball.SetSpeedIncrement(ballSpeedIncrement);
        Ball = ball;
    }

    public void AddHitmarkerBall(float ballSpeedIncrement = 1.0f)
    {
        GameObject ballModel = GameObject.Instantiate(BallModel, new Vector3(0, 0), Quaternion.identity);
        HitmarkerBall ball = ballModel.AddComponent<HitmarkerBall>();
        ball.SetSpeed(5.0f);
        ball.SetSpeedIncrement(ballSpeedIncrement);
        ball.SetHitmarker(Hitmarker);
        Ball = ball;
    }

    public void AddHumanPlayer(Vector2 position, KeyCode upKey, KeyCode downKey)
    {
        GameObject model = GameObject.Instantiate(PaddleModel, new Vector3(position.x, position.y), Quaternion.identity);
        HumanPlayer player = model.AddComponent<HumanPlayer>();
        player.SetKeys(upKey, downKey);
        player.SetSpeed(5.0f);
        Players.Add(player);
    }
    public void AddAiPlayer(Vector2 position, IMoveableObject target, float difficulty = 1.0f)
    {
        GameObject model = GameObject.Instantiate(PaddleModel, new Vector3(position.x, position.y), Quaternion.identity);
        AiPlayer player = model.AddComponent<AiPlayer>();
        player.SetTarget(target);
        player.SetSpeed(8.0f);
        player.SetDifficulty(difficulty);
        Players.Add(player);
    }

    public void AiVersusAi(float difficulty = 1.0f)
    {
        AddAiPlayer(new Vector2(-PaddleDistance, 0), Ball, difficulty);
        AddAiPlayer(new Vector2(PaddleDistance, 0), Ball, difficulty);
    }

    public void HumanVersusAi(float difficulty = 1.0f)
    {
        AddHumanPlayer(new Vector2(-PaddleDistance, 0), KeyCode.W, KeyCode.S);
        AddAiPlayer(new Vector2(PaddleDistance, 0), Ball, difficulty);
    }

    public void HumanVersusHuman()
    {
        AddHumanPlayer(new Vector2(-PaddleDistance, 0), KeyCode.W, KeyCode.S);
        AddHumanPlayer(new Vector2(PaddleDistance, 0), KeyCode.UpArrow, KeyCode.DownArrow);
    }

    public void EndGame(string message)
    {
        GameManager.Instance.Message = message;
        PongSceneManager.MainMenu();
    }
}

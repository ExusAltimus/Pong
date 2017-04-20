using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pong : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Ball;

    public Vector2 InitialBallVelocity;
    public Vector2 BallVelocity;

    public Collider BallCollider;
    public Collider Player1Collider;
    public Collider Player2Collider;

    public Bounds CameraBounds;

    public float Player1Direction = 0;
    public float Player2Direction = 0;

    public float PlayerSpeed = 5.0f;
    public float BallSpeed = 5.0f;
    public float BallSpeedIncrement = 1f;

    public float Player2Difficulty = 0.4f;

    private bool _ballDebounce = true;
    // Use this for initialization
    void Start () {
        StartCoroutine(StartGame());
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        BallCollider = Ball.GetComponent<Collider>();
        Player1Collider = Player1.GetComponent<Collider>();
        Player2Collider = Player2.GetComponent<Collider>();
        CameraBounds = Camera.main.OrthographicBounds();
    }
	
	// Update is called once per frame
	void Update () {

        //Inbput
        HandlePlayer1();
        HandlePlayer2();

        //Movement
        Ball.transform.Translate(BallVelocity * BallSpeed * Time.smoothDeltaTime);
        Player1.transform.Translate(Player1Direction * PlayerSpeed * Vector2.up * Time.smoothDeltaTime);
        Player2.transform.Translate(Player2Direction * PlayerSpeed * Vector2.up * Time.smoothDeltaTime);

        //Don't go off screen
        if (Player1Collider.bounds.max.y > CameraBounds.max.y)
        {
            Player1.transform.position = new Vector2(Player1.transform.position.x, CameraBounds.max.y - Player1Collider.bounds.extents.y);
        }
        else if (Player1Collider.bounds.min.y < CameraBounds.min.y)
        {
            Player1.transform.position = new Vector2(Player1.transform.position.x, CameraBounds.min.y + Player1Collider.bounds.extents.y);
        }

        if (Player2Collider.bounds.max.y > CameraBounds.max.y)
        {
            Player2.transform.position = new Vector2(Player2.transform.position.x, CameraBounds.max.y - Player2Collider.bounds.extents.y);
        }
        else if (Player2Collider.bounds.min.y < CameraBounds.min.y)
        {
            Player2.transform.position = new Vector2(Player2.transform.position.x, CameraBounds.min.y + Player2Collider.bounds.extents.y);
        }

        //Collisions
        HandleCollisions();
    }

    private void HandlePlayer1()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Up
            Player1Direction = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Down
            Player1Direction = -1;
        }
        else
        {
            //Stop
            Player1Direction = 0;
        }
    }

    private bool _canPlayer2Move = true;
    private float _lastPlayer2Move = 0;
    private float _player2Speed = 1.0f;
    private void HandlePlayer2()
    {
        
        if ((Time.time - _lastPlayer2Move > 0.5f))
        {
            _lastPlayer2Move = Time.time;
            _player2Speed = Random.Range(Player2Difficulty, 1f); //Change speed every half second

        }

        float distance = Mathf.Abs(BallCollider.bounds.center.y - Player2Collider.bounds.center.y);

        if (distance < 0.3f)
        {
            _canPlayer2Move = false;
        }
        else
        {
            _canPlayer2Move = true;
        }

        if (_canPlayer2Move)
        {
            if (BallCollider.bounds.center.y > Player2Collider.bounds.center.y)
            {
                Player2Direction = 1 * _player2Speed;
            }
            else
            {
                Player2Direction = -1 * _player2Speed;
            }
        }
        else
        {
            Player2Direction = 0;
        }

   

    }

    private void CheckPlayerCollison(Collider collider)
    {
        if (BallCollider.bounds.Intersects(collider.bounds) && _ballDebounce)
        {
            _ballDebounce = false;

            float angle = Vector2.Angle(BallVelocity, Vector2.right) + Random.Range(-10.0f, 10.0f);
            angle = Mathf.Clamp(angle, -60.0f, 60.0f);

            int direction = BallVelocity.x > 0 ? -1 : 1;
            BallVelocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * Random.Range(1.0f, 2.0f);
            BallVelocity = BallVelocity * direction;
            BallSpeed += BallSpeedIncrement;


        }
    }

    private void HandleCollisions()
    {
        if (!_ballDebounce) //Checks if ball has already collided with a player, then resets when it has passed the center (To prevent multiple player collisions)
        {
            if ((BallVelocity.x > 0 && Ball.transform.position.x > 0.5) || (BallVelocity.x < 0 && Ball.transform.position.x < 0.5))
            {
                _ballDebounce = true;
            }
        }

        //Check direction
        if (BallVelocity.x < 0)
        {
            //Check player 1 collision
            CheckPlayerCollison(Player1Collider);
        }
        else
        {
            //Check player 2 collision
            CheckPlayerCollison(Player2Collider);
        }


        //Check bound collision
        if (BallCollider.bounds.max.y > CameraBounds.max.y)
        {
            BallVelocity.y = BallVelocity.y * -1;
            print("Top collision");
        }
        else if (BallCollider.bounds.min.y < CameraBounds.min.y)
        {
            BallVelocity.y = BallVelocity.y * -1;
            print("Bottom collision");
        }
        else if (BallCollider.bounds.max.x < CameraBounds.min.x)
        {
            print("Player 2 wins");
            EndGame();
        }
        else if (BallCollider.bounds.min.x > CameraBounds.max.x)
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
        int direction = Random.Range(-1.0f, 1.0f) <= 0 ? -1 : 1;
        float angle = Random.Range(-60, 60);

        InitialBallVelocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        BallVelocity = InitialBallVelocity;
        _ballDebounce = true;
    }

    public void EndGame()
    {
        PongSceneManager.MainMenu();
    }
}

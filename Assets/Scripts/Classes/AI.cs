using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AI : Player
{
    public float Difficulty = 0.4f;
    public Ball Ball;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool _canAIMove = true;
    private float _lastMove = 0;
    private float _aiSpeed = 1.0f;
    public override void HandleInput()
    {

        if ((Time.time - _lastMove > 0.5f))
        {
            _lastMove = Time.time;
            _aiSpeed = Random.Range(Difficulty, 1f); //Change speed every half second

        }

        float distance = Mathf.Abs(Ball.Collider.bounds.center.y - Collider.bounds.center.y);

        if (distance < 0.3f)
        {
            _canAIMove = false;
        }
        else
        {
            _canAIMove = true;
        }

        if (_canAIMove)
        {
            if (Ball.Collider.bounds.center.y > Collider.bounds.center.y)
            {
                Direction = 1 * _aiSpeed;
            }
            else
            {
                Direction = -1 * _aiSpeed;
            }
        }
        else
        {
            Direction = 0;
        }



    }
}

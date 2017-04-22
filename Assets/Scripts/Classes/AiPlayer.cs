using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AiPlayer : MoveableObject, IPlayer
{
    public float Difficulty = 0.8f;
    public IMoveableObject Target;

    private bool _canAIMove = true;
    private float _lastMove = 0;
    private float _aiSpeed = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(IMoveableObject target)
    {
        Target = target;
    }

    public override void SetSpeed(float speed)
    {
        _aiSpeed = speed;
    }

    public void HandleInput()
    {
        float diff = Target.Bounds.center.y - Bounds.center.y;
        float distance = Mathf.Abs(diff);
        if ((Time.time - _lastMove > 0.5f))
        {
            _lastMove = Time.time;
            Speed = Random.Range(Difficulty * _aiSpeed, _aiSpeed); //Change speed every half second
        }

        if (distance < 0.2f)
        {
            _canAIMove = false;
        }
        else
        {
            _canAIMove = true;
        }

        if (_canAIMove)
        {
            if (Target.Bounds.center.y > Bounds.center.y)
            {
                Direction.y = 1;
            }
            else
            {
                Direction.y = -1;
            }
        }
        else
        {
            Direction.y = 0;
        }
    }
    public float GetAttackSpeed()
    {
        return Direction.y;
    }
}

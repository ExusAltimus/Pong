using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AiPlayer : MoveableObject, IPlayer
{
    public float Difficulty = 0.4f;
    public MoveableObject Target;

    private bool _canAIMove = true;
    private float _lastMove = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(MoveableObject target)
    {
        Target = target;
    }

    public void HandleInput()
    {

        if ((Time.time - _lastMove > 0.5f))
        {
            _lastMove = Time.time;
            Speed = Random.Range(Difficulty, 1f); //Change speed every half second

        }

        float distance = Mathf.Abs(Target.Bounds.center.y - Bounds.center.y);

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

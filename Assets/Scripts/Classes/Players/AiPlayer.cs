using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AiPlayer : MoveableObject, IPlayer
{
    public const float MAX_OFFSET = 0.8f;
    public float Difficulty = 1f;
    public IMoveableObject Target;

    private bool _canAiMove = true;
    private float _lastMove = 0;
    private float _aiSpeed = 0.0f;
    private float _offset = 0.0f; //Offset from center, so the AI can put more angle on the ball
    private float _newOffset = 0.0f;
    //private float _speedMultiplier = 1.0f;
    // Use this for initialization
    void Start()
    {
        _canAiMove = true;  
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

    public void SetDifficulty(float difficulty)
    {
        Difficulty = difficulty;
    }


    public void HandleInput()
    {
        float diff = Target.Bounds.center.y - (Bounds.center.y + (_offset * Bounds.extents.y));

        float distance = Mathf.Abs(diff);
        float deltaTime = (Time.time - _lastMove);
        float dLimit = Mathf.Clamp01(Difficulty);

        if (deltaTime > 0.5f)
        {
            _lastMove = Time.time;
            
            //_newOffset += Random.Range(-0.25f, 0.25f) * dLimit; //Move closer to edge faster on harder difficulties
            _newOffset = Random.Range(-MAX_OFFSET, MAX_OFFSET) * dLimit; //Move closer to edge faster on harder difficulties
            _newOffset = Mathf.Clamp(_newOffset, -MAX_OFFSET, MAX_OFFSET) * dLimit; //Move closer to edge on harder difficulties
            //_speedMultiplier = Random.Range(dLimit, 1.0f);
        }
        else
        {
            //Debug.Log(deltaTime / 0.5f);
            _offset = Mathf.Lerp(_offset, _newOffset, deltaTime / 0.5f); //Lerp for smoother offset animation
        }

        Speed = distance * ((dLimit * 0.8f) + 0.2f) * Target.Speed; //Change speed every half second

        if (_canAiMove)
        {
            if (diff > 0)
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
}

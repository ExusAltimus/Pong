using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {


    public Collider Collider;
    public Vector2 InitialVelocity;
    public Vector2 Velocity;
    public float Speed = 5.0f;
    public float SpeedIncrement = 1f;

    private bool _ballDebounce = true;
    // Use this for initialization
    void Start () {
        Collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void MovementUpdate()
    {
        transform.Translate(Velocity * Speed * Time.smoothDeltaTime);
        if (!_ballDebounce) //Checks if ball has already collided with a player, then resets when it has passed the center (To prevent multiple player collisions)
        {
            if ((Velocity.x > 0 && transform.position.x > 0.5) || (Velocity.x < 0 && transform.position.x < 0.5))
            {
                _ballDebounce = true;
            }
        }
    }

    public void Initilaize()
    {
        int direction = Random.Range(-1.0f, 1.0f) <= 0 ? -1 : 1;
        float angle = Random.Range(-60, 60);

        InitialVelocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        Velocity = InitialVelocity;
        _ballDebounce = true;
    }

    public void CheckPlayerCollison(Bounds bounds)
    {
        if (Collider.bounds.Intersects(bounds) && _ballDebounce)
        {
            _ballDebounce = false;

            float angle = Vector2.Angle(Velocity, Vector2.right) + Random.Range(-10.0f, 10.0f);
            angle = Mathf.Clamp(angle, -60.0f, 60.0f);

            int direction = Velocity.x > 0 ? -1 : 1;
            Velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * Random.Range(1.0f, 2.0f);
            Velocity = Velocity * direction;
            Speed += SpeedIncrement;
        }
    } 

    public bool CheckMapCollision(Bounds bounds)
    {
        //Check bound collision
        if (Collider.bounds.max.y > bounds.max.y)
        {
            Velocity.y = Velocity.y * -1;
            print("Top collision");
            
        }
        else if (Collider.bounds.min.y < bounds.min.y)
        {
            Velocity.y = Velocity.y * -1;
            print("Bottom collision");
        }
        else if (Collider.bounds.max.x < bounds.min.x)
        {
            print("Player 2 wins");
            return true;
        }
        else if (Collider.bounds.min.x > bounds.max.x)
        {
            print("Player 1 wins");
            return true;
        }
        return false;
    }

}

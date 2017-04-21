using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MoveableObject, IBall {

    public float MinAngle = -60.0f;
    public float MaxAngle = 60.0f;
    public float SpeedIncrement = 1f;
    private bool _checkForPlayerCollisions = true; //Check for player collisions

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    //Not needed anymore because the game checks the direction of the ball
    /*
    public override void MovementUpdate()
    {
        base.MovementUpdate();
        if (!_checkForPlayerCollisions) //Checks if ball has already collided with a player, then resets when it has passed the center (To prevent multiple player collisions)
        {
            if ((Direction.x > 0 && Position.x > 0.5) || (Direction.x < 0 && Position.x < 0.5))
            {
                _checkForPlayerCollisions = true;
            }
        }
    }
    */

    public void Initialize()
    {
        int xdir = Random.Range(-1.0f, 1.0f) <= 0 ? -1 : 1;
        float angle = Random.Range(-60, 60);
        Direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * xdir, Mathf.Sin(Mathf.Deg2Rad * angle));

        _checkForPlayerCollisions = true;
    }

    public bool CheckPlayerCollison(IPlayer player)
    {
        if (Bounds.Intersects(player.Bounds) && _checkForPlayerCollisions)
        {
            _checkForPlayerCollisions = false; //Stop checking for player collisions until it has passed the center of the map
            float attackSpeed = player.GetAttackSpeed();
            float angle = Vector2.Angle(Direction, Vector2.right) + (attackSpeed * 10.0f); //Add angle depending which direction player is attcking the ball from
            angle = Mathf.Clamp(angle, -60.0f, 60.0f); //Make sure ball doesn't go nuts
            int xdir = Direction.x > 0 ? -1 : 1;
            int ydir = Direction.y > 0 ? -1 : 1;
            Direction = new Vector2(xdir * Mathf.Cos(Mathf.Deg2Rad * angle), ydir * Mathf.Sin(Mathf.Deg2Rad * angle));
            Speed += SpeedIncrement;
            return true;
        }
        return false;
    }

    public override EdgeCollision CheckMapEdgeCollision(IMap map)
    {
        EdgeCollision edgeCollision = base.CheckMapEdgeCollision(map); //Call base method, which keeps ball in bounds
        if (edgeCollision == EdgeCollision.Top || edgeCollision == EdgeCollision.Bottom) // Bounce off top or bottom
        {
            Direction.y *= -1; //Flip y direction
        }
        return edgeCollision;
    }
}

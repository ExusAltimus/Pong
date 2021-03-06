﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MoveableObject, IBall {

    public float MAX_BOUNCE_ANGLE = 65.0f;
    public float MAX_STARTING_BOUNCE_ANGLE = 45.0f;
    public float SpeedIncrement { get; set; }
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
        float angle = Random.Range(-MAX_STARTING_BOUNCE_ANGLE, MAX_STARTING_BOUNCE_ANGLE);
        Direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * xdir, Mathf.Sin(Mathf.Deg2Rad * angle));

        _checkForPlayerCollisions = true;
    }

    public void SetSpeedIncrement(float increment)
    {
        SpeedIncrement = increment;
    }

    public virtual bool CheckPlayerCollison(IPlayer player, ref Vector2 hit)
    {
        //Bounds.Intersects(player.Bounds)
        //4-Point intersect ray provides better player collision detection
        //https://gyazo.com/505f4213ee305c6e226b355838ff29c0 <-- I drew this picture to show what it's doing
        //Is there a better way? Probably. but it is 3am and my neck hurts
        bool intersects = player.Bounds.IntersectsLine(LastPosition + (Vector2)Bounds.extents, Position + (Vector2)Bounds.extents);
        intersects = intersects || player.Bounds.IntersectsLine(LastPosition - (Vector2)Bounds.extents, Position - (Vector2)Bounds.extents);
        intersects = intersects || player.Bounds.IntersectsLine(LastPosition + (Vector2)Bounds.extents, Position - (Vector2)Bounds.extents);
        intersects = intersects || player.Bounds.IntersectsLine(LastPosition - (Vector2)Bounds.extents, Position + (Vector2)Bounds.extents);
        if (intersects && _checkForPlayerCollisions)
        {
            //Position = DeltaMovement.origin + ((distance - Bounds.extents.x) * DeltaMovement.direction.normalized); //Jump to face of paddle
            int xdir = Direction.x > 0 ? -1 : 1;
            //int ydir = Direction.y > 0 ? -1 : 1;
            //Direction = new Vector2(xdir * Mathf.Cos(Mathf.Deg2Rad * angle), ydir * Mathf.Sin(Mathf.Deg2Rad * angle));

            //New algorithm for rebound https://gamedev.stackexchange.com/questions/4253/in-pong-how-do-you-calculate-the-balls-direction-when-it-bounces-off-the-paddl
            var relativeIntersectY = (Bounds.center.y - player.Bounds.center.y);
            var normalizedRelativeIntersectionY = (relativeIntersectY / player.Bounds.extents.y);
            normalizedRelativeIntersectionY = Mathf.Clamp(normalizedRelativeIntersectionY, -1.0f, 1.0f);
            var bounceAngle = normalizedRelativeIntersectionY * MAX_BOUNCE_ANGLE;
            
            Direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * bounceAngle) * xdir, Mathf.Sin(Mathf.Deg2Rad * bounceAngle));
            Speed += SpeedIncrement * Mathf.Abs(normalizedRelativeIntersectionY);

            hit = new Vector2(xdir == 1 ? Bounds.min.x : Bounds.max.x, Bounds.center.y);
            Debug.Log("Player hit");
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
            Debug.Log("Ball Edge Collide");
        }

        return edgeCollision;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




public class MoveableObject : MonoBehaviour, IMoveableObject
{

    public Collider Collider;

    public float Speed { get; set; }

    public Vector2 Direction; //Unit vector
    //In case we want to use our own positioning system later
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    public Vector2 LastPosition { get; set; }

    public float Delta { get; set; }
    //In case we want to use our own boundry system later
    public Bounds Bounds
    {
        get
        {
            //this.collider is deprecated           
            return Collider.bounds;
        }
    }

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    void Start()
    {
        
    }

    public virtual void MovementUpdate(float speedMultiplier = 1.0f)
    {
        LastPosition = transform.position;
        transform.Translate(Direction * Speed * speedMultiplier * Time.deltaTime);
    }

    public void MoveTo(Vector2 position)
    {
        Position = position;
    }

    public virtual void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public Vector2 GetDirection()
    {
        return Direction;
    }
    
    //Every object must stay on screen vertically, so let that be the default implementation
    public virtual EdgeCollision CheckMapEdgeCollision(IMap map)
    {
        EdgeCollision edgeCollision = map.CheckEdgeCollision(this.Bounds);
        if (edgeCollision == EdgeCollision.Top)
        {
            MoveTo(new Vector2(this.Bounds.center.x, map.Bounds.max.y - this.Bounds.extents.y));
        }
        else if (edgeCollision == EdgeCollision.Bottom)
        {
            MoveTo(new Vector2(this.Bounds.center.x, map.Bounds.min.y + this.Bounds.extents.y));
        }
        return edgeCollision;
    }
}


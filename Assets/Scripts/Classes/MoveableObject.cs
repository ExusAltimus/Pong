using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




public class MoveableObject : MonoBehaviour, IMoveableObject
{

    public Collider Collider;

    public float Speed;

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

    //In case we want to use our own boundry system later
    public Bounds Bounds
    {
        get
        {
            //this.collider is deprecated           
            return Collider.bounds;
        }
    }


    void Start()
    {
        Collider = GetComponent<Collider>();
    }

    public virtual void MovementUpdate()
    {
        transform.Translate(Direction * Speed * Time.smoothDeltaTime);
    }

    public void MoveTo(Vector2 position)
    {
        Position = position;
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class StaticCameraMap : IMap
{
    public Bounds Bounds { get; private set; }
    public Camera Camera { get; private set; }
    public StaticCameraMap(Camera camera)
    {
        Camera = camera;
        var vertExtent = camera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        Bounds = camera.OrthographicBounds(); //Set bounds
    }

    public virtual OutOfBounds CheckOutOfBounds(Bounds other)
    {       
        //Check bounds
        if (Bounds.max.x < other.min.x)
        {
            return OutOfBounds.Right;
        }
        else if (Bounds.min.x > other.max.x)
        {
            return OutOfBounds.Left;
        }
        else if (Bounds.max.y < other.min.y) //Should never be satisfied, since ball bounces off top and bottom edges
        {
            return OutOfBounds.Top;
        }
        else if (Bounds.min.y > other.max.y)
        {
            return OutOfBounds.Bottom;
        }
        return OutOfBounds.None;
    }


    public virtual EdgeCollision CheckEdgeCollision(Bounds other)
    {

        //Check edges
        if (Bounds.max.y < other.max.y)
        {
            //Debug.Log("Top collision");
            return EdgeCollision.Top;

        }
        else if (Bounds.min.y > other.min.y)
        {
            //Debug.Log("Bottom collision");
            return EdgeCollision.Bottom;

        }

        return EdgeCollision.None;
    }
}


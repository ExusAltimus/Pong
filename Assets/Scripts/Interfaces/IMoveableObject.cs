using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public interface IMoveableObject
{
    Bounds Bounds { get; }
    float Speed { get; set; }
    void SetSpeed(float speed);
    Vector2 GetDirection();
    EdgeCollision CheckMapEdgeCollision(IMap map);
    void MovementUpdate(float speedMultiplier);
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum OutOfBounds { None, Left, Right, Top, Bottom };
public enum EdgeCollision { None, Left, Right, Top, Bottom };
public interface IMap
{
    Bounds Bounds { get; }
    OutOfBounds CheckOutOfBounds(Bounds bounds);
    EdgeCollision CheckEdgeCollision(Bounds bounds);
}


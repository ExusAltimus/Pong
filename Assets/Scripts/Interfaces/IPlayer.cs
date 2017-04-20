using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPlayer
{
    Collider Collider { get; set; }
    void HandleInput();
    void MovementUpdate();
    void MoveTo(Vector2 position);
}

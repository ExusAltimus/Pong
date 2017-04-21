﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPlayer : IMoveableObject
{

    void HandleInput();
    float GetAttackSpeed();
}

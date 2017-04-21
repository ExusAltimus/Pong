using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IBall : IMoveableObject
{
    bool CheckPlayerCollison(IPlayer player);
    void Initialize();
}

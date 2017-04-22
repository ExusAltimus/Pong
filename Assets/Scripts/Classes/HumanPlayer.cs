using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class HumanPlayer : MoveableObject, IPlayer
{
    public const KeyCode UP_KEY = KeyCode.W;
    public const KeyCode DOWN_KEY = KeyCode.S;

    public KeyCode UpKey = UP_KEY;
    public KeyCode DownKey = DOWN_KEY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetKeys(KeyCode upKey, KeyCode downKey)
    {
        UpKey = upKey;
        DownKey = downKey;
    }

    public void HandleInput()
    {
        if (Input.GetKey(UpKey))
        {
            //Up
            Direction.y = 1;
        }
        else if (Input.GetKey(DownKey))
        {
            //Down
            Direction.y = -1;
        }
        else
        {
            //Stop
            Direction.y = 0;
        }
    }
    public float GetAttackSpeed()
    {
        return Direction.y;
    }
}


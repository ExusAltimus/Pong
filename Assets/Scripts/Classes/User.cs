using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class User : Player
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Up
            Direction = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Down
            Direction = -1;
        }
        else
        {
            //Stop
            Direction = 0;
        }
    }
}


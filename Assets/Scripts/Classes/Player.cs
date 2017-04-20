using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Direction = 0.0f;
    public Collider Collider;
    // Use this for initialization
    void Start()
    {
        Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovementUpdate()
    {
        transform.Translate(Direction * Speed * Vector2.up * Time.smoothDeltaTime);
    }

    public void MoveTo(Vector2 position)
    {
        transform.position = position;
    }

    public virtual void HandleInput()
    {

    }
}

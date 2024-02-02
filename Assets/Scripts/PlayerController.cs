using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 movementVector;
    public Vector2 lookVector;
    public float speed;

    private CharacterController _controller;

    public Ball heldBall;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        lookVector = Vector2.up;
    }

    void FixedUpdate()
    {
        _controller.Move(new Vector3(movementVector.x,-9.82f,movementVector.y) * (speed * Time.deltaTime));
        transform.LookAt(transform.position+new Vector3(lookVector.x,0,lookVector.y));

        if (heldBall)
        {
            heldBall.transform.position = transform.position + new Vector3(lookVector.x,0,lookVector.y)*1.1f;
            
        }
    }

    public void OnMove(InputValue vec)
    {
        movementVector = vec.Get<Vector2>();
    }

    public void OnLook(InputValue vec)
    {
        if (vec.Get<Vector2>().magnitude != 0)
        {
            lookVector = vec.Get<Vector2>();
            lookVector.Normalize();
        }
    }

    public void OnFire()
    {
        if (heldBall)
        {
            heldBall.Shoot(lookVector,30f);
            heldBall = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Ball Hit");
        if (other.gameObject.TryGetComponent(out Ball ball))
        {
            if (!ball.HeldOrFired)
            {
                heldBall = ball;
                ball.Pickup();
            }
        }
    }
}

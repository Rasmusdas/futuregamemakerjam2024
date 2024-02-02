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
    public float blinkTimer = 0.3f;

    public float invulnTimer = 1;

    public int health = 3;

    public bool invulnerable;
    private MeshRenderer _mr;

    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
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
            heldBall.Shoot(gameObject,lookVector,30f);
            heldBall = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Ball ball)) return;
        if (heldBall != null) return;
        
        if (!ball.HeldOrFired)
        {
            heldBall = ball;
            ball.Pickup();
        }

        if (ball.fired && !invulnerable && ball.owner != gameObject)
        {
            health -= 1;
            invulnerable = true;
            StartCoroutine(Invulnerability());
        }
    }

    private IEnumerator Invulnerability()
    {
        float t = 0;
        Color defColor = _mr.material.color;
        for (int i = 0; i < invulnTimer / blinkTimer / 2; i++)
        {
            while (t < 1)
            {
                t += Time.deltaTime*1/blinkTimer*2;
                defColor.a = t;
                _mr.material.color = defColor;
                yield return new WaitForEndOfFrame();
            }
            
            while (t > 0)
            {
                t -= Time.deltaTime*1/blinkTimer*2;
                defColor.a = t;
                _mr.material.color = defColor;
                yield return new WaitForEndOfFrame();
            }
        }

        while (t < 1)
        {
            t += Time.deltaTime*1/blinkTimer*2;
            defColor.a = t;
            _mr.material.color = defColor;
            yield return new WaitForEndOfFrame();
        }

        invulnerable = false;

    }
}

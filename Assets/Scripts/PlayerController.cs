using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Vector2 movementVector;
    public Vector2 lookVector;
    public float speed;
    private CharacterController _controller;
    public Ball heldBall;
    public float blinkTimer = 0.3f;

    public float ballOffset = 1.45f;

    public float degreesOfAimAssist = 15;
    public float aimAssistDistanceLimit = 15;

    public float invulnTimer = 1;

    public int health = 3;

    public bool invulnerable;
    private MeshRenderer _mr;
    private Vector3 _lookAtVector;
    private Vector3 _startPosition;


    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _controller = GetComponent<CharacterController>();
        _lookAtVector = Vector2.up;
        _startPosition = PlayerSpawnManager.Instance.GetStartPosition();
    }

    void OnStart()
    {
        PlayerSpawnManager.Instance.started = true;
    }

    
    void FixedUpdate()
    {
        if (!PlayerSpawnManager.Instance.started)
        {
            transform.position = _startPosition;
            return;
        }
        if (heldBall)
        {
            heldBall.transform.position = transform.position + _lookAtVector*ballOffset;
            heldBall.transform.parent = transform;
        }
        
        _controller.Move(new Vector3(movementVector.x,-9.82f,movementVector.y) * (speed * Time.deltaTime));

        if (movementVector.magnitude > 0)
        {
            _lookAtVector = new Vector3(movementVector.x, 0, movementVector.y);
        }
        
        if (lookVector.magnitude > 0)
        {
            _lookAtVector = new Vector3(lookVector.x, 0, lookVector.y);
        }

        if (_lookAtVector.magnitude > 0)
        {
            _lookAtVector.Normalize();
            
            transform.LookAt(transform.position+_lookAtVector);
        }

        GetAssistedAim();
    }


    public Vector3 GetAssistedAim()
    {
        float shortestDist = aimAssistDistanceLimit;
        Vector3 best_dir = -Vector3.one;
        
        foreach (var player in PlayerSpawnManager.Instance.players)
        {
            if (player == this) continue;
            Vector3 dir = player.transform.position - (transform.position + _lookAtVector*ballOffset);

            if (dir.magnitude < shortestDist)
            {
                shortestDist = dir.magnitude;
            }
            else
            {
                continue;
            }
            
            dir.Normalize();

            float cosAng = Vector3.Dot(_lookAtVector, dir);

            if (cosAng > Mathf.Cos(Mathf.PI / 180 * degreesOfAimAssist))
            {
                best_dir = dir;
            }
        }

        if (best_dir != -Vector3.one)
        {
            Debug.DrawRay(transform.position,best_dir,Color.blue);
        }
        return best_dir;
    }

    public void OnMove(InputValue vec)
    {
        movementVector = vec.Get<Vector2>();
    }

    public void OnLook(InputValue vec)
    {
        lookVector = vec.Get<Vector2>();
    }

    public void OnFire()
    {
        if (heldBall)
        {
            heldBall.transform.parent = null;

            Vector3 dir = GetAssistedAim();

            if (dir == -Vector3.one)
            {   
                heldBall.Shoot(gameObject,_lookAtVector,1);
            }
            else
            {
                heldBall.Shoot(gameObject,dir,1);
            }
            
            
            heldBall = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Vector2 movementVector;
    public LineRenderer lr;
    public Vector2 lookVector;
    public float speed;
    private CharacterController _controller;
    public Ball heldBall;
    public float blinkTimer = 0.3f;
    public float chargeTime = 1;
    private float _currentCharge = 0;
    public Transform ballSpot;

    public Image chargeIndicator;

    public bool charging = false;

    public float ballOffset = 1.45f;

    public float degreesOfAimAssist = 15;
    public float aimAssistDistanceLimit = 15;

    public float invulnTimer = 1;

    public int health = 3;

    public bool invulnerable;
    private SkinnedMeshRenderer _mr;
    private Vector3 _lookAtVector;
    private Vector3 _startPosition;
    private PlayerAnimations _animator;


    void Start()
    {
        _animator = GetComponent<PlayerAnimations>();
        _mr = GetComponentInChildren<SkinnedMeshRenderer>();
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
            heldBall.transform.position = ballSpot.position;
            heldBall.transform.parent = transform;

            if (charging)
            {
                _currentCharge = Mathf.Clamp(Time.deltaTime/chargeTime+_currentCharge,0,1) ;
                chargeIndicator.fillAmount = _currentCharge;
            }
        }
        
        _controller.Move(new Vector3(movementVector.x,-9.82f*Time.deltaTime,movementVector.y) * (speed * Time.deltaTime*(Mathf.Clamp(2-_currentCharge*2,0,1))));

        if (movementVector.magnitude > 0)
        {
            _lookAtVector = new Vector3(movementVector.x, 0, movementVector.y);
            _animator.Run();
        }
        else
        {
            _animator.Idle();
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

    public void OnFire(InputValue vec)
    {
        Single pressVal = vec.Get<Single>();
        
        if (pressVal < 0.5 && charging)
        {
            ShootBall();
            _currentCharge = 0;
            charging = false;
            chargeIndicator.fillAmount = _currentCharge;
        }

        charging = pressVal > 0.5f;
    }

    public void ShootBall()
    {
        if (heldBall)
        {
            
            heldBall.transform.parent = null;

            Vector3 dir = GetAssistedAim();

            if (dir == -Vector3.one)
            {   
                heldBall.Shoot(gameObject,_lookAtVector,_currentCharge >= 0.99 ? 1.5f : _currentCharge);
            }
            else
            {
                heldBall.Shoot(gameObject,dir,_currentCharge);
            }
            
            _animator.Kick();
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
            HitPlayer();
        }
    }

    public void HitPlayer()
    {
        if (!invulnerable)
        {
            Debug.Log($"Health: {health}");
            health -= 1;
            invulnerable = true;
            StartCoroutine(Invulnerability());
        }
    }

    private IEnumerator Invulnerability()
    {
        // foreach (var mat in _mr.materials)
        // {
        //     mat.
        // }
        for (int i = 0; i < invulnTimer / blinkTimer / 2; i++)
        {
            _mr.enabled = false;

            yield return new WaitForSeconds(blinkTimer);

            _mr.enabled = true;
            
            yield return new WaitForSeconds(blinkTimer);
        }

        invulnerable = false;

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool fired = false;
    public bool held;
    public int bounces;

    protected Rigidbody _rb;
    public GameObject owner;

    public float shootSpeed = 50f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (held)
        {
            _rb.velocity = Vector3.zero;
        }

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public bool HeldOrFired => fired || held;


    public virtual void Shoot(GameObject shooter, Vector3 dir, float chargeModifier)
    {
        owner = shooter;
        fired = true;
        held = false;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.velocity = dir * chargeModifier * shootSpeed;
    }


    public void Pickup()
    {
        held = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Wall")) return;
        
        bounces--;

        if (bounces <= 0)
        {
            Destroy(gameObject);
        }
    }
}

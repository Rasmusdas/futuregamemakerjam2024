using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool fired = false;
    public bool held;
    public int bounces;
    public float lifeTime = 2;

    private Rigidbody _rb;
    public Rigidbody Rb
    {
        get
        {
            if (_rb)
            {
                return _rb;
            }
            _rb = GetComponent<Rigidbody>();
            return _rb;
        }
    }
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
            Rb.velocity = Vector3.zero;
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
        Rb.constraints = RigidbodyConstraints.None;
        Rb.velocity = dir * chargeModifier * shootSpeed;
        
        Destroy(gameObject,lifeTime);
    }


    public void Pickup()
    {
        held = true;
        Rb.constraints = RigidbodyConstraints.FreezeAll;
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

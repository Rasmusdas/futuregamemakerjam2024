using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public bool fired = false;
    public bool held;

    private Rigidbody _rb;
    public GameObject owner;

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
    }

    public bool HeldOrFired => fired || held;


    public void Shoot(GameObject shooter, Vector2 dir, float power)
    {
        owner = shooter;
        fired = true;
        held = false;
        _rb.constraints = RigidbodyConstraints.None;

        _rb.velocity = new Vector3(dir.x, 0, dir.y) * power;


    }


    public void Pickup()
    {
        held = true;

        _rb.constraints = RigidbodyConstraints.FreezeAll;

    }
}

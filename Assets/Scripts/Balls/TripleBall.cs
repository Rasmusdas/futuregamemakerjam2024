using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TripleBall : Ball
{
    public GameObject ball;
    public float timeTillSplit;
    public float splitScale;
    public float angle;

    public override void Shoot(GameObject shooter, Vector3 dir, float chargeModifier)
    {
        StartCoroutine(Split(dir));
        base.Shoot(shooter, dir, chargeModifier);
    }

    IEnumerator Split(Vector3 dir)
    {
        yield return new WaitForSeconds(timeTillSplit);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        Quaternion reverseRotation = Quaternion.Euler(0, -angle, 0);
        Vector3 rotatedVector = rotation * dir;
        Vector3 reverseRotatedVector = reverseRotation * dir;
        
        GameObject rotBall = Instantiate(ball, transform.position, Quaternion.identity);
        Collider collider1 = rotBall.GetComponent<Collider>();

        GameObject rev_ball = Instantiate(ball, transform.position, Quaternion.identity);
        Collider collider2 = rev_ball.GetComponent<Collider>();

        Collider collider3 = GetComponent<Collider>();
        
        Physics.IgnoreCollision(collider1, collider2);
        Physics.IgnoreCollision(collider2, collider3);
        Physics.IgnoreCollision(collider1, collider3);
        
        rotBall.GetComponent<Rigidbody>().velocity = _rb.velocity.magnitude * reverseRotatedVector;
        rev_ball.GetComponent<Rigidbody>().velocity = _rb.velocity.magnitude * rotatedVector;
        
        rotBall.GetComponent<Ball>().owner = owner;
        rev_ball.GetComponent<Ball>().owner = owner;

        rotBall.transform.localScale = Vector3.one * splitScale;
        rev_ball.transform.localScale = Vector3.one * splitScale;
        transform.localScale = Vector3.one * splitScale;
        
        Destroy(rotBall,lifeTime-timeTillSplit);
        Destroy(rev_ball,lifeTime-timeTillSplit);
    }
}

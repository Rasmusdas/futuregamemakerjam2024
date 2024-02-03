using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    public BallSpawnPoint[] spawnPoints;
    public GameObject ball;
   
    void FixedUpdate()
    {

        SpawnBall();

        

    }


    int _CooldownTime = 0;
    
    public int CooldownTimer = 30;

    void SpawnBall()
    {
        foreach (var sp in spawnPoints)
        {
            sp.SpawnBall();
        }

    }
    
    
    
}

[Serializable]
public class BallSpawnPoint
{
    public Transform spawnPoint;
    public GameObject[] balls;
    public Ball ball;
    
    public bool CanSpawnBall
    {
        get
        {

            foreach (PlayerController obj in GameObject.FindObjectsOfType(typeof(PlayerController)))
            {
                if (Vector3.Distance(obj.transform.position, spawnPoint.position) < 3)
                {
                    return false;
                }
            }
            
            if (ball)
            {
                if (ball.HeldOrFired || ball.owner)
                {
                    return true;
                }
                
                return false;
            }

            return true;
        }
    }

    public void SpawnBall()
    {
        if (CanSpawnBall)
        {
            GameObject gb = Object.Instantiate(balls[Random.Range(0, balls.Length)], spawnPoint);

            ball = gb.GetComponent<Ball>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dommer_Spawner : MonoBehaviour
{
    
    public GameObject ball;
   
    void FixedUpdate()
    {

        SpawnBall();

        

    }

    public int _BallCount = 0;
    int _CooldownTime = 0;
    
    public int CooldownTimer = 30;

    void SpawnBall()
    {



        if (_CooldownTime == CooldownTimer)
        {
            Instantiate(ball);

            _CooldownTime = 0;
            _BallCount++;
        }
        else
        {
            _CooldownTime++;
        }




       

        

        




       



    }





}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Judge : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public Transform hand;

    public PlayerAnimations _anim;

    public GameObject[] balls;

    public float height;

    public float spawnTimer = 5;

    private float _timeSinceSpawn;
    private Vector3 _prevTarget;
    private Vector3 _currTarget;

    public GameObject crossHair;
    public Transform canvas;

    public bool firstStart;
    public bool firstEnd;
    
    private Ball _currBall;

    public SoundManager sndMan;

    public void StopGame()
    {
        sndMan.EndWhistle();
    }
    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("BallSpawn");
        _prevTarget = transform.position+transform.forward;
        _anim = GetComponent<PlayerAnimations>();
        _timeSinceSpawn = 0;
    }

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
        if (!PlayerSpawnManager.Instance.started) return;
        
        if (!firstStart)
        {
            firstStart = true;
            _timeSinceSpawn = -100;
            StartCoroutine(StartGameSetup());
        }

        var players = PlayerSpawnManager.Instance.players;
        var playersWithHealth = players.FindAll(x => x.health > 0);

        if (playersWithHealth.Count <= 1 && PlayerSpawnManager.Instance.started)
        {
            if (!firstEnd)
            {
                _anim.JudgeStop();
                firstEnd = true;
            }
        }
        else
        {
            _timeSinceSpawn += Time.deltaTime;

            if (_timeSinceSpawn >= spawnTimer)
            {
                _timeSinceSpawn = 0;
                _anim.Throw();
            }
        }

    }

    private IEnumerator StartGameSetup()
    {
        sndMan.CountDown();
        yield return new WaitForSeconds(sndMan.countDown.length/2);
        _anim.JudgeStart();
        yield return new WaitForSeconds(sndMan.countDown.length/2);
        sndMan.StartWhistle();
        yield return new WaitForSeconds(sndMan.startWhistle.length);
        _timeSinceSpawn = 1;
        sndMan.PlayMusic();

    }

    void ReleaseBall()
    {
        StartCoroutine(MoveBall(_currBall.transform.position, _currTarget));
    }
    void SpawnBall()
    {
        float angle = Random.value * Mathf.PI * 2;
        _currTarget = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(Mathf.Sin(angle)*1.5f,0,Mathf.Cos(angle)*1.5f);
        StartCoroutine(Crosshair(_currTarget));
        StartCoroutine(TurnTowards(_currTarget));
        _currBall = Instantiate(balls[Random.Range(0, balls.Length)],hand).GetComponent<Ball>();
        _currBall.transform.localPosition = Vector3.zero;
        _currBall.Rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    IEnumerator TurnTowards(Vector3 target)
    {
        float t = 0;
        Vector3 lookTarget = target;
        lookTarget.y = transform.position.y;
        while (t <= 1)
        {
            
            transform.LookAt(Vector3.Lerp(_prevTarget,lookTarget,t));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _prevTarget = lookTarget;
    }
    IEnumerator Crosshair(Vector3 target)
    {
        target.y = 0;
        GameObject ch = Instantiate(crossHair, target, canvas.transform.rotation,canvas);
        float t = 0;
        Vector3 forwardVec = transform.forward;
        while (t < 6*Mathf.PI)
        {
            float tSin = Mathf.Sin(t)/4;
            ch.transform.localScale = new Vector3(tSin+1.25f, tSin+1.25f, 1);
            yield return new WaitForEndOfFrame();
            t+=Time.deltaTime*2*Mathf.PI;
        }
        
        Destroy(ch);
    }

    IEnumerator MoveBall(Vector3 start, Vector3 target)
    {
        _currBall.transform.parent = null;
        float t = 0;
        
        float parabola;
        Vector3 targetPos;

        float diff = hand.position.y - (height - 4 * height * (t - 0.5f) * (t - 0.5f));
        
        parabola = height - 4* height * (t - 0.5f) * (t - 0.5f)+diff;
        
        while (parabola > 0.5f)
        {
            t += Time.deltaTime/2;
            parabola = height - 4 * height * (t - 0.5f) * (t - 0.5f)+ diff;
            targetPos = Vector3.Lerp(start, target, t);
            targetPos.y = parabola;
            _currBall.transform.position = targetPos;
            yield return new WaitForEndOfFrame();
        }
        
        targetPos = target;
        targetPos.y = parabola+0.5f;
        _currBall.transform.position = targetPos;

        if (!_currBall.held)
        {
            _currBall.Rb.constraints = RigidbodyConstraints.None;
            SoundManager.instance.BallHitSound();
        }


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager instance;
    public new AudioSource audio;

    public AudioClip explosion;
    public AudioClip ballHit;
    //public AudienceCheer audience;
    public AudioClip music;
    public AudioClip startWhistle;
    public AudioClip endWhistle;
    public AudioClip youWin;
    public AudioClip countDown;
    public AudioClip kickBall;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       audio = GetComponent <AudioSource>();
       
    }

    public void ExplosionSound()
    {
        audio.PlayOneShot(explosion);
    }
    public void BallHitSound()
    {
        audio.PlayOneShot(ballHit);
    }

    public void PlayMusic()
    {
        audio.PlayOneShot(music);
    }

    public void StartWhistle()
    {
        audio.PlayOneShot(startWhistle);
    }

    public void EndWhistle()
    {
        audio.PlayOneShot(endWhistle);
    }

    public void YouWin()
    {
        audio.PlayOneShot(youWin);
    }

    public void CountDown()
    {
        audio.PlayOneShot(countDown);
    }
    
    public void KickBall()
    {
        audio.PlayOneShot(kickBall);
    }


}

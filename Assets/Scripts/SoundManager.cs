using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audio;

    public AudioClip explosion;
    public AudioClip ballHit;
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

}

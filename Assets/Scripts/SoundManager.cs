using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public new AudioSource audio;

    public AudioClip explosion;
    public AudioClip ballHit;
    //public AudienceCheer audience;

   

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

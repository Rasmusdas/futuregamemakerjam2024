using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceCheer : MonoBehaviour
{

    [SerializeField]private List<AudioClip> CheerSounds = new List<AudioClip>();
    public AudioSource audioSource;

    private bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomCheer()
    {
        if (!isPlaying)
        {
            var rand = Random.Range(0, CheerSounds.Count);
            audioSource.PlayOneShot(CheerSounds[rand]);

            isPlaying = true;

            StartCoroutine(ResetIsPlaying(audioSource.clip.length));
        }
    }
    private void Update()
    {
      PlayRandomCheer();
    }

    IEnumerator ResetIsPlaying(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlaying = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioClip> audioClips;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShotFire()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }
    public void WalkWithRifleSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
        //audioSource.PlayOneShot(audioClips[2]);
        //audioSource.PlayOneShot(audioClips[3]);

    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }
    public void LandSound()
    {
        audioSource.PlayOneShot(audioClips[2]);
    }
}

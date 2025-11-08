using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSrc => GetComponent<AudioSource>();

    protected virtual void Awake()
    {
        audioSrc.playOnAwake = false;
    }

    public void PlaySound(int i, float volume = 1f, float p1 = 0.85f, float p2 = 1.2f, bool destroyed = false)
    {
        AudioClip clip = sounds[i];
        audioSrc.pitch = Random.Range(p1, p2);

        if(destroyed)
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        else 
            audioSrc.PlayOneShot(clip, volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioScr => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1f, bool destroyed = false, float p1 = 0.85f, float p2 = 1.2f)
    {
        audioScr.pitch = Random.Range(p1, p2);
        audioScr.PlayOneShot(clip, volume);
    }
}

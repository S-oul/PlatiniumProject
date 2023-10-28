using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _music;

    void Start()
    {
        _music.loop = true;
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfx.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _music.clip = clip;
        _music.Play();
    }
}

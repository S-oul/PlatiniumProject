using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _music;

    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioClip _gameMusicSubOneMinute;

    float restingTime = 0;

    void Start()
    {
        _music.loop = true;
        if(_gameMusic != null) { PlayMusic(_gameMusic); }
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

    public IEnumerator FadeToZero(float timeToFade)
    {
        restingTime = timeToFade;

        while(restingTime > 0)
        {
            restingTime -= Time.deltaTime;
            float percent = restingTime / timeToFade;
            print(percent);
            _music.volume = percent;
            yield return null;
        }
        _music.volume = 0;
    }
}

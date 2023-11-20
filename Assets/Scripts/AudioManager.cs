using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _music;

    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioClip _gameMusicSubOneMinute;

    float restingTime = 0;
    float oldVol = 0;

    void Start()
    {
        _music.loop = true;
        if(_gameMusic != null) { PlayMusic(_gameMusic); }
        //StartCoroutine(FadeToZero(1));
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
        oldVol = _music.volume;
        while(restingTime > 0)
        {
            restingTime -= Time.deltaTime;
            float percent = restingTime / timeToFade;
            print(restingTime + " " + oldVol + " " + Mathf.Lerp(0, oldVol, percent));
            _music.volume = Mathf.Lerp(0, oldVol,percent);
            yield return null;
        }
        _music.volume = 0;
        restingTime = 0;
        oldVol = 0;
    }
}

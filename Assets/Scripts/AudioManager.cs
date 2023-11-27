using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource _sfx;
    [SerializeField] List<AudioSource> allSoundSource;
    [SerializeField] AudioSource _music;

    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioClip _gameMusicSubOneMinute;

    float restingTime = 0;
    float oldVol = 0;

    public List<SoundConfig> soundBank = new();

    public List<AudioSource> AllSoundSource { get => allSoundSource; set => allSoundSource = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        _music.loop = true;
        if(_gameMusic != null) { PlayMusic(_gameMusic); }
        //StartCoroutine(FadeToZero(1));
        
    }

    public void PlaySFXOS(string clipName, AudioSource source)
    {
        AudioClip clip = FindClip(clipName);
        
        if(clip != null)
        {
            source.PlayOneShot(clip);

        }
        
    }

    public void PlaySFXOS(AudioClip clip, AudioSource source)
    {

        if (clip != null)
        {
            source.PlayOneShot(clip);

        }

    }

    public void PlaySFXLoop(AudioClip clip, AudioSource source)
    {


        if (clip != null)
        {
            source.clip = clip;
            source.Play();

        }

    }

    public void StopSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        
    }

    public AudioClip FindClip(string clipName)
    {
        AudioClip clip = null;
        foreach (SoundConfig sound in soundBank)
        {
            if (sound.audioName == clipName)
            {
                if (sound.audio.Count > 1)
                {
                    clip = sound.audio[Random.Range(0, sound.audio.Count)];
                    break;
                }

                else if (sound.audio.Count == 1)
                {
                    clip = sound.audio[0];
                    break;
                }

            }
        }
        return clip;
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

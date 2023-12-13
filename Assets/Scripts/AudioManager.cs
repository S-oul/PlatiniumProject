using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource _sfx;
    [SerializeField] List<AudioSource> allSoundSource;
    [SerializeField] AudioSource _music;

    float restingTime = 0;
    float oldVol = 0;

    public List<SoundConfig> soundBank = new();

    public List<AudioSource> AllSoundSource { get => allSoundSource; set => allSoundSource = value; }
    public AudioSource Music { get => _music; set => _music = value; }
    public AudioSource Sfx { get => _sfx; set => _sfx = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }


    public void PlaySFXOS(string clipName, AudioSource source)
    {
        AudioClip clip = FindClip(clipName);

        if (clip != null)
        {
            source.loop = false;
            source.PlayOneShot(clip);

        }

    }

    public void PlaySFXOS(AudioClip clip, AudioSource source)
    {

        if (clip != null)
        {
            source.loop = false;
            source.PlayOneShot(clip);

        }

    }

    public void PlaySFXLoop(AudioClip clip, AudioSource source)
    {


        if (clip != null)
        {
            source.loop = true;
            source.clip = clip;
            source.Play();

        }

    }

    public void PlaySFXLoop(string clipName, AudioSource source)
    {

        AudioClip clip = FindClip(clipName);
        if (clip != null)
        {
            source.loop = true;
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

    public void PlayMusic(string clipName)
    {
        AudioClip clip = FindClip(clipName);
        Music.clip = clip;
        Music.Play();
        Music.loop = true;
    }

    public IEnumerator FadeToZero(float timeToFade)
    {
        restingTime = timeToFade;
        oldVol = Music.volume;
        while (restingTime > 0)
        {
            restingTime -= Time.deltaTime;
            float percent = restingTime / timeToFade;
            print(restingTime + " " + oldVol + " " + Mathf.Lerp(0, oldVol, percent));
            Music.volume = Mathf.Lerp(0, oldVol, percent);
            yield return null;
        }
        Music.volume = 0;
        restingTime = 0;
        oldVol = 0;
    }



    public void DestroySelf()
    {
        Destroy(this.gameObject);
        DestroyImmediate(this.gameObject);
        //return this.gameObject;
    }
}

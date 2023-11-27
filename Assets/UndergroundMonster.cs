using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundMonster : InteractableNPC
{
    AudioSource _audioSource;
    bool _monsterSoundCanPlay;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void VocalTalk()
    {
        if (_monsterSoundCanPlay)
        {
            StartCoroutine(PlaySoundMonster());
        }
        
    }
    IEnumerator PlaySoundMonster()
    {
        _monsterSoundCanPlay = false;
        AudioClip clip = AudioManager.instance.FindClip("UndergroundCreature");
        AudioManager.instance.PlaySFXOS(clip, _audioSource);
        yield return new WaitForSeconds(clip.length + 1f);
        _monsterSoundCanPlay = true;
    }
}

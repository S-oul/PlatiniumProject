using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundMonster : InteractableNPC
{
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void VocalTalk()
    {
        AudioManager.instance.PlaySFXOS("UndergroundCreature", _audioSource);
    }
}

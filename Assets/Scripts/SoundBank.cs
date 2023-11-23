using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundConfig
{
    public string audioName;
    public string description;
    public List<AudioClip> audio;
}

[CreateAssetMenu(fileName = "SoundBank", menuName = "Assets/SoundBank")]
public class SoundBank : ScriptableObject
{
    public List<SoundConfig> soundBank = new();

}

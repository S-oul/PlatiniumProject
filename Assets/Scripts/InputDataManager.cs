using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputConfig
{
    public string baseName;

    [Header("Xbox")]
    public string nameXbox;
    public Sprite spriteXbox;

    [Header("Playstation")]
    public string namePlaystation;
    public Sprite spritePlaystation;

    [Header("Switch")]
    public string nameSwitch;
    public Sprite spriteSwitch;

}

[CreateAssetMenu(fileName = "InputDataManager", menuName = "Assets/InputDataManager")]
public class InputDataManager : ScriptableObject
{
    public List<InputConfig> inputs = new();
}
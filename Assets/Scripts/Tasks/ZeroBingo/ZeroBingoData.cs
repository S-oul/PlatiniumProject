using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WordConfig
{
    public string baseWord;
    [Serializable]
    public struct WordWrapper
    {
        public SystemLanguage language;
        public string word;
    }
    public List<WordWrapper> traductions;
}

[CreateAssetMenu(fileName = "ZeroBingoData", menuName = "Assets/DataTask/ZeroBingoData")]
public class ZeroBingoData: ScriptableObject
{
    public List<WordConfig> words = new();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataRoom", menuName = "Assets/DataRoom")]
public class DataRoom : ScriptableObject
{
    [Range(1, 3)]
    public int _roomSize;


}

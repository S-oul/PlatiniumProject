using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStateScreen : MonoBehaviour
{

    [SerializeField] Image _background;
    public void ChangeColor(Color color)
    {
        _background.color = color;
    }
}

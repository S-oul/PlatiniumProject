using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayersLeftText : MonoBehaviour
{
    TextMeshProUGUI _text;
    Color _color;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _color = _text.color;
        StartCoroutine(FlashText());
    }

    IEnumerator FlashText()
    {
        float time = 1f;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            _text.color = new Color(_color.r, _color.g, _color.b, time);    
            yield return null;

        }
        while (time < 1f)
        {
            time += Time.deltaTime;
            _text.color = new Color(_color.r, _color.g, _color.b, time);
            yield return null;

        }
        StartCoroutine(FlashText());
    }
}

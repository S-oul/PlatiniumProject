using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINpc : MonoBehaviour
{
    Canvas _canvas;

    Transform _talkingBubble;
    TextMeshProUGUI _talkingBubbleText;
    Image _talkingBubbleImage;
    void Start()
    {
        _canvas = transform.Find("Canvas").GetComponent<Canvas>();
        _talkingBubble = _canvas.transform.Find("TalkingBubble");
        _talkingBubbleImage = _talkingBubble.Find("Image").GetComponent<Image>();
        _talkingBubbleText = _talkingBubble.Find("Text").GetComponent<TextMeshProUGUI>();
        //print(_canvas + " " + _talkingBubble + " " + _talkingBubbleText + " "+ _talkingBubbleImage );
    }


    public void ChangeBubbleImage(Sprite image)
    {
        _talkingBubbleImage.sprite = image;
    }
    public void ChangeBubbleText(string text)
    {
        _talkingBubbleText.text = text;
    }
    public void DisplayTalkingBubble(bool isActive)
    {
        _talkingBubble.gameObject.SetActive(isActive);
    }

    

}

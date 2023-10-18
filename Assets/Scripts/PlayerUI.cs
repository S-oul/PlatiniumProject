using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    [Header("InputsUI")]
    [SerializeField] Canvas _canvas;

    //QTE UI
    Transform _qteUI;
    TextMeshProUGUI _textInputsUI;
    Slider _sliderInputsUI;
    Image _input;

   /*[HideInInspector]*/ public float _sliderPercentValue;

    private void Start()
    { 
        _qteUI = _canvas.transform.Find("QTEInputs");
        _textInputsUI = _qteUI.transform.Find("TextInputs").Find("Text").GetComponent<TextMeshProUGUI>();
        _sliderInputsUI = _qteUI.transform.Find("Slider").GetComponent<Slider>();
        _input = _sliderInputsUI.gameObject.transform.Find("SmallerCircle").Find("Image").GetComponent<Image>();
    }
    public void ChangeUIInputs(string text)
    {
        _textInputsUI.text = text;
    }

    public void ChangeUIInputs(Color color)
    {
        _input.color = color;
    }

    private void Update()
    {
        _sliderInputsUI.value = _sliderPercentValue;
    }
}

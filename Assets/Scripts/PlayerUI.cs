using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    [Header("InputsUI")]
    [SerializeField] TextMeshProUGUI _textInputsUI;
    [SerializeField] Slider _sliderInputsUI;
    [SerializeField] Image _input;

   /*[HideInInspector]*/ public float _sliderPercentValue;


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

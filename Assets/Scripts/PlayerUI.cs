using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class PlayerUI : MonoBehaviour
{

    [Header("InputsUI")]
    [SerializeField] Canvas _canvas;

    //QTE UI
    Transform _qteUI;
    TextMeshProUGUI _textInputsUI;
    Slider _sliderInputsUI;
    Image _input;
    List<Transform> _validationInputs = new List<Transform>();

   /*[HideInInspector]*/ public float _sliderPercentValue;

    private void Start()
    { 
        _qteUI = _canvas.transform.Find("QTEInputs");
        _textInputsUI = _qteUI.transform.Find("TextInputs").Find("Text").GetComponent<TextMeshProUGUI>();
        _sliderInputsUI = _qteUI.transform.Find("Slider").GetComponent<Slider>();
        _input = _sliderInputsUI.gameObject.transform.Find("SmallerCircle").Find("Image").GetComponent<Image>();
        Transform validation = _qteUI.transform.Find("Validation");
        foreach (Transform validationInput in validation)
        {
            _validationInputs.Add(validationInput);
        }
    }
    public void ChangeUIInputs(string text)
    {
        _textInputsUI.text = text;
    }

    public void ChangeUIInputs(Color color)
    {
        _input.color = color;
    }

    public void ChangeUIInputsValidation(int index, Color color)
    {
        _validationInputs[index].GetComponent<Image>().color = color;
    }

    public void ClearUIInputsValidation()
    {
        foreach (Transform validationInput in _validationInputs)
        {
            validationInput.GetComponent<Image>().color = Color.white;
        }

    }

    public void ClearUIInputs()
    {
        ChangeUIInputs(Color.white);
        ChangeUIInputs("");
        _sliderInputsUI.value = 1;
        ClearUIInputsValidation();
    }

    private void Update()
    {
        _sliderInputsUI.value = _sliderPercentValue;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class PlayerUI : MonoBehaviour
{

    
    [SerializeField] Canvas _canvas;

    [Header("InputsUI")]
    Transform _qteUI;
    TextMeshProUGUI _textInputsUI;
    Slider _sliderInputsUI;
    Image _input;
    bool qteInputIsActive = false;
    [HideInInspector] public float _sliderPercentValue;
    Slider _validationBadInputSlider;
   

    [Header("Duolingo")]
    Transform _duolingoUI;
    List<TextMeshProUGUI> _answersDuolingo = new List<TextMeshProUGUI>();
    private void Start()
    {
        
        StartUI();
        DisplayInputsUI(false);
    }


    #region InputsUI
    void StartUI()
    {
        //QTE
        _qteUI = _canvas.transform.Find("QTEInputs");
        _textInputsUI = _qteUI.transform.Find("TextInputs").Find("Text").GetComponent<TextMeshProUGUI>();
        _sliderInputsUI = _qteUI.transform.Find("Slider").GetComponent<Slider>();
        _input = _sliderInputsUI.gameObject.transform.Find("SmallerCircle").Find("Image").GetComponent<Image>();
        _validationBadInputSlider = _qteUI.transform.Find("Validation").Find("BadInputs").GetComponent<Slider>();

        //Duolingo
        _duolingoUI = _canvas.transform.Find("DuolingoInputs");
        foreach (Transform answer in _duolingoUI)
        {
            TextMeshProUGUI tempText = answer.Find("TextAnswer").GetComponent<TextMeshProUGUI>();
            _answersDuolingo.Add(tempText);
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

    public void ChangeUIInputsValidation(float value)
    {
        _validationBadInputSlider.value = value;
    }

/*    public void ClearUIInputsValidation()
    {
        foreach (Transform validationInput in _validationInputs)
        {
            validationInput.GetComponent<Image>().color = Color.white;
        }

    }*/

    public void ClearUIInputs()
    {
        ChangeUIInputs(Color.white);
        ChangeUIInputs("");
        _sliderInputsUI.value = 1;
/*        ClearUIInputsValidation();*/
    }



    public void DisplayAnswersDuolingo(List<string> words)
    {
        List<string> inputs = new List<string>() { "Y", "X", "B" };
        if (words == null) { return; }
        for (int i = 0; i < 3; i++)
        {
            string tempInput = inputs[Random.Range(0, inputs.Count)];
            _answersDuolingo[i].text =  tempInput + ": " + words[i];
            inputs.Remove(tempInput);
        }
    }



    public void DisplayQTEUI(bool value)
    {
        qteInputIsActive = value;
        _qteUI.gameObject.SetActive(value);
        
    }

    public void DisplayDuolingoUI(bool value)
    {
       
        _duolingoUI.gameObject.SetActive(value);
    }

    public void DisplayInputsUI(bool value)
    {
        DisplayDuolingoUI(value);
        DisplayQTEUI(value);
    }

    #endregion


    private void Update()
    {
        if(qteInputIsActive) 
        {
            _sliderInputsUI.value = _sliderPercentValue;
        }
        
    }

    
}

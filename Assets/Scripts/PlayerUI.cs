using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;

public class PlayerUI : MonoBehaviour
{

    
    [SerializeField] Canvas _canvas;

    [Header("InputsUI")]
    Transform _qteUI;
    TextMeshProUGUI _textInputsUI;
    Slider _sliderInputsUI;
    Image _input;
    bool qteInputIsActive = false;
    float sliderPercentValue;
    Slider _validationBadInputSlider;
    Image _roundInputTimer;
    Vector3 _roundTimerOriginalSize;
   

    [Header("Duolingo")]
    Transform _duolingoUI;
    List<TextMeshProUGUI> _answersDuolingo = new List<TextMeshProUGUI>();

    [Header("Le Code")]
    Transform _leCodeUI;
    Image _up;
    Image _down;
    Image _right;
    Image _left;

    public float SliderPercentValue { get => sliderPercentValue; set => sliderPercentValue = value; }
    public Image RoundInputTimer { get => _roundInputTimer; set => _roundInputTimer = value; }

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
        RoundInputTimer = _sliderInputsUI.transform.GetChild(3).GetComponent<Image>();
        _roundTimerOriginalSize = RoundInputTimer.transform.localScale;
        DisplayCowboyQTEUI(false);
        DisplayVolleyQTEUI(false);


        //Duolingo
        _duolingoUI = _canvas.transform.Find("DuolingoInputs");
        foreach (Transform answer in _duolingoUI)
        {
            TextMeshProUGUI tempText = answer.Find("TextAnswer").GetComponent<TextMeshProUGUI>();
            _answersDuolingo.Add(tempText);
        }

        //Le Code
        _leCodeUI = _canvas.transform.Find("CodeInputs");
        _up = _leCodeUI.transform.GetChild(0).GetComponent<Image>();
        _right = _leCodeUI.transform.GetChild(1).GetComponent<Image>();
        _left = _leCodeUI.transform.GetChild(2).GetComponent<Image>();
        _down = _leCodeUI.transform.GetChild(3).GetComponent<Image>();

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

    public void DisplayCowboyQTEUI(bool value)
    {
        _sliderInputsUI.transform.GetChild(0).gameObject.SetActive(value);
        _sliderInputsUI.transform.GetChild(1).gameObject.SetActive(value);
    }

    public void ClearUIInputs()
    {
        ChangeUIInputs(Color.white);
        ChangeUIInputs("");
        _sliderInputsUI.value = 1;
/*        ClearUIInputsValidation();*/
    }

    public void DisplayQTEUI(bool value)
    {
        qteInputIsActive = value;
        _qteUI.gameObject.SetActive(value);

    }

    public void DisplayVolleyQTEUI(bool value)
    {
        _sliderInputsUI.transform.GetChild(3).gameObject.SetActive(value);
    }
    
    public void ChangeRoundTimerValue(float percent)
    {
        _roundInputTimer.transform.localScale = Vector3.Lerp(_roundTimerOriginalSize, _input.transform.localScale, percent);
    }

    public void ResetRoundTimerQTE()
    {
        RoundInputTimer.transform.localScale = _roundTimerOriginalSize;
    }

    public void DisplayAnswersDuolingo(List<string> words, List<string> inputs)
    {
        if (words == null) { return; }
        for (int i = 0; i < 3; i++)
        {
            _answersDuolingo[i].text = inputs[i] + ": " + words[i];
        }
    }

    public void ClearAnswersDuolingo()
    {
        for (int i = 0; i < 3; i++)
        {
            _answersDuolingo[i].text = "";
        }
    }

    

    public void DisplayDuolingoUI(bool value)
    {
       
        _duolingoUI.gameObject.SetActive(value);
    }

    public void DisplayLeCodeUI(bool value)
    {
        _leCodeUI.gameObject.SetActive(value);
    }

    public void DisplayInputsUI(bool value)
    {
        DisplayDuolingoUI(value);
        DisplayQTEUI(value);
        DisplayLeCodeUI(value);
    }

    #endregion


    private void Update()
    {
        if(qteInputIsActive) 
        {
            _sliderInputsUI.value = SliderPercentValue;
        }
        
    }

    
}

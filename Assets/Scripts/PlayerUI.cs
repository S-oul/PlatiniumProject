using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerUI : MonoBehaviour
{

    
    [SerializeField] Canvas _canvas;

    [Header("InputsUI")]
    Transform _qteUI;
    TextMeshProUGUI _textInputsUI;
    Slider _sliderInputsUI;
    Image _inputQTE;
    Transform _inputToPress;
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


    [Header("MashDownButton")]
    Transform _mashDownTransform;

    public float SliderPercentValue { get => sliderPercentValue; set => sliderPercentValue = value; }
    public Image RoundInputTimer { get => _roundInputTimer; set => _roundInputTimer = value; }
    

    private void Start()
    {

        StartUI();
        DisplayInputsTaskUI(false);
    }


    #region InputsUI
    void StartUI()
    {
        //QTE
        _qteUI = _canvas.transform.Find("QTEInputs");
        _textInputsUI = _qteUI.transform.Find("TextInputs").Find("Text").GetComponent<TextMeshProUGUI>();
        _sliderInputsUI = _qteUI.transform.Find("Slider").GetComponent<Slider>();
        _inputQTE = _qteUI.transform.Find("Slider").Find("SmallerCircle").Find("Image").GetComponent<Image>();
        _validationBadInputSlider = _qteUI.transform.Find("Validation").Find("BadInputs").GetComponent<Slider>();
        _inputToPress = _canvas.transform.Find("InputToPress");
        /*RoundInputTimer = _sliderInputsUI.transform.GetChild(3).GetComponent<Image>();*/
       /* _roundTimerOriginalSize = RoundInputTimer.transform.localScale;*/
        DisplayCowboyQTEUI(false);
        DisplayVolleyQTEUI(false);
        DisplayInputToPress(false, "");


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

        // MashDOwnButton
        _mashDownTransform = _canvas.transform.Find("MashDownButton");

    }

    public void ChangeInputValueUI(string text)
    {
        _textInputsUI.text = text;
    }

    public void ChangeInputValueUI(Sprite image)
    {
        _inputQTE.sprite = image;
    }

    public void ChangeUIInputs(Color color)
    {
        _inputQTE.color = color;
    }

    public void ChangeUIInputsValidation(float value)
    {
        _validationBadInputSlider.value = value;
    }

    public void DisplayCowboyQTEUI(bool value)
    {
        _sliderInputsUI.transform.GetChild(0).gameObject.SetActive(value);
        _sliderInputsUI.transform.GetChild(1).gameObject.SetActive(value);
        _validationBadInputSlider.gameObject.SetActive(value);
    }

    public void ClearUIInputs()
    {
        ChangeUIInputs(Color.white);
        ChangeInputValueUI("");
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
        //_sliderInputsUI.transform.GetChild(3).gameObject.SetActive(value);
    }
    
    public void ChangeRoundTimerValue(float percent)
    {
        _roundInputTimer.transform.localScale = Vector3.Lerp(_roundTimerOriginalSize, _inputQTE.transform.localScale, percent);
    }

    /*public void ResetRoundTimerQTE()
    {
        RoundInputTimer.transform.localScale = _roundTimerOriginalSize;
    }
*/
    public void DisplayInputToPress(bool value, string input)
    {
        _inputToPress.gameObject.SetActive(value);
        Sprite inputSprite = DataManager.Instance.FindInputSprite(input, gameObject.GetComponent<PlayerController>().Type);
        _inputToPress.Find("InputImage").GetComponent<Image>().sprite = inputSprite;

    }

    public void DisplayAnswersDuolingo(List<string> words, List<string> inputs)
    {
        if (words == null) { return; }
        for (int i = 0; i < 3; i++)
        {
            _answersDuolingo[i].text = /*inputs[i] + ": " +*/ words[i];
            string inputName = "";
            switch (i)
            {
                case 0:
                    inputName = "Y";
                    break;
                case 1:
                    inputName = "X";
                    break;
                case 2:
                    inputName = "B";
                    break;
            }
            _answersDuolingo[i].transform.parent.Find("Image").GetComponent<Image>().sprite = DataManager.Instance.FindInputSprite(inputName, gameObject.GetComponent<PlayerController>().Type);
        }
    }


    public void ClearAnswersDuolingo()
    {
        ClearColorAnswerBubble();
        for (int i = 0; i < 3; i++)
        {
            _answersDuolingo[i].text = "";
        }
    }

    public void ChangeColorAnswerBubble(WordConfig word, Color color)
    {

        string text = word.baseWord;
        foreach (TextMeshProUGUI answer in _answersDuolingo)
        {


            if (answer.text == text)
            {
                answer.transform.parent.Find("Background").GetComponent<Image>().color = color;
            }

        }
    }

    public void ClearColorAnswerBubble()
    {
        foreach (TextMeshProUGUI answer in _answersDuolingo)
        {
            answer.transform.parent.Find("Background").GetComponent<Image>().color = Color.white;
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

    public void DisplayMashDownButton(bool value, string inputName)
    {
        _mashDownTransform.gameObject.SetActive(value);
        _mashDownTransform.GetChild(0).GetComponent<Image>().sprite = DataManager.Instance.FindInputSprite(inputName, gameObject.GetComponent<PlayerController>().Type);
        _mashDownTransform.GetComponent<MashDownButton>().ChangeSwap(value);
    }

    public void DisplayInputsTaskUI(bool value)
    {
        DisplayDuolingoUI(value);
        DisplayQTEUI(value);
        DisplayLeCodeUI(value);
        DisplayMashDownButton(value, "");
    }

    public void DisplayInputUI(bool value)
    {
        DisplayQTEUI(value);
        DisplayCowboyQTEUI(!value);

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

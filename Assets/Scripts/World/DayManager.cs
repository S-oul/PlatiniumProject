using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DayManager : MonoBehaviour
{
    DayTimer _dayTimer;
    DaySlider _daySlider;

    [SerializeField] Text _textDay;

    public DayTimer DayTimer { get => _dayTimer; set => _dayTimer = value; }
    public DaySlider DaySlider { get => _daySlider; set => _daySlider = value; }

    private void Start()
    {
        GameManager.Instance.DayManager = this;
        _dayTimer = FindObjectOfType<DayTimer>();
        _daySlider = FindObjectOfType<DaySlider>();
        GameManager.Instance.StartDay();
        if(GameManager.Instance.DayIndex == 0)
        {
            _daySlider.SetValue(1);

        }
        else
        {
            _daySlider.SetValue(GameManager.Instance.DaySliderOverDay + .3f);
        }
        _textDay.text = (GameManager.Instance.DayIndex +1).ToString();
    }




}

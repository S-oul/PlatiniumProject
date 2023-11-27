using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    DayTimer _dayTimer;
    DaySlider _daySlider;

    public DayTimer DayTimer { get => _dayTimer; set => _dayTimer = value; }
    public DaySlider DaySlider { get => _daySlider; set => _daySlider = value; }

    private void Start()
    {
        GameManager.Instance.DayManager = this;
        _dayTimer = FindObjectOfType<DayTimer>();
        _daySlider = FindObjectOfType<DaySlider>();
        GameManager.Instance.StartDay();
        print(_daySlider);
        _daySlider.SetValue(1);
    }




}

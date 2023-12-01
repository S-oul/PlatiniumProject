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
        print(_daySlider);
        _daySlider.SetValue(1);
        //_textDay.text = GameManager.Instance.DayIndex.ToString();
    }




}

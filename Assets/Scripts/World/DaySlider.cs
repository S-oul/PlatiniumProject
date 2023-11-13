using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaySlider : MonoBehaviour
{
    Slider _slider;
    float _unClampedValue = 1;
    [SerializeField] float _speedTimer = .3f;
    [Tooltip("In Percent")]
    [SerializeField] float _OnRoomLoose = .20f;
    [SerializeField] float _OnRoomWin = .10f;

    public float OnRoomLoose { get => _OnRoomLoose; set => _OnRoomLoose = value; }
    public float OnRoomWin { get => _OnRoomWin; set => _OnRoomWin = value; }

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }
    private void Update()
    {
        RemoveValue(_speedTimer * Time.deltaTime);

        _slider.value = Mathf.Lerp(_unClampedValue, _slider.value, Time.deltaTime);

    }
    public float SetValue(float val)
    {
        _unClampedValue = val;
        return _unClampedValue;
    }
    public float AddValue(float val)
    {
        _unClampedValue += val;
        return _unClampedValue;
    }
    public float RemoveValue(float val)
    {
        _unClampedValue -= val;
        return _unClampedValue;
    }

    public float GetValue()
    {
        return _unClampedValue;
    }
}

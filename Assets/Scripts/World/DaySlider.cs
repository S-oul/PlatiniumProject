using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Rendering;

public class DaySlider : MonoBehaviour
{
    Slider _slider;
    [SerializeField] float _unclampedValue = 1;
    [SerializeField] float _speedTimer = .3f;
    [SerializeField] float _speedUnclamped = .1f;
    [Tooltip("In Percent")]
    [SerializeField] float _OnRoomLoose = .20f;
    [SerializeField] float _OnRoomWin = .10f;


    private bool _isOnCrunch = false;

    public float OnRoomLoose { get => _OnRoomLoose; set => _OnRoomLoose = value; }
    public float OnRoomWin { get => _OnRoomWin; set => _OnRoomWin = value; }
    public bool IsOnCrunch { get => _isOnCrunch; set => _isOnCrunch = value; }

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }
    private void Update()
    {
        _unclampedValue = Mathf.Clamp01(_unclampedValue);
        RemoveValue(_speedTimer * Time.deltaTime);

        _slider.value = Mathf.Lerp( _slider.value, _unclampedValue, _speedUnclamped);

    }
    public float SetValue(float val)
    {
        if (!_isOnCrunch)
        {
            _unclampedValue = val;
        }
        return _unclampedValue;

    }
    public float AddValue(float val)
    {
        if (!_isOnCrunch)
        {
            _unclampedValue += val;
        }
        return _unclampedValue;
    }
    public float RemoveValue(float val)
    {
        if (!_isOnCrunch)
        {
            _unclampedValue -= val;
        }
        return _unclampedValue;
    }

    public float GetValue()
    {
        return _unclampedValue;
    }


    [Button]
    public void Add20()
    {
        AddValue(.2f);
    }
    [Button]
    public void Remove20()
    {
        RemoveValue(.2f);
    }
}

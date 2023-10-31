using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashDownButton : MonoBehaviour
{
    [SerializeField] bool _doSwap = false;
    [SerializeField] float _swapTime = .5f;
    [SerializeField] Image _mashImage;

    public bool DoSwap { get => _doSwap;}

    public bool ChangeSwap(bool value)
    {

        _doSwap = value;
        if (value) { StartCoroutine(SwapOn()); }
        return value;
    }
    IEnumerator SwapOn()
    {
        if(!_doSwap) yield return null;
        
        _mashImage.color = Color.white;
        yield return new WaitForSeconds(_swapTime);
        StartCoroutine(SwapOff());
    }
    IEnumerator SwapOff()
    {
        if (!_doSwap) yield return null;

        _mashImage.color = Color.grey;
        yield return new WaitForSeconds(_swapTime);
        StartCoroutine(SwapOn());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStateScreen : MonoBehaviour
{

    [SerializeField] Animator _anim;

    public enum WinScreenState
    {
        Idle,
        Success,
        Fail,
        Retry
    }

    private void Start()
    {
        ChangeValue(WinScreenState.Idle);
    }
    public void ChangeValue(WinScreenState state)
    {
        switch (state)
        {
            case WinScreenState.Idle:
                _anim.SetBool("isDone", false);
                break;
            case WinScreenState.Success:
                _anim.SetBool("isDone", true);
                _anim.SetTrigger("isSuccess");
                break;
            case WinScreenState.Fail:
                _anim.SetBool("isDone", true);
                _anim.SetTrigger("isFail");
                break;
            case WinScreenState.Retry:
                _anim.SetBool("isDone", true);
                _anim.SetTrigger("isRetry");
                break;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TamponageGameManager : MonoBehaviour
{
    [SerializeField] int _numOfClicks;
    [SerializeField] float _timeLimit;

    int _remainingClicks;
    float _remainingTime;

    enum BUTTON { NULL,LEFT_BUTTON, RIGHT_BUTTON }
    BUTTON _lastPressed = BUTTON.NULL;

    // Start is called before the first frame update
    void Start()
    {
        _remainingClicks = _numOfClicks;
        _remainingTime = _timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        _remainingTime -= Time.deltaTime;
        if (_remainingClicks < 0) { 
            Debug.Log("win");
            GetComponent<PlayerInput>().enabled = false;
            this.enabled = false; 
        }
        if (_remainingTime < 0) { 
            Debug.Log("game over");
            GetComponent<PlayerInput>().enabled = false;
            this.enabled = false; 
        }
    }

    public void InputManager(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (context.ReadValue<float>() > 0) { Press(BUTTON.LEFT_BUTTON); }
            if (context.ReadValue<float>() < 0) { Press(BUTTON.RIGHT_BUTTON); }
        }
    }

    private void Press(BUTTON pressedButton)
    {
        if (_lastPressed == pressedButton) { ResetCounter(); Debug.Log("WRONG!"); }
        else 
        { 
            _remainingClicks--;
            _lastPressed = pressedButton;
        }
    }

    private void ResetCounter()
    {
        _remainingClicks = _numOfClicks;
    }

    /* check if both players are in position
     * start timer 
     * inisilide the number of presse
     * update: if one player presses twise in a row, counter goes red and timer/starter restarts. 
     */
}

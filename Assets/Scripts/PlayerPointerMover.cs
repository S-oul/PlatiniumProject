using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPointerMover : MonoBehaviour
{    
    Transform _playerPointerTrans;
    Vector3 _playerStartPosition;

    float _playerStepDistanceInput = 0.69f;
    Vector3 _playerStepDistance;
    [SerializeField] [Range(0.01f, 0.5f)] float _playerSpeed = 0.1f;

    int _numberOfLayers = 6;

    enum PlayerPointerState
    {
        IDLE,
        MOVING_RIGHT,
        MOVING_LEFT,
        ANIMATING,
        DIEING,
    }
    PlayerPointerState _starterState = PlayerPointerState.IDLE;
    PlayerPointerState _currentState;
    PlayerPointerState _previousState;

    enum PlayerLayerPosition
    {
        START,
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        HOME,
    }
    PlayerLayerPosition _currentPlayerLayerPosition = PlayerLayerPosition.START;
    Dictionary<PlayerLayerPosition, Vector3> _positionDict;

    public void Awake()
    {   
        _playerPointerTrans = GetComponent<Transform>();
        _playerStartPosition = _playerPointerTrans.position;

        _currentState = _starterState;
        _previousState = _starterState;

        _playerStepDistance = new Vector3(_playerStepDistanceInput, 0, 0);
        _positionDict = CreatePositionDictionary();
    }

    public void InputManager(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0) { MovePlayerForward(); }
        if (context.ReadValue<float>() < 0) { MovePlayerBack(); }
    } //called by PlayerInput System

    private void MovePlayerForward() 
    {
        if (_currentState is not PlayerPointerState.IDLE) { return; }
        _currentState = PlayerPointerState.MOVING_RIGHT;
        /*
if (context.started)
    Debug.Log("Action was started");
else if (context.performed)
    Debug.Log("Action was performed");
else if (context.canceled)
    Debug.Log("Action was cancelled");
*/
    }   
    private void MovePlayerBack()
    {
        if (_currentState is not PlayerPointerState.IDLE) { return; }
        _currentState = PlayerPointerState.MOVING_LEFT;
        /*
if (context.started)
    Debug.Log("Action was started");
else if (context.performed)
    Debug.Log("Action was performed");
else if (context.canceled)
    Debug.Log("Action was cancelled");
*/
    }   

    public void FixedUpdate()
    {
        Vector3 newPos = _playerPointerTrans.position;
        switch (_currentState)
        {
            case PlayerPointerState.MOVING_RIGHT:
                if ((int)_currentPlayerLayerPosition +1 >= _numberOfLayers) { _currentState = PlayerPointerState.IDLE; break; }
                _currentPlayerLayerPosition++;
                _currentState = PlayerPointerState.ANIMATING;
                break;
            case PlayerPointerState.MOVING_LEFT:
                if ((int)_currentPlayerLayerPosition -1 < 0) { _currentState = PlayerPointerState.IDLE; break; }
                _currentPlayerLayerPosition--;
                _currentState = PlayerPointerState.ANIMATING;
                break;
            case PlayerPointerState.ANIMATING:
                if (_positionDict[_currentPlayerLayerPosition] == _playerPointerTrans.position) { _currentState = PlayerPointerState.IDLE; break; }
                AnimatePlayer(_positionDict[_currentPlayerLayerPosition]);
                break;
        }
    }

    public void AnimatePlayer(Vector3 targetPosition)
    {
        _playerPointerTrans.position = Vector3.MoveTowards(_playerPointerTrans.position, targetPosition, _playerSpeed);           
    }

    private Dictionary<PlayerLayerPosition, Vector3> CreatePositionDictionary()
    {
        Dictionary<PlayerLayerPosition, Vector3> dict = new Dictionary<PlayerLayerPosition, Vector3>();
        Vector3 vec = _playerStartPosition;
        for (int i=0; i <_numberOfLayers ; i++)
        {
            dict.Add((PlayerLayerPosition)i, vec);
            vec += _playerStepDistance;
        }
        return dict;
    }
}



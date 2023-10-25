using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPointerMover : MonoBehaviour
{
    #region Declarations
    Transform _playerPointerTrans;
    Vector3 _playerStartPosition;

    float _playerStepDistanceInput = 0.45f;
    Vector3 _playerStepDistance;
    [SerializeField] [Range(0.01f, 0.5f)] float _playerSpeed = 0.1f;

    [SerializeField] float _deathDuriation = 0.5f;
    float _deathCountDown;

    readonly int _numberOfLayers = 6;

    enum PlayerPointerState
    {
        IDLE,
        MOVING_RIGHT,
        MOVING_LEFT,
        ANIMATING,
        HIT,
        DIEING,
    }
    PlayerPointerState _starterState = PlayerPointerState.IDLE;
    PlayerPointerState _currentState;

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
    Dictionary<PlayerLayerPosition, Vector3> 
        _positionDict;

    Renderer _ppVisualsSprite;
    Renderer _ppDeathVisualsSprite;
    #endregion

    public void Awake()
    {
        _playerPointerTrans = GetComponent<Transform>();
        _playerStartPosition = _playerPointerTrans.localPosition;

        SetState(_starterState);

        _playerStepDistance = new Vector3(_playerStepDistanceInput, 0, 0);
        _positionDict = CreatePositionDictionary();

        _ppVisualsSprite = GameObject.Find("PPVisuals").GetComponent<Renderer>();
        _ppDeathVisualsSprite = GameObject.Find("PPDeathVisual").GetComponent<Renderer>();
    }

    public void InputManager(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0) { MovePlayerForward(); }
        if (context.ReadValue<float>() < 0) { MovePlayerBack(); }
        return;
    } //called by PlayerInput System

    private void MovePlayerForward() 
    {
        if (_currentState is not PlayerPointerState.IDLE) { return; }
        SetState(PlayerPointerState.MOVING_RIGHT);
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
        SetState(PlayerPointerState.MOVING_LEFT);
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
        {
            switch (_currentState)
            {
                case PlayerPointerState.HIT:
                    _ppVisualsSprite.enabled = false;
                    _ppDeathVisualsSprite.enabled = true;
                    _deathCountDown = _deathDuriation;
                    SetState(PlayerPointerState.DIEING);
                    break; 
                case PlayerPointerState.MOVING_RIGHT:
                    if ((int)_currentPlayerLayerPosition + 1 >= _numberOfLayers) { SetState(PlayerPointerState.IDLE); break; }
                    _currentPlayerLayerPosition++;
                    SetState(PlayerPointerState.ANIMATING);
                    break;
                case PlayerPointerState.MOVING_LEFT:
                    if ((int)_currentPlayerLayerPosition - 1 < 0) { SetState(PlayerPointerState.IDLE); break; }
                    _currentPlayerLayerPosition--;
                    SetState(PlayerPointerState.ANIMATING);
                    break;
                case PlayerPointerState.ANIMATING:
                    if (_positionDict[_currentPlayerLayerPosition] == _playerPointerTrans.position) { SetState(PlayerPointerState.IDLE); break;}
                    AnimatePlayer(_positionDict[_currentPlayerLayerPosition]);
                    break;
                case PlayerPointerState.DIEING:
                    if (_deathCountDown > 0) { _deathCountDown -= Time.deltaTime; break; }
                    _ppVisualsSprite.enabled = true;
                    _ppDeathVisualsSprite.enabled = false;
                    _currentPlayerLayerPosition = PlayerLayerPosition.START;
                    _playerPointerTrans.position = _positionDict[_currentPlayerLayerPosition];
                    SetState(PlayerPointerState.IDLE);
                    break; 
            }
        }
    }

    private void SetState(PlayerPointerState newState)
    {
        if (_currentState != newState)
        {
            _currentState = newState;
        }
    }

    private void AnimatePlayer(Vector3 targetPosition)
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

    public void killPlayerPointer()
    {
        if (_currentState == PlayerPointerState.DIEING ||  _currentState == PlayerPointerState.HIT || _currentPlayerLayerPosition == PlayerLayerPosition.HOME) { return; }
        SetState(PlayerPointerState.HIT);
    }
}



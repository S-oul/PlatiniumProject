using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class MatrixTask : InputTask
{
    List<Transform> _posPlayers = new List<Transform>();
    MatrixBoss _teleBoss;

    [Header("Game Settings")]
    [SerializeField] float _timeBeforeTask;
    int _phase;
    List<List<Inputs>> _inputsToDo = new List<List<Inputs>> ();
    bool _inputHasBeenPressed;
    List<GameObject> _playersInOrder = new List<GameObject> ();
    Cam _cam;
    Color _colorScreen;
    int _currentInputID;
    GameObject _rightPlayer;
    Color _currentColor;
    List<Color> colors = new List<Color> ();
    List<Inputs> _inputsList = new List<Inputs> ();
    List<Coroutine> _coroutinesRunning = new List<Coroutine> ();

    private void Start()
    {
        
    }
    public override void StartTask()
    {
        _teleBoss = RoomTask.gameObject.transform.Find("TeleBoss").GetComponent<MatrixBoss>();
        _teleBoss.SetActiveInput(false);
        _cam = Camera.main.GetComponent<Cam>();
        _cam.FixOnRoomVoid(RoomTask);
        _phase = 1;
        _inputHasBeenPressed = false;
        StartCoroutine(WaitForStart());

    }   
    public override void End(bool isSuccessful)
    {

    }

    void ShufflePlayerOrder()
    {
        _playersInOrder.Clear();
        List<GameObject> playersTemp = new List<GameObject> ();
        foreach (GameObject player in PlayersDoingTask)
        {
            playersTemp.Add(player);
        }
        for (int i = 0; i < playersTemp.Count; i++)
        {
            _playersInOrder.Add(playersTemp[Random.Range(0, playersTemp.Count)]);
        }
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(_timeBeforeTask);
        DisplayAllInputs();
    }

    void DisplayAllInputs()
    {
        
        ShufflePlayerOrder();
        _inputsToDo = CreateListInputs();

        StartCoroutine(DisplayInputs(_inputsToDo));

    }


    IEnumerator DisplayInputs(List<List<Inputs>> list)
    {
        yield return new WaitForSeconds(3); // => Message "Watch!"
        _currentInputID = 0;
        colors = SetColorList();
        _teleBoss.SetActiveInput(true);
        switch (_phase)
        {
            case 1:
                
                for (int i = 0; i < _playersInOrder.Count; i++)
                {

                    _colorScreen = colors[i];
                    _teleBoss.AttackAnimation();
                    foreach (Inputs input in list[i])
                    {
                        _inputsList.Add(input);
                        _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));                        
                    }
                    yield return new WaitForSeconds(0.5f);
                    _teleBoss.ClearInput();
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 2:
                for (int i = 0; i < _playersInOrder.Count; i++)
                {
                    _colorScreen = colors[i];
                    _teleBoss.AttackAnimation();
                    foreach (Inputs input in list[i])
                    {
                        _inputsList.Add(input);
                        _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));
                        yield return new WaitForSeconds(0.5f);
                        _teleBoss.ClearInput();
                        yield return new WaitForSeconds(0.05f);
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                break;
            case 3:
                colors = GetRandomColor(colors, _playersInOrder.Count * list[0].Count);
                for (int i = 0; i < _playersInOrder.Count * list[i].Count; i++)
                {
                    _colorScreen = colors[i];
                    _teleBoss.AttackAnimation();
                    Inputs input = _inputsToDo[Random.Range(0, _inputsToDo.Count)][Random.Range(0, list[i].Count)];
                    _inputsList.Add(input);
                    _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));

                    yield return new WaitForSeconds(0.3f);
                    _teleBoss.ClearInput();
                    yield return new WaitForSeconds(0.05f);
                }
                break;
        }
        _teleBoss.SetActiveInput(false);

        StartCoroutine(TimeBeforeInputCheck()); // => Message "Ready? 3-2-1"
    }

    List<Color> GetRandomColor(List<Color> colors, int iteration)
    {
        List<Color> randomColor = new List<Color>();
        for (int i = 0; i < iteration; i++)
        {
            Color tempColor = colors[Random.Range(0, colors.Count)];
            randomColor.Add(tempColor);
        }
        
        return randomColor;
    }

    IEnumerator TimeBeforeInputCheck()
    {
        yield return new WaitForSeconds(4);
        foreach (GameObject player in PlayersDoingTask)
        {
            _coroutinesRunning.Add(StartCoroutine(WaitToPressInput(player, 2f, _inputsList[0])));
        }
    }

    List<Color> SetColorList()
    {
        List<Color> colors = new List<Color>();
        foreach(GameObject player in PlayersDoingTask)
        {
            colors.Add(player.GetComponent<PlayerController>().ColorPlayer);
        }
        return colors;
    }

    IEnumerator WaitToPressInput(GameObject player, float timeBetweenInputs, Inputs _currentInput)
    {
        PlayerController _controller = player.GetComponent<PlayerController>();
        PlayerUI _playerUI = player.GetComponent<PlayerUI>();
        float _timeToPressInput = timeBetweenInputs;
        while (CheckInputValue(_controller.currentContextName, InputsToString[_currentInput], _controller) == PlayerInputValue.None && _timeToPressInput > 0)
        {
            _timeToPressInput -= Time.deltaTime;
            yield return null; //=> Inportant => Inbecile

        }
        if (_timeToPressInput <= 0)
        {
            InputValue(false, player);
        }

        else if (_inputValue == PlayerInputValue.WrongValue)
        {
            _inputHasBeenPressed = true;
            InputValue(false, player);

        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            _inputHasBeenPressed = true;
            InputValue(true, player);

        }

    }
    void InputValue(bool isInputRight, GameObject player)
    {
        foreach(GameObject playerObject in PlayersDoingTask)
        {
            playerObject.GetComponent<PlayerController>().currentContextName = "";
        }
        
        if (player == PlayerManager.Instance.FindPlayerFromColor(_currentColor))
        {
            if (isInputRight)
            {
                _currentInputID++;
                if (_currentInputID == colors.Count - 1)
                {
                    //_playerUI.ChangeUIInputsValidation(_index, Color.green);
                    //EndQTE(true);
                    return;
                }
                //Stack overflow because it goes here directly
                else
                {
                    //Display Input fait une overflow
                    // _playerUI.ChangeUIInputsValidation(_index, Color.green);
                    _coroutinesRunning.Clear();
                    foreach (GameObject playerObject in PlayersDoingTask)
                    {
                        _coroutinesRunning.Add(StartCoroutine(WaitToPressInput(playerObject, 2f, _inputsList[_currentInputID])));
                    }
                    return;
                }
            }
            else
            {
                /*_numberOfFails++;
                FeedBackBadInputs();
                if (_numberOfFails == 3)
                {
                    // _playerUI.ChangeUIInputsValidation(_index, Color.red);
                    EndQTE(false);
                    PushBack();
                    return;
                }
                else
                {
                    //_playerUI.ChangeUIInputsValidation(_index, Color.red);

                    //Start Task fait une overflow
                    StartTaskQTE();
                    return;
                }*/

            }
        }
        
        
        

    }


    List<List<Inputs>> CreateListInputs()
    {
        List<List<Inputs>> inputs = new List<List<Inputs>>();
        for (int i = 0; i < PlayersDoingTask.Count; i++)
        {
            List<Inputs> inputsPlayer = new List<Inputs>();
            for (int j = 0; j < _phase; j++)
            {
                inputsPlayer.Add((Inputs)Random.Range(0, 4));
            }
            inputs.Add(inputsPlayer);
        }
        return inputs;
    }

}


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
    List<Transform> _positionsPlayer = new List<Transform>();
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

    Dictionary<int, Color> inputsPlayer = new Dictionary<int, Color> ();

    bool _canChekInput = false;
    private void Start()
    {
        
    }
    public override void StartTask()
    {
        foreach(Transform pos in RoomTask.gameObject.transform.Find("PlayerPositions"))
        {
            _posPlayers.Add(pos);
        }
        print(_posPlayers.Count);
        _teleBoss = RoomTask.gameObject.transform.Find("TeleBoss").GetComponent<MatrixBoss>();
        _teleBoss.SetActiveInput(false);
        _cam = Camera.main.GetComponent<Cam>();
        _cam.FixOnRoomVoid(RoomTask);
        _phase = 1;
        _currentInputID = 0;
        _inputHasBeenPressed = false;
        foreach(GameObject player in PlayersDoingTask)
        {
            player.GetComponent<PlayerController>().DisableAllInputs();
        }
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
        int iteration = playersTemp.Count;
        for (int i = 0; i < iteration; i++)
        {
            GameObject _tempPlayer = playersTemp[Random.Range(0, playersTemp.Count)];
            _playersInOrder.Add(_tempPlayer);
            playersTemp.Remove(_tempPlayer);

        }
        
        for (int j = 0; j < _playersInOrder.Count; j++)
        {

            _playersInOrder[j].transform.position = _posPlayers[j].transform.position; 

        }
    }

    void EnableInput(bool value, GameObject player)
    {
        if(value)
        {
            player.GetComponent<PlayerController>().DisableMovementEnableInputs();
        }
        else
        {
            player.GetComponent<PlayerController>().DisableAllInputs();
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
        _teleBoss.SliderActive(false);
        _teleBoss.DisplayText("Watch!");
        int IDInput = 0;
        yield return new WaitForSeconds(3); // => Message "Watch!"
        inputsPlayer.Clear();
        _teleBoss.DisplayText("");
        colors = SetColorList();
        _teleBoss.SetActiveInput(true);
        _currentInputID = 0;
        switch (_phase)
        {
            case 1:
                
                for (int i = 0; i < _playersInOrder.Count; i++)
                {

                    _colorScreen = colors[i];
                    _teleBoss.AttackAnimation();
                    foreach (Inputs input in list[i])
                    {
                        
                        inputsPlayer.Add(IDInput, _colorScreen);
                        IDInput++;
                        _teleBoss.DisplayColorInput(_colorScreen);
                        _inputsList.Add(input);
                        _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));                        
                    }
                    yield return new WaitForSeconds(0.6f);
                    _teleBoss.ClearInput();
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case 2:
                for (int i = 0; i < _playersInOrder.Count; i++)
                {
                    print(_playersInOrder[i].GetComponent<PlayerController>().ColorPlayer);
                    _colorScreen = colors[i];
                    _teleBoss.AttackAnimation();
                    foreach (Inputs input in list[i])
                    {
                        
                        inputsPlayer.Add(IDInput, _colorScreen);
                        IDInput++;
                        _inputsList.Add(input);
                        
                        _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));
                        _teleBoss.DisplayColorInput(_colorScreen);
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
                    int randomID = Random.Range(0, _inputsToDo.Count);

                    Inputs input = _inputsToDo[randomID][Random.Range(0, list[i].Count)];
                    _inputsToDo[randomID].Remove(input);
                    _inputsList.Add(input);
                    inputsPlayer.Add(IDInput, _colorScreen);
                    IDInput++;
                    _teleBoss.DisplayColorInput(_colorScreen);
                    _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));

                    yield return new WaitForSeconds(0.4f);
                    _teleBoss.ClearInput();
                    yield return new WaitForSeconds(0.05f);
                }
                break;
        }
        _teleBoss.SetActiveInput(false);
        _canChekInput = true;
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
        float time = 4;
        _teleBoss.DisplayText("Ready?");
        yield return new WaitForSeconds(3);
        while (time > 0)
        {
            _teleBoss.DisplayText("" + (int)time);
            time -= Time.deltaTime;
            yield return null;
        }
        _teleBoss.DisplayText("");

        foreach (GameObject player in PlayersDoingTask)
        {
            _coroutinesRunning.Add(StartCoroutine(WaitToPressInput(player, 2f, _inputsList[0])));
        }

    }

    List<Color> SetColorList()
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < _playersInOrder.Count; i++)
        {
            colors.Add(_playersInOrder[i].GetComponent<PlayerController>().ColorPlayer);
        }
        
        return colors;
    }

    IEnumerator WaitToPressInput(GameObject player, float timeBetweenInputs, Inputs _currentInput)
    {
        EnableInput(true, player);

        PlayerController _controller = player.GetComponent<PlayerController>();
        _currentColor = colors[_currentInputID];
        _teleBoss.SliderActive(true);
        PlayerUI _playerUI = player.GetComponent<PlayerUI>();
        float _timeToPressInput = timeBetweenInputs;
        while (CheckInputValue(_controller.currentContextName, InputsToString[_currentInput], _controller) == PlayerInputValue.None && _timeToPressInput > 0)
        {
            
            if (_canChekInput)
            {
                _timeToPressInput -= Time.deltaTime;
                _teleBoss.SliderValue(Mathf.InverseLerp(0, timeBetweenInputs, _timeToPressInput));
            }
               
            yield return null; //=> Inportant => Inbecile

        }
        
        if (_canChekInput)
        {
            if (_timeToPressInput <= 0)
            {
                _canChekInput = false;
                InputValue(false, player);

            }

            else if (_inputValue == PlayerInputValue.WrongValue)
            {
                _canChekInput = false;
                _inputHasBeenPressed = true;
                InputValue(false, player);

            }
            else if (_inputValue == PlayerInputValue.RightValue)
            {
                _canChekInput = false;
                _inputHasBeenPressed = true;
                InputValue(true, player);

            }
        }
       

    }
    void InputValue(bool isInputRight, GameObject player)
    {
        
        _teleBoss.SliderActive(false);
        _teleBoss.SliderValue(0);
        _currentColor = inputsPlayer[_currentInputID];
        _coroutinesRunning.Clear();
       
        foreach (GameObject playerObject in PlayersDoingTask)
        {
            
            playerObject.GetComponent<PlayerController>().currentContextName = "";
            EnableInput(false, playerObject);
            
        }
       
        if (player == PlayerManager.Instance.FindPlayerFromColor(_currentColor))
        {
            if (isInputRight)
            {
                _currentInputID++;
                if (_currentInputID == colors.Count )
                {
                    print("QTE suite réussie");
                    _teleBoss.HitAnimation();
                    _phase++;
                    
                    DisplayAllInputs();

                    //_playerUI.ChangeUIInputsValidation(_index, Color.green);
                    //EndQTE(true);
                    return;
                }
                //Stack overflow because it goes here directly
                else
                {
                    //Display Input fait une overflow
                    // _playerUI.ChangeUIInputsValidation(_index, Color.green);
                    print("QTE solo réussi");
                    foreach (GameObject playerObject in PlayersDoingTask)
                    {
                        Coroutine newCouroutine = StartCoroutine(WaitToPressInput(playerObject, 2f, _inputsList[_currentInputID]));
                        _coroutinesRunning.Add(newCouroutine);
                    }
                    _canChekInput = true;
                    
                    
                    return;
                }
            }
            else
            {
                print("QTE solo fail");
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


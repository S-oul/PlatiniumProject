using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


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
    [SerializeField] int tries;
    Dictionary<int, Color> inputsPlayer = new Dictionary<int, Color> ();

    bool _canCheckInput = false;
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
        _teleBoss.SliderActive(false);
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
        base.End(isSuccessful);
        GameManager.Instance.DayIndex++;
        List<GameObject> l = new List<GameObject>();
        l = GameManager.Instance.Players;
        SceneManager.LoadScene(2);
        GameManager.Instance.Players = l;
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
        _inputsToDo.Clear();
        _inputsToDo = CreateListInputs();

        StartCoroutine(DisplayInputs(_inputsToDo));

    }


    IEnumerator DisplayInputs(List<List<Inputs>> list)
    {
        _teleBoss.SliderActive(false);
        _teleBoss.DisplayText("Watch carefully!");
        AudioManager.Instance.PlaySFXOS("MatrixLaugh", _teleBoss.gameObject.GetComponent<AudioSource>());
        int IDInput = 0;
        yield return new WaitForSeconds(3); // => Message "Watch!"
        inputsPlayer.Clear();
        _inputsList.Clear();
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
                        _teleBoss.ChangeScreensColor(_colorScreen);
                    }
                    yield return new WaitForSeconds(0.8f);
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
                        
                        inputsPlayer.Add(IDInput, _colorScreen);
                        IDInput++;
                        _teleBoss.DisplayColorInput(_colorScreen);
                        _teleBoss.ChangeScreensColor(_colorScreen);
                        _inputsList.Add(input);
                        _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[i].GetComponent<PlayerController>().Type));
                        
                        yield return new WaitForSeconds(0.65f);
                        _teleBoss.ClearInput();
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                break;
            case 3:
                
                int numberInterations = _playersInOrder.Count * list[0].Count;
                colors = GetRandomColor(numberInterations);
                foreach(Color color in colors)
                {
                    print(color);
                }
                for (int i = 0; i < numberInterations; i++)
                {
                    
                    _colorScreen = colors[i];
                    GameObject _currentPlayer = PlayerManager.Instance.FindPlayerFromColor(_colorScreen);
                    int IDPlayer = 0;
                    for (int j = 0; j < _playersInOrder.Count; j++)
                    {
                        if(_playersInOrder[j] == _currentPlayer)
                        {
                            IDPlayer = j;
                        }
                    }
                    _teleBoss.AttackAnimation();
                    int randomID = Random.Range(0, list.Count);
                    int randomInputID = Random.Range(0, list[randomID].Count);
                    Inputs input = list[randomID][randomInputID];
                    list[randomID].Remove(input);
                    if (list[randomID].Count == 0)
                    {
                        list.Remove(list[randomID]);
                    }
                    /*_inputsToDo[randomID].Remove(input);*/
                    
                    _inputsList.Add(input);
                    inputsPlayer.Add(i, _colorScreen);
                    _teleBoss.ChangeScreensColor(_colorScreen);
                    IDInput++;
                    _teleBoss.DisplayColorInput(_colorScreen);
                    _teleBoss.DisplayInput(DataManager.Instance.FindInputSprite(InputsToString[input], _playersInOrder[IDPlayer].GetComponent<PlayerController>().Type));

                    yield return new WaitForSeconds(0.5f);
                    _teleBoss.ClearInput();
                    yield return new WaitForSeconds(0.05f);
                }
                break;
        }

        _teleBoss.SetActiveInput(false);
        _teleBoss.ChangeScreensColor(Color.black);
        _canCheckInput = true;
        StartCoroutine(TimeBeforeInputCheck()); // => Message "Ready? 3-2-1"
    }

    List<Color> GetRandomColor(int iteration)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < _playersInOrder.Count; i++)
        {
            for (int j = 0; j < _phase; j++)
            {
                colors.Add(_playersInOrder[i].GetComponent<PlayerController>().ColorPlayer);
            }
            
        }

        List<Color> randomColor = new List<Color>();
        for (int i = 0; i < iteration; i++)
        {
           
            Color tempColor = colors[Random.Range(0, colors.Count)];
            randomColor.Add(tempColor);
            colors.Remove(tempColor);
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
        bool _isTheRightPlayer = false;
        if (player == PlayerManager.Instance.FindPlayerFromColor(inputsPlayer[_currentInputID]))
        {
            _isTheRightPlayer = true;
            print(player.GetComponent<PlayerController>().Name);
        }
        
        PlayerController _controller = player.GetComponent<PlayerController>();
        
        _teleBoss.SliderActive(true);
        PlayerUI _playerUI = player.GetComponent<PlayerUI>();
        float _timeToPressInput = timeBetweenInputs;
        int actualInputID = _currentInputID;
        while (_canCheckInput && CheckInputValue(_controller.currentContextName, InputsToString[_inputsList[_currentInputID]], _controller) == PlayerInputValue.None && _timeToPressInput > 0)
        {
            _timeToPressInput -= Time.deltaTime;
            if (_isTheRightPlayer)
            {
                print(actualInputID);
                _teleBoss.SliderValue(Mathf.InverseLerp(0, timeBetweenInputs, _timeToPressInput));
            }
            yield return null; 

        }
        
        
        if (_canCheckInput)
        {
            if (_timeToPressInput <= 0)
            {
                print("Time's up : " + player.GetComponent<PlayerController>().Name + " " + actualInputID);
                InputValue(false, player);

            }

            else if (_inputValue == PlayerInputValue.WrongValue)
            {
                _inputHasBeenPressed = true;
                print("Wrong Input");
                InputValue(false, player);

            }
            else if (_inputValue == PlayerInputValue.RightValue)
            {
                
                _inputHasBeenPressed = true;

                InputValue(true, player);

            }
        }

       

    }
    void InputValue(bool isInputRight, GameObject player)
    {
        _canCheckInput = false;
        _teleBoss.SliderActive(false);
        _teleBoss.SliderValue(0);
        _currentColor = inputsPlayer[_currentInputID];
        _coroutinesRunning.Clear();
       
        foreach (GameObject playerObject in PlayersDoingTask)
        {
            
            playerObject.GetComponent<PlayerController>().currentContextName = "";
            EnableInput(false, playerObject);


        }
        StopAllCoroutines();            
        
        if (player == PlayerManager.Instance.FindPlayerFromColor(_currentColor))
        {
            if (isInputRight)
            {
                _currentInputID++;
                if (_currentInputID == _inputsList.Count )
                {
                    print("QTE suite réussie");
                    _teleBoss.HitAnimation();
                    AudioManager.Instance.PlaySFXOS("MatrixPain", _teleBoss.gameObject.GetComponent<AudioSource>());
                    _phase++;
                    if(_phase == 4)
                    {
                        End(true);
                    }
                    else
                    {
                        DisplayAllInputs();
                    }
                    

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
                    _canCheckInput = true;
                    foreach (GameObject playerObject in PlayersDoingTask)
                    {
                        
                        Coroutine newCouroutine = StartCoroutine(WaitToPressInput(playerObject, 2f, _inputsList[_currentInputID]));
                        _coroutinesRunning.Add(newCouroutine);
                    }
                    
                    
                    
                    return;
                }
            }
            else
            {
                if(tries > 0)
                {
                    tries--;
                    if (tries > 0)
                    {
                        StartCoroutine(DialoguesPlayerLoss());
                    }
                    else
                    {
                        End(false);
                    }
                }
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
        else
        {
            if (tries > 0)
            {
                tries--;
                if(tries > 0)
                {
                    StartCoroutine(DialoguesPlayerLoss());
                }
                else
                {
                    End(false);
                }
                
            }
        }
        
        

    }

    IEnumerator DialoguesPlayerLoss()
    {
        _teleBoss.DisplayText("You failed?");
        yield return new WaitForSeconds(2.5f);

        _teleBoss.DisplayText("Retry.");
        yield return new WaitForSeconds(2.5f);
        _teleBoss.DisplayText("");
        DisplayAllInputs();
    }


    List<List<Inputs>> CreateListInputs()
    {
        List<List<Inputs>> inputs = new List<List<Inputs>>();
        for (int i = 0; i < _playersInOrder.Count; i++)
        {
            List<Inputs> inputsPlayer = new List<Inputs>();
            for (int j = 0; j < _phase; j++)
            {
                Inputs newInput = (Inputs)Random.Range(0, 4);
                inputsPlayer.Add(newInput);
            }
            
            inputs.Add(inputsPlayer);
        }
        
        return inputs;
    }

}


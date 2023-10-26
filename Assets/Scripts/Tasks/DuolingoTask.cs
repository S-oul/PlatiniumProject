using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static CowboyQTE;

public class DuolingoTask : InputTask
{
    NPC _npcDuolingo;
    [SerializeField] List<string> _words = new List<string>();
    [SerializeField] List<string> _wordsTranslated = new List<string>();
    Dictionary<string, string> _rightAnswers = new Dictionary<string, string>();

    string _contextName;
    PlayerController _controller;
    string _rightWord = "";

    [SerializeField] int _numberOfWordsAsked;

    Dictionary<string, string> _wordToKey = new Dictionary<string, string>();

    List<string> _inputsName = new List<string>() {  "Y", "X", "B" };

    GameObject _currentPlayer;
    GameObject _otherPlayer;

    string _rightInputName;
    bool _canPressInput = false;

    int _rightAnswerIndex = 0;
    public NPC NPCDuolingo { get => _npcDuolingo; set => _npcDuolingo = value; }
    private void Start()
    {
        foreach(var word in _words)
        {
            _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(false);
            int wordIndex = _words.LastIndexOf(word);
            _rightAnswers.Add(word, _wordsTranslated[wordIndex]);
        }
    }

    
    public override void StartTask()
    {
        
        List<GameObject> players = PlayersDoingTask;
        _currentPlayer = players[0];
        _otherPlayer = players[1];
        TaskLoop();
    }


    void TaskLoop()
    {
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(true);
        _controller = _currentPlayer.GetComponent<PlayerController>();
        DisplayWordToFind();
    }

    void DisplayWordToFind()
    {
        _rightWord = _words[Random.Range(0, _words.Count)];
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(true);
        _npcDuolingo.GetComponent<UINpc>().ChangeBubbleText(_rightWord + "?");
        DisplayAnswers(_rightWord);
    }

    void DisplayAnswers(string word)
    {
        if (word == null) { return; }
        List<string> allWords = new List<string>();
        foreach (string _word in _wordsTranslated)
        {
            allWords.Add(_word);
        }
        List<string> answers = new List<string>();
        string rightAnswer = _rightAnswers[word];
        allWords.Remove(rightAnswer);
        Debug.Log(rightAnswer);
        for (int i = 0; i < 3; i++)
        {
            string tempWord = allWords[Random.Range(0, allWords.Count)];
            allWords.Remove(tempWord);
            Debug.Log(tempWord);
            answers.Add(tempWord);
        }

        answers[Random.Range(0, answers.Count)] = _rightAnswers[word];
        _currentPlayer.GetComponent<PlayerUI>().DisplayDuolingoUI(true);
        _currentPlayer.GetComponent<PlayerUI>().DisplayAnswersDuolingo(answers, _inputsName);
        /*ShuffleAnswers(answers);*/
        InputAssignement(answers);
        _canPressInput = true;
    }
    
    /*void ShuffleAnswers(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            string temp = words[i];
            int randomIndex = (int)Random.Range(0, words.Count);
            words[i] = words[randomIndex];
            words[randomIndex] = temp;
            
        }
        
    }*/

    

    void InputAssignement(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            _wordToKey.Add(words[i], _inputsName[i]);
            
        }
        _rightInputName = _wordToKey[_rightAnswers[_rightWord]];
    }

    void PressInputCheck()
    {
        if (_inputValue == PlayerInputValue.WrongValue)
        {
            _canPressInput = false;
            _contextName = "";
            Debug.Log("Wrong");
            EndDuolingo(false); 
        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            _canPressInput = false;
            Debug.Log("Right");
            _rightAnswerIndex++;
            CheckIfReplay();
        }
    }

    void CheckIfReplay()
    {
        if(_rightAnswerIndex < _numberOfWordsAsked)
        {
            _wordToKey.Clear();
            _currentPlayer.GetComponent<PlayerUI>().DisplayDuolingoUI(false);
            SwitchPlayer();
            TaskLoop();
        }
        else if(_rightAnswerIndex == _numberOfWordsAsked)
        {
            _currentPlayer.GetComponent<PlayerUI>().DisplayDuolingoUI(false);
            EndDuolingo(true);
        }
    }

    private void Update()
    {
        if (_canPressInput)
        {
            _inputValue = CheckInputValue(_contextName, _wordToKey[_rightAnswers[_rightWord]], _controller);
            PressInputCheck();
        }
       
    }

    void SwitchPlayer()
    {
        GameObject tempPlayer = _otherPlayer;
        _otherPlayer = _currentPlayer;
        _currentPlayer = tempPlayer;
        
    }

    void EndDuolingo(bool value)
    {
        foreach (GameObject player in PlayersDoingTask)
        {
            player.transform.position = gameObject.transform.parent.parent.Find("PlayerRespawnPoint").position;
            player.GetComponent<PlayerController>().BlockPlayer(false);
            player.GetComponent<SpriteRenderer>().sortingOrder = 5;
            player.GetComponent<PlayerController>().EnableMovementDisableInputs();
            if(value == false)
            {
                player.GetComponent<PlayerController>().DownPlayer();
                player.GetComponent<PlayerUI>().ClearAnswersDuolingo();
                player.GetComponent<PlayerUI>().DisplayDuolingoUI(false);
                StartCoroutine(RecuperatePlayer(player));
            }
        }

        _npcDuolingo.GetComponent<UINpc>().ChangeBubbleText("");
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(false);
        End(value);
    }

    IEnumerator RecuperatePlayer(GameObject player)
    {
        yield return new WaitForSeconds(5);
        player.GetComponent<PlayerController>().EnableMovementDisableInputs();

    }
}

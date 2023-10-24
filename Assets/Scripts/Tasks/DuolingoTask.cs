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

    [SerializeField] List<string> answers;

    GameObject _currentPlayer;
    GameObject _otherPlayer;

    string _rightInputName;

    public NPC NPCDuolingo { get => _npcDuolingo; set => _npcDuolingo = value; }
    private void Start()
    {
        foreach(var word in _words)
        {
            int wordIndex = _words.LastIndexOf(word);
            _rightAnswers.Add(word, _wordsTranslated[wordIndex]);
        }
    }
    public override void StartTask()
    {
        List<GameObject> players = PlayersDoingTask;
        _currentPlayer = PlayersDoingTask[Random.Range(0, 2)];
        players.Remove(_currentPlayer);
        _otherPlayer = players[0];
        _controller = _currentPlayer.GetComponent<PlayerController>();
        TaskLoop();
    }


    void TaskLoop()
    {
        DisplayWordToFind();
    }

    void DisplayWordToFind()
    {
        _rightWord = _words[Random.Range(0, _words.Count)];
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(true);
        _npcDuolingo.GetComponent<UINpc>().ChangeBubbleContent(_rightWord + "?");
        DisplayAnswers(_rightWord);
    }

    void DisplayAnswers(string word)
    {
        if (word == null) { return; }
        /*List<string> */answers = new List<string>();
        string rightAnswer = _rightAnswers[word];

        for (int i = 0; i < 3; i++)
        {
            string tempWord = _wordsTranslated[Random.Range(0, _wordsTranslated.Count)];
            
            while (answers.Contains(tempWord) && tempWord == rightAnswer)
            {
                tempWord = _wordsTranslated[Random.Range(0, _wordsTranslated.Count)];
            }
            answers.Add(tempWord);
        }

        answers[Random.Range(0, answers.Count)] = _rightAnswers[word];
/*        ShuffleAnswers(answers);*/
        InputAssignement(answers);
        PressInputCheck();
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
            Debug.Log(words[i] + ": " + _inputsName[i]);
        }
        _rightInputName = _wordToKey[_rightAnswers[_rightWord]];
    }

    void PressInputCheck()
    {
        //GetComponent toutes les frames
        while(CheckInputValue(_contextName, _wordToKey[_rightAnswers[_rightWord]], _controller) == PlayerInputValue.None)
        {
            
        }
        if (_inputValue == PlayerInputValue.WrongValue)
        {

            Debug.Log("Caca nonono");
        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {

            Debug.Log("Caca Miam Miam :D");
        }
    }

    void AskTranslate()
    {
        
    }
}

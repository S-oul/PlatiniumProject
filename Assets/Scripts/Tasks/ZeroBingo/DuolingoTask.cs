using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static CowboyQTE;

public class DuolingoTask : InputTask
{
    [Header("Duolingo")]
    [SerializeField] ZeroBingoData _wordsData;

    NPC _npcDuolingo;
    [SerializeField] List<string> _words = new List<string>();
    [SerializeField] List<string> _wordsTranslated = new List<string>();
    Dictionary<string, string> _rightAnswers = new Dictionary<string, string>();

    string _contextName;
    PlayerController _controller;
    /*string _rightWord = "";*/


    [SerializeField] int _numberOfWordsAsked;

    Dictionary<WordConfig, string> _wordToKey = new Dictionary<WordConfig, string>();
    Dictionary<string, WordConfig> _keyToWord = new Dictionary<string, WordConfig>();

    List<string> _inputsName = new List<string>() {  "Y", "X", "B" };

    GameObject _currentPlayer;
    GameObject _otherPlayer;

    string _rightInputName;
    WordConfig _currentGuessWord;
    bool _canPressInput = false;

    int _rightAnswerIndex = 0;
    public NPC NPCDuolingo { get => _npcDuolingo; set => _npcDuolingo = value; }



    List<WordConfig> _allWords = new List<WordConfig>();

    WordConfig _rightWord;

    
    public override void StartTask()
    {
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(false);
        _wordToKey.Clear();
        _keyToWord.Clear();
        List<GameObject> players = PlayersDoingTask;
        _currentPlayer = players[0];
        _otherPlayer = players[1];
        _rightAnswerIndex = 0;
        TaskLoop();
        /*_rightAnswers.Clear();
        _wordToKey.Clear();
        
        foreach (var word in _words)
        {
            _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(false);
            int wordIndex = _words.LastIndexOf(word);
            _rightAnswers.Add(word, _wordsTranslated[wordIndex]);
        }
        List<GameObject> players = PlayersDoingTask;
        _currentPlayer = players[0];
        _otherPlayer = players[1];
        TaskLoop();*/
    }


    void TaskLoop()
    {
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(true);
        _controller = _currentPlayer.GetComponent<PlayerController>();
        DisplayWordToFind();
    }

    void DisplayWordToFind()
    {
        _allWords.Clear();
        _keyToWord.Clear();
        foreach (WordConfig word in _wordsData.words)
        {
            _allWords.Add(word);
        }
        _rightWord = _allWords[Random.Range(0, _allWords.Count)];
        _allWords.Remove(_rightWord);
        string rightWordTrad = TraductionOfWord(_rightWord, SystemLanguage.Spanish);
        _npcDuolingo.GetComponent<UINpc>().DisplayTalkingBubble(true);
        _npcDuolingo.GetComponent<UINpc>().ChangeBubbleText(rightWordTrad + "?");
        DisplayAnswers(_rightWord);
    }

    void DisplayAnswers(WordConfig word)
    {
        List<WordConfig> answers = new List<WordConfig>();
        
        for (int i = 0; i < 3; i++)
        {
            WordConfig tempWord = _allWords[Random.Range(0, _allWords.Count)];
            _allWords.Remove(tempWord);
            answers.Add(tempWord);
        }
        answers[Random.Range(0, answers.Count)] = word;
        _currentPlayer.GetComponent<PlayerUI>().DisplayDuolingoUI(true);
        _currentPlayer.GetComponent<PlayerUI>().DisplayAnswersDuolingo(AnswerToDisplay(answers), _inputsName);
        InputAssignement(answers);
        _canPressInput = true;
        /*if (word == null) { return; }
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
        *//*ShuffleAnswers(answers);*//*
        InputAssignement(answers);
        _canPressInput = true;*/
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

    List<string> AnswerToDisplay(List<WordConfig> words)
    {
        List<string> tempWords = new List<string>();
        foreach (WordConfig word in words)
        {
            tempWords.Add(word.baseWord);

            /*switch (Difficulty)
            {
                case 1:

            }*/

        }
        return tempWords;
    }

    string TraductionOfWord(WordConfig word, SystemLanguage language)
    {
        string tempWord = "";
        foreach(WordConfig.WordWrapper trad in word.traductions)
        {
            if(trad.language == language)
            {
                tempWord = trad.word;
                break;
            }
        }
        if(tempWord == "")
        {
            tempWord = word.traductions[Random.Range(0, word.traductions.Count)].word;
        }
        return tempWord;
    }

    void InputAssignement(List<WordConfig> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            _wordToKey.Add(words[i], _inputsName[i]);
            _keyToWord.Add(_inputsName[i], words[i]);
        }
        _rightInputName = _wordToKey[_rightWord];
    }

    void PressInputCheck()
    {
        
        if (_inputValue == PlayerInputValue.WrongValue)
        {
            //_currentGuessWord = _keyToWord[_contextName];
            StartCoroutine(DisplayRightAnswer(false));
            _canPressInput = false;
            _contextName = "";
        }
        else if (_inputValue == PlayerInputValue.RightValue)
        {
            //_currentGuessWord = _keyToWord[_contextName];
            StartCoroutine(DisplayRightAnswer(true));
            _canPressInput = false;
            _rightAnswerIndex++;
            
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
            _inputValue = CheckInputValue(_contextName, _wordToKey[_rightWord], _controller);
           
            PressInputCheck();
        }
       
    }

    IEnumerator DisplayRightAnswer(bool isRight)
    {
        if (isRight)
        {
            _currentPlayer.GetComponent<PlayerUI>().ChangeColorAnswerBubble(_rightWord, Color.green);
            yield return new WaitForSeconds(1.5f);
            CheckIfReplay();
        }
        else
        {
            _currentPlayer.GetComponent<PlayerUI>().ChangeColorAnswerBubble(_rightWord, Color.green);
            _currentPlayer.GetComponent<PlayerUI>().ChangeColorAnswerBubble(_currentGuessWord, Color.red);
            yield return new WaitForSeconds(1.5f);
            EndDuolingo(false);
        }

        _currentPlayer.GetComponent<PlayerUI>().ClearColorAnswerBubble();
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
            player.GetComponent<SpriteRenderer>().sortingOrder = 8;
            player.GetComponent<PlayerController>().EnableMovementDisableInputs();
            if(value == false)
            {
                StartCoroutine(player.GetComponent<PlayerController>().PlayerDown(2f));
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

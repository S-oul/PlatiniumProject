using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static CowboyQTE;

public class LeCode : Task
{
    
    GameManager _gameManager;
    GameObject _playerDoingTask = null;
    PlayerController _controller;


    [SerializeField] GameObject _postIt;
    string _code = "";
    TextMeshPro _screenText;


    public bool HaveOnePlayer() { if (_playerDoingTask != null) return true; else return false; }
    public PlayerController Controller { get => _controller; set => _controller = value; }
    public GameObject Player { get => _playerDoingTask; set => _playerDoingTask = value; }
    public string Code { get => _code; set => _code = value; }

    private void Awake()
    {       
        _screenText = GetComponentInChildren<TextMeshPro>();
        _code = Random.Range(1000, 10000).ToString();
        print(_code);

        _gameManager = GameManager.Instance;
        for(int i = 0; i < 4; i++)
        {
            int r = Random.Range(0, _gameManager.RoomList.Count);
            while (_gameManager.RoomList[r].HasPostIt == true)
            {
                r = Random.Range(0, _gameManager.RoomList.Count);
            }
            GameObject go = Instantiate(_postIt);
            go.transform.position = _gameManager.RoomList[r].gameObject.transform.position;
            _gameManager.RoomList[r].HasPostIt = true;
        }
    }

    public override void Init()
    {
        
    }

    public override void End(bool isSuccessful)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (HaveOnePlayer())
        {
            if(_controller.CodeContext != null)
            {
                print(_controller.CodeContext);
                switch (_controller.CodeContext)
                {
                    //HAUT
                    case "Triangle":
                    case "Y":

                    break;

                    //BAS
                    case "Cross":
                    case "A":

                    break;

                    //Gauche
                    case "Square":
                    case "X":

                    break;

                    //Droite
                    case "Circle":
                    case "B":
                    
                    break;
                }
            }
        }
    }

}

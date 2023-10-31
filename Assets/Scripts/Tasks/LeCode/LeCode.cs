using System.Collections;
using TMPro;
using UnityEngine;

public class LeCode : Task
{
    
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
        _screenText.text = "";
        if (GameManager.Instance != null) _gameManager = GameManager.Instance;

        _code = Random.Range(1, 5).ToString();
        _code += Random.Range(1, 5).ToString();
        _code += Random.Range(1, 5).ToString();
        _code += Random.Range(1, 5).ToString();

        print(_code);
        int r = Random.Range(0, _gameManager.RoomList.Count);
        while (_gameManager.RoomList[r].HasPostIt)
        {
            r = Random.Range(0, _gameManager.RoomList.Count);
        }
        if(_gameManager.RoomList[r].PosItPos == null)
        {
            print(_gameManager.RoomList[r]);
            //_gameManager.RoomList[r].gameObject.SetActive(false);
        }
            GameObject go = Instantiate(_postIt);
            go.transform.SetParent(_gameManager.RoomList[r].gameObject.transform);
            go.transform.localPosition = _gameManager.RoomList[r].PosItPos.localPosition;
            _gameManager.RoomList[r].HasPostIt = true;
            go.transform.localScale = new Vector3(.1f, .2f, 1);
            //go.transform.localScale = new Vector3(1 / transform.parent.transform.localScale.x, 1 / transform.parent.transform.localScale.y, 1 / transform.parent.transform.localScale.z);
            go.GetComponent<PostIt>().Code = _code;
            go.GetComponent<PostIt>().Initialize();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void End(bool isSuccessful)
    {
        if (isSuccessful)
        {
            _screenText.color = Color.green;
            Debug.Log("Good code");
        }
        else
        {
            _screenText.color = Color.red;
            Debug.Log("Wrong code");

        }
        base.End(isSuccessful);
    }


    IEnumerator TimeBeforeRestart()
    {
        yield return new WaitForSeconds(2f);
        _screenText.text = "";
    }
    private void Update()
    {
        if (HaveOnePlayer())
        {
            if(_controller.CodeContext != null)
            {
                switch (_controller.CodeContext)
                {
                    //HAUT
                    case "Triangle":
                    case "Y":
                        //print("aaaa");
                        _screenText.text += "1";
                        _controller.CodeContext = "";
                    break;

                    //BAS
                    case "Cross":
                    case "A":
                        _screenText.text += "3";
                        _controller.CodeContext = "";

                        break;

                    //Gauche
                    case "Square":
                    case "X":
                        _screenText.text += "4";
                        _controller.CodeContext = "";

                        break;

                    //Droite
                    case "Circle":
                    case "B":
                        _screenText.text += "2";
                        _controller.CodeContext = "";

                        break;
                }
                if(_code == _screenText.text)
                {
                    End(true);
                }
                if(_screenText.text.Length > 4)
                {
                    _screenText.text = "";
                }
            }
        }
    }

}

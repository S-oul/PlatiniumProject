using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolleyballTwoVsTwo : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    int playerCount;
    List<Transform> _posPlayerList = new List<Transform>();
    [SerializeField] List<Transform> _iconsPlayers = new List<Transform>();

    Transform _spawnBallPos;

    [SerializeField] GameObject _ballPrefab;

    int _redPoints;
    int _bluePoints;

    GameObject _net;

    GameObject _playerTouch;

    Cam _cam;

    TextMeshProUGUI _textScore;

    [SerializeField] Transform _pointVolley;
    [SerializeField] int _pointsToWin;

    TextMeshProUGUI _textVolleyUI;

    Room _room;

    [SerializeField] AnimationCurve _animCurve;

    List<GameObject> _redTeam = new List<GameObject>();
    List<GameObject> _blueTeam = new List<GameObject>();

    public GameObject PlayerTouch { get => _playerTouch; set => _playerTouch = value; }
    public Transform PointVolley { get => _pointVolley; set => _pointVolley = value; }
    public List<GameObject> Players { get => players; set => players = value; }
    public TextMeshProUGUI TextVolleyUI { get => _textVolleyUI; set => _textVolleyUI = value; }
    public int PlayerCount { get => playerCount; set => playerCount = value; }

    private void Start()
    {
        _room = transform.parent.GetComponent<Room>();
    }
    public void AddPlayer(GameObject newPlayer)
    {
        int idPlayer = 0;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("FinalRoomMusic");
        }
        UIManager.Instance.UIVolley.gameObject.SetActive(true);
        TextVolleyUI = UIManager.Instance.UIVolley.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextVolleyUI.gameObject.SetActive(false);
        _textScore = _room.transform.Find("Score").GetChild(0).Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _textScore.text = "0 | 0";
        PointVolley = _room.transform.Find("PointVolley");

        PlayerTouch = null;
        foreach (Transform pos in _room.transform.Find("PlayerPositions"))
        {
            _posPlayerList.Add(pos);
        }
        ChangeIconScores(idPlayer, newPlayer);
        Transform newPos = _posPlayerList[idPlayer];
        _posPlayerList.Remove(newPos);
        newPlayer.transform.position = newPos.position;
        newPlayer.GetComponent<PlayerController>().ChangeMobiltyFactor(2.5f, 2);
        print("player");
        idPlayer++;





    }

    public void StartMatch()
    {
        _spawnBallPos = _room.transform.Find("BallStartPos");
        _net = _room.transform.Find("Net").gameObject;
        _cam = Camera.main.GetComponent<Cam>();
        StartCoroutine(TimerBeforeBall(4));
    }
    void ChangeIconScores(int ID, GameObject player)
    {
        switch (ID)
        {
            case 0:
                _blueTeam.Add(player);
                break;
            case 1:
                _redTeam.Add(player);
                break;
            case 2:
                _blueTeam.Add(player);
                break;
            case 3:
                _redTeam.Add(player);
                break;

        }

        _iconsPlayers[ID].GetComponent<Image>().sprite = player.transform.Find("Animation").GetComponent<SpriteRenderer>().sprite;
    }

    IEnumerator TimerBeforeBall(float time)
    {
        yield return new WaitForSeconds(time);
        /*while (time > 0)
        {
            
            time -= Time.deltaTime;
            
            //Feedback Canvas timer
            yield return null;
        }*/
        TextVolleyUI.gameObject.SetActive(false);
        SpawnVolleyBall();
    }

    void SpawnVolleyBall()
    {
        GameObject ball = Instantiate(_ballPrefab, _spawnBallPos.position, Quaternion.identity, _room.transform);
        //ball.GetComponent<BallVolley>().Task = this;


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void Point()
    {
        if (_redPoints < _pointsToWin && _bluePoints < _pointsToWin)
        {
            StartCoroutine(TimerBeforeBall(2f));
        }
        else if (_redPoints == _pointsToWin)
        {
            StartCoroutine(WinVolley("red"));
        }
        else if (_bluePoints == _pointsToWin)
        {
            StartCoroutine(WinVolley("blue"));
            

        }
    }
    IEnumerator LostTask()
    {
        Camera.main.GetComponent<Cam>().FixOnPlayerVoid(GameManager.Instance.LastPlayerToFail);
        Time.timeScale = .5f;
        yield return new WaitForSecondsRealtime(2.5f);
        Time.timeScale = 1f;
        End();
    }

    IEnumerator WinVolley(string colorTeam)
    {
        string text = "";
        switch (colorTeam)
        {
            case "red":
                text = "Red team has won!";
                break;
            case "blue":
                text = "Blue team has won!";
                break;

        }
        StartCoroutine(TextAnimation(text));
        yield return new WaitForSeconds(3f);
        TextVolleyUI.gameObject.SetActive(false);
        End();
    }
    public void CheckPoints(bool isForBlue)
    {
        if (isForBlue)
        {
            _bluePoints++;
        }
        else
        {
            _redPoints++;
        }
        _textScore.text = _bluePoints + " | " + _redPoints;
    }

    

    public IEnumerator TextAnimation(string text)
    {
        TextVolleyUI.text = text;
        float totalTime = 0.5f;
        float remainingTime = totalTime;
        float _fontSizeMax = 1800;
        TextVolleyUI.fontSize = _fontSizeMax;
        
        while (remainingTime > 0)
        {


            TextVolleyUI.fontSize = _fontSizeMax * _animCurve.Evaluate(Mathf.Lerp(0, 1, 1 - (remainingTime / totalTime)));
            remainingTime -= Time.deltaTime;
            yield return null;

        }
    }

   
}

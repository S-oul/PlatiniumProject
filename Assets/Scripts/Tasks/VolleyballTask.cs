using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolleyballTask : Task
{
    List<Transform> _posPlayerList = new List<Transform>();

    Transform _spawnBallPos;

    [SerializeField] float _timeBeforeStart;

    [SerializeField] GameObject _ballPrefab;

    [SerializeField] [Range(0, 100)] int _squidChanceToHit = 100;

    [SerializeField] int _pointsToWin;

    GameObject _tentacle;

    GameObject _net;

    GameObject _squid;

    GameObject _playerTouch;

    Cam _cam;

    TextMeshProUGUI _textScore;

    [SerializeField] Transform _pointVolley;

    TextMeshProUGUI _textVolleyUI;

    int _playersPoints = 0;
    int _squidPoints = 0;

    [SerializeField] AnimationCurve _animCurve;

    public GameObject Net { get => _net; set => _net = value; }
    public GameObject Squid { get => _squid; set => _squid = value; }
    public int SquidChanceToHit { get => _squidChanceToHit; set => _squidChanceToHit = value; }
    public GameObject PlayerTouch { get => _playerTouch; set => _playerTouch = value; }
    public Transform PointVolley { get => _pointVolley; set => _pointVolley = value; }
    public TextMeshProUGUI TextVolleyUI { get => _textVolleyUI; set => _textVolleyUI = value; }
    public GameObject Tentacle { get => _tentacle; set => _tentacle = value; }

    public override void End(bool isSuccessful)
    {

        base.End(isSuccessful);
        UIManager.Instance.UIVolley.gameObject.SetActive(false);
        foreach (GameObject player in PlayersDoingTask)
        {
            player.GetComponent<PlayerController>().ChangeMobiltyFactor(1, 1);
        }

        GameManager.Instance.DayIndex++;
        List<GameObject> l = new List<GameObject>();
        l = GameManager.Instance.Players;
        GameManager.Instance.DaySliderOverDay = GameManager.Instance.DaySlider.GetValue();
        SceneManager.LoadScene(2);
        GameManager.Instance.Players = l;
        
    }
    public override void Init()
    {

        base.Init();
        UIManager.Instance.UIVolley.gameObject.SetActive(true);
        _textVolleyUI = UIManager.Instance.UIVolley.GetChild(0).GetComponent<TextMeshProUGUI>();
        _textVolleyUI.gameObject.SetActive(false);
        _textScore = RoomTask.transform.Find("Score").GetChild(0).Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _textScore.text = "0 | 0";
        _pointVolley = RoomTask.transform.Find("PointVolley");
        PlayerTouch = null;
        foreach (Transform pos in RoomTask.transform.Find("PlayerPositions"))
        {
            _posPlayerList.Add(pos);
        }
        foreach (GameObject player in PlayersDoingTask)
        {
            Transform newPos = _posPlayerList[Random.Range(0, _posPlayerList.Count)];
            _posPlayerList.Remove(newPos);
            player.transform.position = newPos.position;
            player.GetComponent<PlayerController>().ChangeMobiltyFactor(1.5f, 2);
        }
        _spawnBallPos = RoomTask.transform.Find("BallStartPos");
        
        Squid = RoomTask.transform.Find("Squid").gameObject;
        Tentacle = _squid.transform.GetChild(0).GetChild(0).gameObject;
        Net = RoomTask.transform.Find("Net").gameObject;
        _cam = Camera.main.GetComponent<Cam>();
        _cam.FixOnRoomVoid(RoomTask);
        StartCoroutine(TimerBeforeBall(_timeBeforeStart));
        
    }

    IEnumerator TimerBeforeBall(float time)
    {
        yield return new WaitForSeconds(time/4 * 3);
        AudioManager.instance.PlaySFXOS("SquidVO", _squid.GetComponent<AudioSource>());
        yield return new WaitForSeconds(time/4);
        /*while (time > 0)
        {
            
            time -= Time.deltaTime;
            
            //Feedback Canvas timer
            yield return null;
        }*/
        _textVolleyUI.gameObject.SetActive(false);
        SpawnVolleyBall();
    }

    void SpawnVolleyBall()
    {
        GameObject ball = Instantiate(_ballPrefab, _spawnBallPos.position, Quaternion.identity, RoomTask.transform);
        ball.GetComponent<BallVolley>().Task = this;


    }

    public void Point()
    {
        
        
        
        if (_squidPoints < _pointsToWin && _playersPoints < _pointsToWin)
        {
            StartCoroutine(TimerBeforeBall(2f));
        }
        else if (_squidPoints == _pointsToWin)
        {
            _textVolleyUI.gameObject.SetActive(false);
            End(false);
            Debug.Log("Defeat");
        }
        else if (_playersPoints == _pointsToWin)
        {

            End(true) ;
            _textVolleyUI.gameObject.SetActive(false);
            Debug.Log("Win");
        }
    }

    public void CheckPoints(bool isForPlayer)
    {
        if (isForPlayer)
        {
            _playersPoints++;
        }
        else
        {
            _squidPoints++;
        }
        _textScore.text = _playersPoints + " | " + _squidPoints;
    }

    public void ChangeColorPlayers()
    {
        
        foreach (GameObject player in PlayersDoingTask)
        {
            print(player.name + " // " + PlayerTouch.name);
            if (player != null)
            {
                return;
            }
            if(player == PlayerTouch)
            {
                
                player.transform.Find("Animation").GetComponent<SpriteRenderer>().color = new Color(127, 127, 127);
            }
            else
            {
                player.transform.Find("Animation").GetComponent<SpriteRenderer>().color = Color.white;
            }

        }
    }

    public IEnumerator TextAnimation()
    {
        float totalTime = 0.5f;
        float remainingTime = totalTime;
        float _fontSizeMax = 1800;
        _textVolleyUI.fontSize = _fontSizeMax;
        while (remainingTime > 0)
        {


            _textVolleyUI.fontSize = _fontSizeMax * _animCurve.Evaluate(Mathf.Lerp(0, 1, 1 - (remainingTime / totalTime)));
            remainingTime -= Time.deltaTime;
            yield return null;

        }
    }

    public void PlayTentacleAnimation()
    {
        Tentacle.GetComponent<Animator>().SetTrigger("Attacks");
        AudioManager.instance.PlaySFXOS("SquidHit", _squid.GetComponent<AudioSource>());
    }
}
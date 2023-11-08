using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GraffitiGameManager : Task
{
    [SerializeField] List<GameObject> level1;
    [SerializeField] List<GameObject> level2;
    [SerializeField] List<GameObject> level3;
    [SerializeField] List<GameObject> level4;
    [SerializeField] List<GameObject> level5;

    [SerializeField] float _spriteDisapearanceSpeed = 1;

    List<GameObject> _graffitiList;
    GameObject _currentGraffitiBeingCleaned;
    Animator _currentGraffitiWashAnimator;
    SpriteRenderer _currentGraffitiSprite;
    float _currentOpacity;

    PlayerController _controller;

    int _previouseSwipeDirection = 0;
    int _totalSwipes = 0;
    float _timer = 0;
    int _timerResetInterval = 1; //at what intervals (in seconds) will the animation speed be updated
    float _swipeSpeed = 0;

    void Start()
    {
        _graffitiList = createOrderedDifficultyDict()[Difficulty];
        foreach (GameObject graffiti in _graffitiList)
        {
            graffiti.SetActive(true);
        }
    }

    public override void Init()
    {
        base.Init();

        _controller = PlayerGameObject.GetComponent<PlayerController>();
        _controller.EnableDecryptageDisableMovements();

        ChooseGraffitiToStartCleaning(); // Defines: _currentGraffitiBeingCleaned, _currentGraffitiWashAnimator, _currentGraffitiSprite, _currentOpacity

        StartTask();
    }
    public void StartTask()
    {
        IsStarted = true;
    }
    public override void End(bool isSuccessful)
    {
        if (!isSuccessful) { }
        _controller.DisableDecryptageEnableMovements();
        base.End(isSuccessful);
    }

    private void FixedUpdate()
    {
        if (IsStarted && !IsDone)
        {
            // record player actions
            switch (_controller.DecrytContext.x)
            {
                case > 0:
                    _controller.DecrytContext = Vector2.zero;
                    if (_previouseSwipeDirection == -1) { _totalSwipes++; }
                    _previouseSwipeDirection = 1;
                    break;
                case < 0:
                    _controller.DecrytContext = Vector2.zero;
                    if (_previouseSwipeDirection == 1) { _totalSwipes++; }
                    _previouseSwipeDirection = 0;
                    break;
                default:
                    _controller.DecrytContext = Vector2.zero;
                    break;
            }
            
            // manage animation speed
            _timer += Time.deltaTime;
            if (_timer > _timerResetInterval) 
            {
                _swipeSpeed = _totalSwipes;
                _timer = 0;
                _totalSwipes = 0;

                _currentGraffitiWashAnimator.GetComponent<Animator>().speed = _swipeSpeed;
                print("SWWWIIIIIIIIIIIIPPPPPES : " + _swipeSpeed);
            }

            // adjusting graffiti Opacity
            _currentOpacity -= _swipeSpeed / 1000 * _spriteDisapearanceSpeed;
            Debug.Log(_currentOpacity);
            _currentGraffitiSprite.color = new Color(1f, 1f, 1f, _currentOpacity);

            // check for win OR select next graffiti.
            if (_currentOpacity <= 0) 
            {
                DeactivateCurrentGraffiti();

                if (ThereIsMoreGraffitiOnWall())
                {
                    ChooseGraffitiToStartCleaning();
                }
                else End(true); 
            }
        }
    }

    //Helper functions 
    void ChooseGraffitiToStartCleaning() 
    {
        _currentGraffitiBeingCleaned = _graffitiList[0];
        _graffitiList.RemoveAt(0);

        GameObject _graffAnimObject = _currentGraffitiBeingCleaned.transform.GetChild(1).gameObject;
        _graffAnimObject.SetActive(true);
        _currentGraffitiWashAnimator = _graffAnimObject.GetComponent<Animator>();

        _currentGraffitiSprite = _currentGraffitiBeingCleaned.GetComponentInChildren<SpriteRenderer>();

        _currentOpacity = 1; 
    }
    bool ThereIsMoreGraffitiOnWall() { return (_graffitiList.Count != 0); }
    void DeactivateCurrentGraffiti() { _currentGraffitiBeingCleaned.SetActive(false); }
    Dictionary<int, List<GameObject>> createOrderedDifficultyDict()
    {
        Dictionary<int, List<GameObject>> dict = new Dictionary<int, List<GameObject>>();
        List<List<GameObject>> levelList = new List<List<GameObject>>
        {
            level1,
            level2,
            level3,
            level4,
            level5
        };
        int key = 1;
        foreach (List<GameObject> level in levelList)
        {
            dict.Add(key, level);
            key++;
        }
        return dict;
    }
}

/* choose a graffiti to spawn. 
 * when player interacts, teleport them to player spot and start game. 
 * The game:
 * how many times the player had gone back and forth. 
 * the num of 'wipes' per second determines the whipe animation speed. 
 * also determind the rate of graffiti disapearance. 
 * if new player is added, 
 *   TP them to spot
 *   added spunge to animate
 *   add their wipe count to the wipe per second counter. 
 * When graffiti visibility is zero, game win
 */

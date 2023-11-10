using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GraffitiGameManager : Task
{
    #region Declarations
    [SerializeField] List<GameObject> level1;
    [SerializeField] List<GameObject> level2;
    [SerializeField] List<GameObject> level3;
    [SerializeField] List<GameObject> level4;
    [SerializeField] List<GameObject> level5;

    [SerializeField] float _spriteDisapearanceSpeed = 1;

    List<GameObject> _tempGraffitiList;
    List<Graffiti> _graffitiList;
    GameObject _currentGraffitiBeingCleaned;
    Animator _currentGraffitiWashAnimator;
    SpriteRenderer _currentGraffitiSprite;
    float _currentOpacity;

    List<PlayerController> _controllers;

    int _previouseSwipeDirection = 0;
    int _totalSwipes = 0;
    float _timer = 0;
    int _timerResetInterval = 1; //at what intervals (in seconds) will the animation speed be updated
    float _swipeSpeed = 0;

    #endregion

    void Start()
    {
        _tempGraffitiList = createOrderedDifficultyDict()[Difficulty];
        Update_graffitiList(_tempGraffitiList);
        foreach (Graffiti graffiti in _graffitiList)
        {
            graffiti.Activate();
        }
    }

    public override void Init()
    {
        base.Init();
        _controllers.Clear();
        foreach(GameObject player in PlayersDoingTask)
        {
            _controllers.Add(player.GetComponent<PlayerController>());
            _controllers[0].EnableDecryptageDisableMovements();
        }
        
        
        //PlayerInGraffiti Player1 = new PlayerInGraffiti() { }

        /*
        PlayersDoingTask[0].GetComponent<PlayerController>().EnableDecryptageDisableMovements();
        _playersDoingTasksStList.Add(new graffitiPlayerStruct());
        _playersDoingTasksStList[0].playerObject = PlayersDoingTask[0];
        */
        if(PlayersDoingTask.Count == 1)
        {
            StartTask();
        }
         // Defines: _currentGraffitiBeingCleaned, _currentGraffitiWashAnimator, _currentGraffitiSprite, _currentOpacity

        
    }
    public void StartTask()
    {
        ChooseGraffitiToStartCleaning();
        IsStarted = true;
    }
    public override void End(bool isSuccessful)
    {
        //if (!isSuccessful) { }
        foreach (GameObject player in PlayersDoingTask)
        {
            //_controller.DisableDecryptageEnableMovements();
            player.GetComponent<PlayerController>().DisableDecryptageEnableMovements();
        }
        base.End(isSuccessful);
    }

    private void FixedUpdate()
    {
        if (IsStarted && !IsDone)
        {
            foreach(PlayerController controller in _controllers)
            // record player actions
            switch (controller.DecrytContext.x)
            {
                case > 0:
                    controller.DecrytContext = Vector2.zero;
                    if (_previouseSwipeDirection == -1) { _totalSwipes++; }
                    _previouseSwipeDirection = 1;
                    break;
                case < 0:
                    controller.DecrytContext = Vector2.zero;
                    if (_previouseSwipeDirection == 1) { _totalSwipes++; }
                    _previouseSwipeDirection = 0;
                    break;
                default:
                    controller.DecrytContext = Vector2.zero;
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
        _currentGraffitiBeingCleaned = _graffitiList[0].graffitiObject;
        _graffitiList.RemoveAt(0);

        GameObject _graffAnimObject = _currentGraffitiBeingCleaned.transform.GetChild(1).gameObject;
        _graffAnimObject.SetActive(true);
        _currentGraffitiWashAnimator = _graffAnimObject.GetComponent<Animator>();

        _currentGraffitiSprite = _currentGraffitiBeingCleaned.GetComponentInChildren<SpriteRenderer>();

        _currentOpacity = 1; 
    }

    void Update_graffitiList(List<GameObject> oldList)
    {
        foreach (GameObject obj in oldList)
        {
            _graffitiList.Add(new Graffiti(obj));
        }
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



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

    [SerializeField] float _adjustableSpriteDisapearanceSpeed = 1;

    List<GameObject> _tempGraffitiList;
    List<GraffitiDrawing> _graffitiList = new List<GraffitiDrawing>();
    /* Old Code
     * GameObject _currentGraffitiBeingCleaned;
    Animator _currentGraffitiWashAnimator;
    SpriteRenderer _currentGraffitiSprite;
    float _currentOpacity;*/

    List<PlayerInGraffiti> _listOfPlayersInGraffiti = new List<PlayerInGraffiti>();
    /* Old Code
     * PlayerController _controller;

    //int _previouseSwipeDirection = 0;
    //int _totalSwipes = 0;*/

    float _timer = 0;
    int _timerResetInterval = 1; //at what intervals (in seconds) will the animation speed be updated
    //float _swipeSpeed = 0;

    #endregion

    void Awake()
    {
        _tempGraffitiList = createOrderedDifficultyDict()[Difficulty];
        Update_graffitiList(_tempGraffitiList);
        foreach (GraffitiDrawing graffiti in _graffitiList)
        {
            graffiti.Activate();
        }
    }

    public override void Init()
    {
        base.Init();

        AddNewPlayer(PlayerGameObject);

        /* Less Old Code
         * PlayerInGraffiti newPlayer = new PlayerInGraffiti(PlayerGameObject);
        newPlayer.playerController.EnableDecryptageDisableMovements();
        newPlayer.AssignGraffiti(ChooseGraffitiToStartCleaning());

        _listOfPlayersInGraffiti.Add(newPlayer);*/

        /*
        PlayersDoingTask[0].GetComponent<PlayerController>().EnableDecryptageDisableMovements();
        _playersDoingTasksStList.Add(new graffitiPlayerStruct());
        _playersDoingTasksStList[0].playerObject = PlayersDoingTask[0];
        */ 

        StartTask();
    }
    public void StartTask()
    {
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

            //check if new player has joined
            if (NewPlayerHasJoined())
            {
                AddNewPlayer(PlayersDoingTask[-1]);
            }

            // record player actions
            foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti)
            {
                /* Old Code
                 * switch (player.playerController.DecrytContext.x)
                {
                    case > 0:
                        player.playerController.DecrytContext = Vector2.zero;
                        if (_previouseSwipeDirection == -1) { _totalSwipes++; }
                        _previouseSwipeDirection = 1;
                        break;
                    case < 0:
                        player.playerController.DecrytContext = Vector2.zero;
                        if (_previouseSwipeDirection == 1) { _totalSwipes++; }
                        _previouseSwipeDirection = 0;
                        break;
                    default:
                        player.playerController.DecrytContext = Vector2.zero;
                        break;
                }*/
                player.ManagerInput();
            }
            
            // manager player speed
            _timer += Time.deltaTime;
            if (_timer > _timerResetInterval) 
            {   
                foreach (GraffitiDrawing graffiti in _graffitiList) { graffiti.ResetOpasityChangeRate(); }

                foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti)
                {   
                    player.currentGraffitiAnimation.SetSpeed(player.GetPlayerSpeed());
                    player.currentGraffiti.AddToOpasityChangeRate(player.GetPlayerSpeed());
                    player.ResetSpeed();
                }
                _timer = 0;
            }

            // adjusting graffiti Opacity
            foreach (GraffitiDrawing graffiti in _graffitiList)
            {
                graffiti.UpdateOpacity(_adjustableSpriteDisapearanceSpeed);
            }

            /*Old Code
             * _currentOpacity -= _swipeSpeed / 1000 * _adjustableSpriteDisapearanceSpeed;
            Debug.Log(_currentOpacity);
            _currentGraffitiSprite.color = new Color(1f, 1f, 1f, _currentOpacity);*/

            // check for win OR select next graffiti.

            foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti)
            {
                if (player.currentGraffiti.currentOpacity == 0)
                {
                    player.currentGraffiti.Deactivate();
                    RemoveCleanedGraffiti(player.currentGraffiti);

                    if (ThereIsMoreGraffitiOnWall())
                    {
                        player.AssignGraffiti(ChooseGraffitiToStartCleaning());
                    }
                    else End(true);
                }
            }

            /* Old Code
             * if (_currentOpacity <= 0) 
            {
                DeactivateCurrentGraffiti();

                if (ThereIsMoreGraffitiOnWall())
                {
                    ChooseGraffitiToStartCleaning();
                }
                else End(true); 
            }*/
        }
    }

    //Helper functions 

    /* Old Code
     *void ChooseGraffitiToStartCleaning() 
    {
        _currentGraffitiBeingCleaned = _graffitiList[0];
        _graffitiList.RemoveAt(0);

        GameObject _graffAnimObject = _currentGraffitiBeingCleaned.transform.GetChild(1).gameObject;
        _graffAnimObject.SetActive(true);
        _currentGraffitiWashAnimator = _graffAnimObject.GetComponent<Animator>();

        _currentGraffitiSprite = _currentGraffitiBeingCleaned.GetComponentInChildren<SpriteRenderer>();

        _currentOpacity = 1; 
    }
    */

    GraffitiDrawing ChooseGraffitiToStartCleaning()
    {
        return _graffitiList[0];        
    }

    void Update_graffitiList(List<GameObject> oldList)
    {
        foreach (GameObject obj in oldList)
        {
            GraffitiDrawing newGraff = new GraffitiDrawing(obj);
            _graffitiList.Add(newGraff);
        }
    }

    void RemoveCleanedGraffiti(GraffitiDrawing graffiti)
    {
        _graffitiList.Remove(graffiti);
    }

    bool ThereIsMoreGraffitiOnWall() { return (_graffitiList.Count != 0); }
    /* Old Code 
     * //void DeactivateCurrentGraffiti() { _currentGraffitiBeingCleaned.SetActive(false); }*/

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

    bool NewPlayerHasJoined()
    {
        return (PlayersDoingTask.Count != _listOfPlayersInGraffiti.Count);
    }

    void AddNewPlayer(GameObject newPlayerObject)
    {
        PlayerInGraffiti newPlayer = new PlayerInGraffiti(newPlayerObject);
        newPlayer.playerController.EnableDecryptageDisableMovements();
        newPlayer.AssignGraffiti(ChooseGraffitiToStartCleaning());

        _listOfPlayersInGraffiti.Add(newPlayer);
    }
}




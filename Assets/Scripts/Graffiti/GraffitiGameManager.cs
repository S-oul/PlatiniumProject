using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraffitiGameManager : Task
{
    #region Declarations
    [SerializeField] List<GameObject> level1;
    [SerializeField] List<GameObject> level2;
    [SerializeField] List<GameObject> level3;
    [SerializeField] List<GameObject> level4;
    [SerializeField] List<GameObject> level5;

    // Adjust wash speed of specific graffiti
    [SerializeField] float SmalGraffitiWashSpeed = 1;
    [SerializeField] float MediumGraffitiWashSpeed = 1;
    [SerializeField] float LargeGraffitiWashSpeed = 1;

    // Adjust overall wash speed
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
        CreateGraffitiList();
        ActivateGraffitiInList();
    }

    public override void Init()
    {
        base.Init();
        //AddNewPlayer(PlayerGameObject);
        AddNewPlayer(PlayersDoingTask[0]) ;
        StartTask();

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
    }

    public void StartTask()
    {
        ChooseGraffitiToStartCleaning();
        IsStarted = true;
    }

    public override void End(bool isSuccessful)
    {
        //if (!isSuccessful) { }
        foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti)
        {
            player.setCraftingAnimation(false);
            player.playerController.DisableDecryptageEnableMovements();
        }
        base.End(isSuccessful);
    }


    private void FixedUpdate()
    {
        if (IsStarted && !IsDone)
        {
            //Debug.Log("len of PlayersDorinTask is " + PlayersDoingTask.Count + ". len of PlayersInGraffiti is " + _listOfPlayersInGraffiti.Count);

            //check if new player has joined
            if (NewPlayerHasJoined())
            {
                AddNewPlayer(PlayersDoingTask[PlayersDoingTask.Count-1]);
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
                player.ManageInput();
            }
            
            // manager player speed
            _timer += Time.deltaTime;
            if (_timer > _timerResetInterval) 
            {   
                foreach (GraffitiDrawing graffiti in _graffitiList) { graffiti.ResetOpasityChangeRate(); }
                foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti) { player.UpdateWashingSpeed(); }
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

            // check for win OR assign next graffiti.
            foreach (PlayerInGraffiti player in _listOfPlayersInGraffiti)
            {
                if (player.CurrentGraffitiIsClean())
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
        _currentGraffitiBeingCleaned = _graffitiList[0].graffitiObject;
        _graffitiList.RemoveAt(0);

        GameObject _graffAnimObject = _currentGraffitiBeingCleaned.transform.GetChild(1).gameObject;
        _graffAnimObject.SetActive(true);
        _currentGraffitiWashAnimator = _graffAnimObject.GetComponent<Animator>();

        _currentGraffitiSprite = _currentGraffitiBeingCleaned.GetComponentInChildren<SpriteRenderer>();

        _currentOpacity = 1; 
    }
    */

    void CreateGraffitiList() // defines _graffitiList
    {
        foreach (GameObject obj in createOrderedDifficultyDict()[Difficulty])
        {
            GraffitiDrawing newGraff = new GraffitiDrawing(obj, GetObjectWashSpeed(obj));
            _graffitiList.Add(newGraff);
        }
    }

    float GetObjectWashSpeed(GameObject obj)
    {
        switch (obj.name)
        {
            case "graffiti_petit":
                return SmalGraffitiWashSpeed;
            case "graffiti_moyen":
                return MediumGraffitiWashSpeed;
            case "graffiti_grand":
                return LargeGraffitiWashSpeed;
            default:
                throw new ArgumentException(obj.GetType().Name + " is not the name of an existing graffiti GameObject");
        }
    }

    void ActivateGraffitiInList()
    {
        foreach (GraffitiDrawing graffiti in _graffitiList) { graffiti.Activate(); }
    }

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

    void AddNewPlayer(GameObject newPlayerObject)
    {
        PlayerInGraffiti newPlayer = new PlayerInGraffiti(newPlayerObject);
        newPlayer.playerController.EnableDecryptageDisableMovements();
        newPlayer.AssignGraffiti(ChooseGraffitiToStartCleaning()); // new player MUST NOT be added if there are no remaining graffiti
        newPlayer.setCraftingAnimation(true);

        _listOfPlayersInGraffiti.Add(newPlayer);
    }

    bool NewPlayerHasJoined()
    {
        return (PlayersDoingTask.Count != _listOfPlayersInGraffiti.Count);
    }

    GraffitiDrawing ChooseGraffitiToStartCleaning()
    {
        if (_graffitiList.Count == 0) { throw new ArgumentException("Cannot call ChooseGraffitiToStartCleaning() if _graffitiList is empty."); }
        return _graffitiList[0];        
    }

    bool ThereIsMoreGraffitiOnWall() { return (_graffitiList.Count != 0); }

    /* Old Code
     * void Update_graffitiList(List<GameObject> oldList)
    {
        foreach (GameObject obj in oldList)
        {
            GraffitiDrawing newGraff = new GraffitiDrawing(obj);
            _graffitiList.Add(newGraff);
        }
    }*/

    void RemoveCleanedGraffiti(GraffitiDrawing graffiti)
    {
        _graffitiList.Remove(graffiti);
    }

    /* Old Code 
     * //void DeactivateCurrentGraffiti() { _currentGraffitiBeingCleaned.SetActive(false); }*/


}




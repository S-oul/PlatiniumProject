using System.Collections.Generic;
using UnityEngine;

public class PlayerInGraffiti
{
    GameObject _playerObject;
    GraffitiCleanAnimation _currentGraffitiAnimation;

    public PlayerController playerController { get; set; }
    public GraffitiDrawing currentGraffiti { get; set; }

    // for managing player input
    int _previouseSwipeDirection = 0;
    int _swipeCounter = 0;

    public PlayerInGraffiti(GameObject _playerObject)
    {
        this._playerObject = _playerObject;
        playerController = this._playerObject.GetComponent<PlayerController>();
    } 

    public void AssignGraffiti(GraffitiDrawing graffiti)
    {
        currentGraffiti = graffiti;
        _currentGraffitiAnimation = currentGraffiti.GetAvailableAnimationObject();
    }

    public void ManageInput()
    {
        switch (playerController.DecrytContext.x)
        {
            case > 0:
                playerController.DecrytContext = Vector2.zero;
                if (_previouseSwipeDirection == -1) { _swipeCounter++; }
                _previouseSwipeDirection = 1;
                break;
            case < 0:
                playerController.DecrytContext = Vector2.zero;
                if (_previouseSwipeDirection == 1) { _swipeCounter++; }
                _previouseSwipeDirection = 0;
                break;
            default:
                playerController.DecrytContext = Vector2.zero;
                break;
        }
    }

    public void UpdateWashingSpeed()
    {
        _currentGraffitiAnimation.SetSpeed(GetPlayerSpeed());
        currentGraffiti.AddToOpasityChangeRate(GetPlayerSpeed());
        ResetSpeed();
    }

    public int GetPlayerSpeed() => _swipeCounter;

    public void ResetSpeed() { _swipeCounter = 0; }

    public bool CurrentGraffitiIsClean() => currentGraffiti.IsClean();

    public void setCraftingAnimation(bool b)
    {
        _playerObject.GetComponentInChildren<Animator>().SetBool("isInteractingWithItem", b);
    }
}

// ----------------------------------------------------------------------------------------------------

public class GraffitiDrawing
{
    GameObject _graffitiObject;

    float _currentOpacity = 1;
    float _opasityChangeRate = 0;
    float _graffitiSpecificWashSpeed;

    Dictionary<GraffitiCleanAnimation, bool> _animationAciveStatusDict = new Dictionary<GraffitiCleanAnimation, bool>();

    public GraffitiDrawing(GameObject graffitiObject, float washSpeed)
    {
        _graffitiObject = graffitiObject;
        _graffitiSpecificWashSpeed = washSpeed;
    }

    public void Activate() 
    {
        _graffitiObject.SetActive(true);
        foreach (Transform child in _graffitiObject.transform.Find("Animations"))
        {
            Debug.Log(child);
            _animationAciveStatusDict.Add(new GraffitiCleanAnimation(child.gameObject), false);
        }
    }

    public void Deactivate() { _graffitiObject.SetActive(false);}

    public void UpdateOpacity(float speedAdjustment)
    {
        _currentOpacity -= _opasityChangeRate / 1000 * _graffitiSpecificWashSpeed * speedAdjustment;        
        _currentOpacity = _currentOpacity < 0 ? 0 : _currentOpacity;
        _graffitiObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f,_currentOpacity );
    }

    public void AddToOpasityChangeRate(int playerSpeed)
    {
        _opasityChangeRate += playerSpeed;
    }

    public void ResetOpasityChangeRate()
    {
        //Debug.Log("OPASITY CHANGE RATE SET TO 0");
        _opasityChangeRate = 0;
    }

    public bool IsClean() => _currentOpacity == 0;

    public GraffitiCleanAnimation GetAvailableAnimationObject()
    {
        foreach (GraffitiCleanAnimation animator in _animationAciveStatusDict.Keys)
        {
            if (_animationAciveStatusDict[animator] != true) 
            {
                _animationAciveStatusDict[animator] = true;
                animator.Activate();
                return animator; 
            }
        }
        Debug.Log("This code should never need to run!"); 
        return new GraffitiCleanAnimation(); // This must not run. 
    }
}

// ----------------------------------------------------------------------------------------------------

public class GraffitiCleanAnimation
{
    GameObject _cleanAnimationObject;

    public GraffitiCleanAnimation(GameObject _cleanAnimationObject)
    {
        this._cleanAnimationObject = _cleanAnimationObject;
    }

    public void Activate()
    {
        _cleanAnimationObject.SetActive(true);

    }

    public void Deactivate()
    {
        _cleanAnimationObject.SetActive(false);
    }

    public void SetSpeed(int speed)
    {
        _cleanAnimationObject.GetComponent<Animator>().speed = speed;
    }

    public bool ParentIsActive() => _cleanAnimationObject.GetComponentInParent<Transform>().gameObject.activeSelf;

    public GraffitiCleanAnimation(){} //This must never run
}
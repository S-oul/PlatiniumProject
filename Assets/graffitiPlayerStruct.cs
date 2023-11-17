using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInGraffiti
{
    public GameObject playerObject;
    public PlayerController playerController;

    public GraffitiDrawing currentGraffiti;
    public GraffitiCleanAnimation currentGraffitiAnimation;

    int _previouseSwipeDirection = 0;
    int swipeCounter = 0;

    public PlayerInGraffiti(GameObject _playerObject)
    {
        playerObject = _playerObject;
        playerController = playerObject.GetComponent<PlayerController>();
    }

    public void AssignGraffiti(GraffitiDrawing graffiti)
    {
        currentGraffiti = graffiti;
        currentGraffitiAnimation = currentGraffiti.GetAvailableAnimationObject();
    }

    public void ManagerInput()
    {
        switch (playerController.DecrytContext.x)
        {
            case > 0:
                playerController.DecrytContext = Vector2.zero;
                if (_previouseSwipeDirection == -1) { swipeCounter++; }
                _previouseSwipeDirection = 1;
                break;
            case < 0:
                playerController.DecrytContext = Vector2.zero;
                if (_previouseSwipeDirection == 1) { swipeCounter++; }
                _previouseSwipeDirection = 0;
                break;
            default:
                playerController.DecrytContext = Vector2.zero;
                break;
        }
    }

    public int GetPlayerSpeed()
    {
        return swipeCounter;
    }
    public void ResetSpeed() { swipeCounter = 0; }
}



public class GraffitiDrawing
{
    public GameObject graffitiObject;
    public float currentOpacity = 1;
    public float opasityChangeRate = 0;

    Dictionary<GraffitiCleanAnimation, bool> animationAciveStatusDict = new Dictionary<GraffitiCleanAnimation, bool>();

    public GraffitiDrawing(GameObject _graffitiObject)
    {
        graffitiObject = _graffitiObject;
    }

    public void Activate() 
    {
        Debug.Log("Activate graffiti func started");
        graffitiObject.SetActive(true);
        foreach (Transform child in graffitiObject.transform.Find("Animations"))
        {
            Debug.Log(child);
            animationAciveStatusDict.Add(new GraffitiCleanAnimation(child.gameObject), false);
        }
    }

    public void Deactivate() { graffitiObject.SetActive(false);}

    public void AddToOpasityChangeRate(int playerSpeed)
    {
        opasityChangeRate += playerSpeed;
    }

    public void ResetOpasityChangeRate() {
        Debug.Log("OPASITY RATE SET TO 0");
        opasityChangeRate = 0; }

    public void UpdateOpacity(float speedAdjustment)
    {
        //Debug.Log("Changerate is " + opasityChangeRate);


        currentOpacity -= opasityChangeRate / 1000 * speedAdjustment;

        //Debug.Log("1 - " + opasityChangeRate + "/1000 * " + speedAdjustment + " = " + currentOpacity);
        Debug.Log(opasityChangeRate / 1000 * speedAdjustment);
        
        currentOpacity = currentOpacity < 0 ? 0 : currentOpacity;
        graffitiObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f,currentOpacity );
        //Debug.Log("Opasity is " +  currentOpacity);

    }

    public GraffitiCleanAnimation GetAvailableAnimationObject()
    {
        foreach (GraffitiCleanAnimation animator in animationAciveStatusDict.Keys)
        {
            if (animationAciveStatusDict[animator] != true) 
            { 
                animator.Activate();
                return animator; 
            }
        }
        Debug.Log("This code should never need to run!"); 
        return new GraffitiCleanAnimation(); // This must not run. 
    }
}



public class GraffitiCleanAnimation
{
    public GameObject cleanAnimationObject;

    public GraffitiCleanAnimation(GameObject _cleanAnimationObject)
    {
        cleanAnimationObject = _cleanAnimationObject;
    }

    public void SetSpeed(int speed)
    {
        cleanAnimationObject.GetComponent<Animator>().speed = speed;
    }

    public void Activate()
    {
        cleanAnimationObject.SetActive(true);
    }

    public void Deactivate()
    {
        cleanAnimationObject.SetActive(false);
    }

    public GraffitiCleanAnimation(){}
}
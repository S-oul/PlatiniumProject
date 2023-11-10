using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGraffiti
{
    public GameObject playerObject;
    public PlayerController playerController;

    public Graffiti currentGraffiti;
    public GraffitiCleanAnimation currentGraffitiAnimation;
}

public class Graffiti
{
    public GameObject graffitiObject;
    public float currentOpacity = 1;

    Dictionary<GraffitiCleanAnimation, bool> animationAciveStatusDict;

    public Graffiti(GameObject _graffitiObject) 
    {
        graffitiObject = _graffitiObject;

        foreach (Transform child in graffitiObject.transform.Find("Animation"))
        {
            animationAciveStatusDict.Add(new GraffitiCleanAnimation(child.gameObject), false);
        }
    }

    public void Activate() { graffitiObject.SetActive(true); }

    public GraffitiCleanAnimation GetAvailableAnimationObject()
    {
        foreach (GraffitiCleanAnimation animator in animationAciveStatusDict.Keys)
        {
            if (animationAciveStatusDict[animator] != true) { return animator; }
        }
        Debug.Log("This code should never need to run!"); 
        return new GraffitiCleanAnimation(); // This must not run. 
    }
}

public class GraffitiCleanAnimation
{
    public GameObject CleanAnimationObject;
    public float CleanAnimationSpeed;

    public GraffitiCleanAnimation(GameObject _cleanAnimationObject)
    {
        CleanAnimationObject = _cleanAnimationObject;
    }

    public void SetSpeed(float speed)
    {
        CleanAnimationObject.GetComponent<Animator>().speed = speed;
    }

    public GraffitiCleanAnimation(){}
}
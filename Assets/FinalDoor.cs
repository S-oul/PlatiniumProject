using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private GameObject _door1;
    private GameObject _door2;
    void Start()
    {
        _door1 = transform.GetChild(0).gameObject;
        _door2 = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

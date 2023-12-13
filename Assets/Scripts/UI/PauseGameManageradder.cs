using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameManageradder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PauseMenu = transform.GetChild(0).gameObject;
    }
}

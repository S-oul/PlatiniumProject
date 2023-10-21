using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerEnemyDetection : MonoBehaviour
{
    PlayerPointerMover _playerPointerMover;

    public void Awake()
    {
        _playerPointerMover = new PlayerPointerMover();
    }

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "DecryptageEnemy")
        {
            _playerPointerMover.killPlayerPointer();
        }

        if (otherCollider.gameObject.tag == "DecryptageTarget")
        {
            Debug.Log("WIN!");
        }
    }
}

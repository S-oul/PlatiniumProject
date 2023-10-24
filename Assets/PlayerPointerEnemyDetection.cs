using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerEnemyDetection : MonoBehaviour
{
    public PlayerPointerMover _playerPointerMover;

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "DecryptageEnemy")
        {
            _playerPointerMover.killPlayerPointer();
            Debug.Log("Hit!");
        }

        if (otherCollider.gameObject.tag == "DecryptageTarget")
        {
            Debug.Log("WIN!");
        }
    }
}

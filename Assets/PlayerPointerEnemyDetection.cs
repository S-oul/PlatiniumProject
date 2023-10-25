using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerEnemyDetection : MonoBehaviour
{
    public PlayerPointerMover _playerPointerMover;
    int _loseCount = 0;

    void OnTriggerEnter2D (Collider2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "DecryptageEnemy")
        {   
            _playerPointerMover.killPlayerPointer();
        }

        if (otherCollider.gameObject.tag == "DecryptageTarget")
        {
            _playerPointerMover.EndGame(PlayerPointerMover.END_STATE.WIN);
        }
    }
}

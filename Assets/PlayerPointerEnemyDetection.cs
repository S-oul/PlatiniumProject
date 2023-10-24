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
            transform.parent.parent.GetComponent<DecryptageTask>().End(false);
            Debug.Log("Hit!");
        }

        if (otherCollider.gameObject.tag == "DecryptageTarget")
        {
            transform.parent.parent.GetComponent<DecryptageTask>().End(true);
            Debug.Log("WIN!");
        }
    }
}

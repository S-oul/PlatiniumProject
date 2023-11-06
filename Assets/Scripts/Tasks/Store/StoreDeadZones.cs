using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDeadZones : MonoBehaviour
{
    bool _isInOtherCollider = true;

    public bool IsInOtherCollider { get => _isInOtherCollider; set => _isInOtherCollider = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZoneStore"))
        {
            print("eeeee");
            _isInOtherCollider = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZoneStore"))
        {
            print("fffffffffffffffffffffff");
            _isInOtherCollider = false;
        }
    }
}

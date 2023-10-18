using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LayerCenterSt
{
    public Transform transform;

    public Vector3 pos => transform.position;
    public float spinSpeed;

    public LayerCenterSt(Transform objectTrans, float speed)
    {
        this.transform = objectTrans;
        this.spinSpeed = speed;
    }
}

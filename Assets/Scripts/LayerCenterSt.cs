using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class LayerCenterSt : MonoBehaviour 
//{

    [Serializable]
    public struct LayerCenterSt
    {
        public Transform transform;
        public float spinSpeed;

        public LayerCenterSt(Transform objectTrans, float speed)
        {
            this.transform = objectTrans;
            this.spinSpeed = speed;
        }
    }
//}

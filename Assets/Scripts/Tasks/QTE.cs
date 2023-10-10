using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : Tasks, ITimedTask
{

    
    public float _givenTime => 0f;
    //[Header("QTE variables")]
   
    enum QTEInputs
    {
        None, 
        AnyKey,
        X,
        Y,
        A,
        B,
        L1,
        L2,
        L3,
        R1,
        R2,
        R3
    }

    [Header("QTE variables")]
    [SerializeField] List<QTEInputs> _inputsNeeded;

}

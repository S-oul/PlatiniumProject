using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<LayerCenterSt> _layerStructs;
    public List<float> speedList;

    bool _doSpin = false;

    public bool DoSpin { get => _doSpin; set => _doSpin = value; }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _layerStructs.Add(new(transform.GetChild(i), speedList[i]));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_doSpin)
        {
            for (int i = 0; i < transform.childCount; i++)
                _layerStructs[i].transform.Rotate(0, 0, _layerStructs[i].spinSpeed); 
        }
    }
}

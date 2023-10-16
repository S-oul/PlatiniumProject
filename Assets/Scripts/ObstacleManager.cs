using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] float _layer1Speed;
    [SerializeField] float _layer2Speed;
    [SerializeField] float _layer3Speed;
    [SerializeField] float _layer4Speed;


    LayerCenter _layer1 = new LayerCenter() { };
    LayerCenter _layer2 = new LayerCenter();
    LayerCenter _layer3 = new LayerCenter();
    LayerCenter _layer4 = new LayerCenter();

    // Start is called before the first frame update
    void Start()
    {
        //_layer1Trans = Layer1Center.GetComponent<Transform>();
        //Debug.Log(_layer1Trans.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

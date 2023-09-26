using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GenerateBuilding : MonoBehaviour
{
    public int NombreDeSalle = 3;
    public int NombreDetage = 3;
    public float EspaceDetage = 20;
    public List<GameObject> Pool = new List<GameObject>();

    List<GameObject> PlacedRoom = new List<GameObject>();
    bool isFisrt = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < NombreDetage; i++)
        {
            float EspaceEtage = i * EspaceDetage;
            isFisrt = true;
            int y = 0;
            while(y < NombreDeSalle)
            {
                //TODO

            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

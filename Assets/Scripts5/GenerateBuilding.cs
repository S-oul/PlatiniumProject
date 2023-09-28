using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        for (int y = 0; y < NombreDetage; y++)
        {
            float EspaceEtage = y * EspaceDetage;
            isFisrt = true;
            int i = 0;
            while(i <= NombreDeSalle)
            {
                GameObject go = Instantiate(Pool[Random.Range(0,Pool.Count)], transform);

                float dist = 0;
                foreach(GameObject olds in PlacedRoom) 
                {
                    dist += olds.transform.localScale.x*1.5f;
                }
                go.transform.position += new Vector3(dist, 0,0);
                go.name = "Room " + i;
                PlacedRoom.Add(go);
                i += go.GetComponent<Room>().roomType;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

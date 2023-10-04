using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class GenerateBuilding : MonoBehaviour
{
    public int NombreDeSalle = 3;
    public int NombreDetage = 3;
    public float EspaceDetage = 20;
    public List<GameObject> Pool = new List<GameObject>();

    List<int> PlacedRoom;
    bool isFisrt = true;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }


    private void Generate()
    {
        PlacedRoom = new List<int>(NombreDeSalle * NombreDetage);
        print(NombreDeSalle * NombreDetage);
        string str = "";
        foreach(int i in PlacedRoom)
        {
            PlacedRoom[i] = Random.Range(1,4);
            if(str.Count() % 3 == 0)
            {
                str += "\n";
            }
            str += PlacedRoom[i].ToString();
            
        }
        print(str);

        /*switch ()
        {
            case 1:
                PlacedRoom.Add(R) ;


                break;
                

            *//*case 2: Instantiate(go, transform); break;
            case 3: Instantiate(go, transform); break;*//*

        }*/

        /*int espaceEtageRestant = NombreDeSalle;
        while(espaceEtageRestant != 0)
        {
            GameObject go = Pool[Random.Range(0,Pool.Count)];
            int roomspace = go.GetComponent<Room>().roomType;

            if (espaceEtageRestant - roomspace < 0)
            {
                print("iveBreak");
                break;
            }else if(espaceEtageRestant == NombreDeSalle)
            {
                print("imaONE");
                Instantiate(go, transform);
            }else
            {

                Instantiate(go, transform);
                Vector2 pos = new Vector3(PlacedRoom[PlacedRoom.Count - 1].transform.position.x + PlacedRoom[PlacedRoom.Count - 1].transform.localScale.x/2, 0, 0);
                print("pos : " + pos);
                go.transform.localPosition = pos;
            }
            espaceEtageRestant -= roomspace;
            go.name = espaceEtageRestant.ToString();
            PlacedRoom.Add(go);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

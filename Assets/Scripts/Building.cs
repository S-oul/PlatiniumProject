using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int _maxRooms = 3;
    [SerializeField] private int _maxFloors = 5;
    [SerializeField] private float _heightBetweenFloor = 2f;

    [SerializeField] private List<GameObject> _poolType1 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType2 = new List<GameObject>();
    [SerializeField] private List<GameObject> _poolType3 = new List<GameObject>();

    [SerializeField] private List<Floors> _floors = new List<Floors>();

    private void Start()
    {
        for (float i = 0; i < _maxFloors; i++)
        {
            GenerateFloor(i*5f + i* _heightBetweenFloor);

        }
    }

    void GenerateFloor(float height)
    {
        List<GameObject> rooms = new List<GameObject>();
        int currentRoom = 0;
        bool isfirst = true;

        while (currentRoom != _maxRooms)
        {
            int r = Random.Range(1, 4);
            while (currentRoom + r > _maxRooms)
            {
                r -= 1;
                if (r == 0)
                {
                    print("NOPE R = 0" );
                    return;
                }
            }
            if (r == 1)
            {
                GameObject go = Instantiate(_poolType1[Random.Range(0, _poolType1.Count)], transform);
                Room room = go.GetComponent<Room>();
                currentRoom += room.RoomSize;
                if (isfirst) 
                { 
                    isfirst = false;
                    go.transform.position = new Vector3(Mathf.Abs(room.transform.localScale.x/2), height, 0);
                }
                else
                {
                    go.transform.position = new Vector3(rooms[^1].transform.localPosition.x + rooms[^1].transform.localScale.x/2 + go.transform.localScale.x/2, height, 0);
                }
                rooms.Add(go);
            }
            else if (r == 2)
            {
                GameObject go = Instantiate(_poolType2[Random.Range(0, _poolType2.Count)], transform);
                Room room = go.GetComponent<Room>();
                currentRoom += room.RoomSize;
                if (isfirst)
                {
                    isfirst = false;
                    go.transform.position = new Vector3(Mathf.Abs(room.transform.localScale.x / 2), height, 0);
                }
                else
                {
                    go.transform.position = new Vector3(rooms[^1].transform.localPosition.x + rooms[^1].transform.localScale.x / 2 + go.transform.localScale.x / 2, height, 0);
                }
                rooms.Add(go);
            }
            else
            {
                GameObject go = Instantiate(_poolType3[Random.Range(0, _poolType3.Count)], transform);
                Room room = go.GetComponent<Room>();
                currentRoom += room.RoomSize;
                if (isfirst)
                {
                    isfirst = false;
                    go.transform.position = new Vector3(Mathf.Abs(room.transform.localScale.x / 2), height, 0);
                }
                else
                {
                    go.transform.position = new Vector3(rooms[^1].transform.localPosition.x + rooms[^1].transform.localScale.x / 2 + go.transform.localScale.x / 2, height, 0);
                }
                rooms.Add(go);
            }
        }
    }
/*    bool isFloorFull(Floors floor)
    {
        int current = 0;
        foreach(Room room in floor.Rooms)
        {
            current += room.RoomSize;
        }
        if(current == _maxRooms) { return true; }
        else { return false; }
    }*/

    [System.Serializable]
    public struct Floors
    {
        private List<Room> rooms;
        private int _actualRoom;
        public List<Room> Rooms { get => rooms; set => rooms = value; }

    }
}

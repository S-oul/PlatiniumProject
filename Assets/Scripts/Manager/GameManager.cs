using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    List<GameObject> _npcList = new List<GameObject>();
    List<GameObject> _objectList = new List<GameObject>();
    List<GameObject> _eventList = new List<GameObject> ();
    
    List<Room> _roomList = new List<Room>();


    public List<Lift> _liftList = new List<Lift>();
    int _playerCount;
    
    [SerializeField] GameObject[] _players = new GameObject[4];

    public int PlayerCount { get => _playerCount; set => _playerCount = value; }

    public GameObject[] Players { get => _players; }
    public List<Room> RoomList { get => _roomList; set => _roomList = value; }

    public void ResetAllList()
    {
        _npcList.Clear();
        _objectList.Clear();
        _eventList.Clear();
        _liftList.Clear();

    }

    public void LinkLifts()
    {
        Shuffle(_liftList);
        for(int i = 0; i < _liftList.Count; i++)
        {
            if (i + 1 >= _liftList.Count)
            {
                _liftList[i].TeleportPos = _liftList[0].MyPos;

            }
            else
            {
                _liftList[i].TeleportPos = _liftList[i+1].MyPos;
            }
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    private List<Lift> Shuffle(List<Lift> list)
    {
        int r = Random.Range(0, list.Count - 2);
        int n = list.Count;
        while (n> 1)
        {
            Lift l = list[r];
            list.RemoveAt(r);
            list.Add(l);
            n--;
        }
        return list;
    }

}

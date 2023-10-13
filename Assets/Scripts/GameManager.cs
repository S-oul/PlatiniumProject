using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> _npcList = new List<GameObject>();
    public List<GameObject> _objectList = new List<GameObject>();
    public List<GameObject> _eventList = new List<GameObject> ();


    public List<Lift> _liftList = new List<Lift>();


    [SerializeField] GameObject[] _players = new GameObject[4];
    
    public GameObject[] Players { get => _players; }



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
        int n = list.Count;
        while (n> 1)
        {
            Lift l = list[n-1];
            int r = Random.Range(0, list.Count);
            list.Insert(r, l);
            n--;
        }
        return list;
    }

}

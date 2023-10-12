using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> _npcList = new List<GameObject>();
    public List<GameObject> _objectList = new List<GameObject>();
    public List<GameObject> _EventList = new List<GameObject> ();




    [SerializeField] GameObject[] _players = new GameObject[4];
    
    public GameObject[] Players { get => _players; }



    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

}

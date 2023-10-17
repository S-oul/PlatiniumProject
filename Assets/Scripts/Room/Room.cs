using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class Room : MonoBehaviour
{
    #region Variables
    [Range(1,4)]
    [SerializeField] int _roomSize = 1;
    [SerializeField] string _id = "UNSET ==> go to room prefab";

    [SerializeField] List<GameObject> _objectList;
    [SerializeField] List<GameObject> _npcList;
    [SerializeField] List<GameObject> _eventList;
    #endregion
    
    GameManager _gameManager;


    #region in Game Variable

    [SerializeField] bool _isPlayerRoom;
    [SerializeField] List<GameObject> _listPlayer = new List<GameObject>();

    #endregion

    #region Accesseur
    public int RoomSize { get => _roomSize; }
    public string Id { get => _id; set => _id = value; }
    #endregion

    public void InitRoom()
    {
        if(GameManager.Instance != null) _gameManager = GameManager.Instance;

        foreach(var o in _npcList)
        {
            _gameManager._npcList.Add(o);
        }
        foreach (var o in _objectList)
        {
            _gameManager._objectList.Add(o);
        }
        foreach (var o in _eventList)
        {
            _gameManager._eventList.Add(o);
        }
        ///Lift
        if (_id.Contains("L"))
        {
            _gameManager._liftList.Add(GetComponent<Lift>());
        }

        ///Spawn
        if (_id.Contains("S"))
        {

        }
    }

    #region UNITY EDITOR
#if UNITY_EDITOR

    private void OnValidate()
    {
        transform.localScale = new Vector3(RoomSize * 5, transform.localScale.y, 1);
    }
#endif
    #endregion
}

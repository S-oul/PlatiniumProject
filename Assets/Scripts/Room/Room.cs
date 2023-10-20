using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    #region Variables
    [Range(1,4)]
    [SerializeField] int _roomSize = 1;
    [SerializeField] string _id = "UNSET ==> go to room prefab";

    //Color
    [SerializeField] SpriteRenderer _sprite;
    float h = 0;
    float s = 0;
    float v = 0;

    [SerializeField] List<GameObject> _objectList;
    [SerializeField] List<GameObject> _npcList;
    [SerializeField] List<GameObject> _eventList;



    #endregion
    
    GameManager _gameManager;


    #region in Game Variable

    [SerializeField] List<GameObject> _listPlayer = new List<GameObject>();
    [SerializeField] bool _isPlayerInRoom() { if (_listPlayer.Count > 0) return true; else return false; }

    #endregion

    #region Accesseur
    public int RoomSize { get => _roomSize; }
    public string Id { get => _id; set => _id = value; }
    public List<GameObject> ListPlayer { get => _listPlayer; set => _listPlayer = value; }
    #endregion

    public void InitRoom()
    {
        if(GameManager.Instance != null) _gameManager = GameManager.Instance;
        if(_sprite == null) { _sprite = GetComponentInChildren<SpriteRenderer>(); }


        Color.RGBToHSV(_sprite.color, out h, out s, out v);
        print(gameObject.name + " : " + h + " " + s + " " + v);
        _sprite.color = Color.HSVToRGB(h, s, .4f);


        foreach (var o in _npcList)
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

    public void OnRoomEnter()
    {
        _sprite.color = Color.HSVToRGB(h, s, v);
    }
    public void OnRoomExit()
    {
        //_sprite.color = Color.HSVToRGB(h, s, 0.1f);

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

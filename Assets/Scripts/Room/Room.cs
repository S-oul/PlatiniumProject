using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Room : MonoBehaviour
{
    #region Variables
    [Range(1,4)]
    [SerializeField] int _roomSize = 1;
    [SerializeField] string _id = "UNSET ==> go to room prefab";

    //Color
    [SerializeField] SpriteRenderer _spriteRoom;
    float h = 0;
    float s = 0;
    float v = 0;

    [SerializeField] SpriteRenderer _screenTexture;

    [SerializeField] List<GameObject> _objectList;
    [SerializeField] List<GameObject> _npcList;
    [SerializeField] List<GameObject> _eventList;

    Task _task;

    [SerializeField] Transform _posItPos;
    [SerializeField] bool _hasPostIt = false;

    [SerializeField] WinStateScreen _winStateScreen;

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

    public Task TaskRoom { get => _task; set => _task = value; }
    public SpriteRenderer ScreenTexture { get => _screenTexture; set => _screenTexture = value; }
    public bool HasPostIt { get => _hasPostIt; set => _hasPostIt = value; }
    public Transform PosItPos { get => _posItPos; set => _posItPos = value; }
    public WinStateScreen WinStateScreen { get => _winStateScreen; set => _winStateScreen = value; }
    #endregion

    public virtual void InitRoom()
    {
        if(GameManager.Instance != null) _gameManager = GameManager.Instance;
        if(_spriteRoom == null) { _spriteRoom = GetComponentInChildren<SpriteRenderer>(); }

        //WinStateScreen.ChangeColor(Color.white);
        Color.RGBToHSV(_spriteRoom.color, out h, out s, out v);
        //print(gameObject.name + " : " + h + " " + s + " " + v);
        _spriteRoom.color = Color.HSVToRGB(h, s, .2f);

        _gameManager.RoomList.Add(this);
/*        foreach (var o in _npcList)
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
        }*/
        ///Lift
        if (_id.Contains("L"))
        {
            _gameManager.LiftList.Add(GetComponent<Lift>());
        }

        ///Spawn
        if (_id.Contains("S"))
        {

        }
    }

    public void OnRoomEnter()
    {
        _spriteRoom.color = Color.HSVToRGB(h, s, v);
    }
    public void OnRoomExit()
    {
        //_sprite.color = Color.HSVToRGB(h, s, 0.1f);

    }

    #region UNITY EDITOR
/*#if UNITY_EDITOR

    private void OnValidate()
    {
        transform.localScale = new Vector3(RoomSize * 5, transform.localScale.y, 1);
    }
#endif*/
    #endregion
}

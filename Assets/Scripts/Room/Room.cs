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

    Task _task;

    [SerializeField] Transform _posItPos;
    [SerializeField] bool _hasPostIt = false;

    [SerializeField] WinStateScreen _winStateScreen;

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _onRoomSuccessClip;
    [SerializeField] AudioClip _onRoomFailClip;

    [SerializeField] BoxCollider2D _boxCollider;


    #endregion

    GameManager _gameManager;

    SpriteRenderer _light;


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
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public AudioClip OnRoomSuccessClip { get => _onRoomSuccessClip; set => _onRoomSuccessClip = value; }
    public AudioClip OnRoomFailClip { get => _onRoomFailClip; set => _onRoomFailClip = value; }
    public BoxCollider2D BoxCollider { get => _boxCollider; set => _boxCollider = value; }
    #endregion

    public virtual void InitRoom()
    {
        if(GameManager.Instance != null) _gameManager = GameManager.Instance;
        
        if(_spriteRoom == null) { _spriteRoom = GetComponentInChildren<SpriteRenderer>(); }
        if(!Id.Contains("S")) { _light = gameObject.transform.Find("LightBlack").GetComponent<SpriteRenderer>(); }
        //WinStateScreen.ChangeColor(Color.white);
        //Color.RGBToHSV(_spriteRoom.color, out h, out s, out v);
        if(_light != null)
        {
            _light.color = new Color(0, 0, 0, 0.9f);
        }
        
        //print(gameObject.name + " : " + h + " " + s + " " + v);
        //_spriteRoom.color = Color.HSVToRGB(h, s, 0);

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
        if (_id.Contains("T"))
        {
            _gameManager.RoomTaskList.Add(this);
        }
        
        ///Spawn
        /*if (_id.Contains("S"))
        {

        }*/
    }

    public void OnRoomEnter()
    {
        /*if(_spriteRoom != null)
        {
            _spriteRoom.color = Color.HSVToRGB(h, s, v);
        }*/
        if(_light != null)
        {
            _light.color = new Color(0, 0, 0, 0);
        }
        
    }
    public void OnRoomExit()
    {
        //_sprite.color = Color.HSVToRGB(h, s, 0.1f);

    }
}

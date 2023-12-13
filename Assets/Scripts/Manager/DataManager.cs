using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    Dictionary<string, string> inputsNamesConverterSwitch = new Dictionary<string, string>()
    {
        
        
        {"Y", "X" },
        {"X", "Y" },
        {"B", "A" },
        {"A", "B" },
        {"R", "R1" },
        {"ZR", "R2" },
        {"Right Stick", "R3" },
        {"L", "L1" },
        {"ZL", "L2" },
        {"Left Stick", "L3" },
    };

    Dictionary<string, string> inputsNamesConverterXbox = new Dictionary<string, string>()
    {
        {"X", "X" },
        {"Y", "Y" },
        {"A", "A" },
        {"B", "B" },
        {"Right Bumper", "R1" },
        {"Right Trigger", "R2" },
        {"Right Stick Press", "R3" },
        {"Left Bumper", "L1" },
        {"Left Trigger", "L2" },
        {"Left Stick Press", "L3" },
    };

    Dictionary<string, string> inputsNamesConverterPlaystation = new Dictionary<string, string>()
    {
        {"Square", "X" },
        {"Triangle", "Y" },
        {"Cross", "A" },
        {"Circle", "B" },
        {"R1", "R1" },
        {"R2", "R2" },
        {"R3", "R3" },
        {"L1", "L1" },
        {"L2", "L2" },
        {"L3", "L3" },
    };

    

    [SerializeField] List<GameObject> _allTasks = new List<GameObject>();
    [SerializeField] InputDataManager _inputsData;


    //[SerializeField] List<Sprite> _spritePlayers = new List<Sprite>();
    [SerializeField] List<RuntimeAnimatorController> _animationPlayers = new List<RuntimeAnimatorController>();
    Dictionary<InputDeviceDescription, RuntimeAnimatorController> _dicSpritePlayer = new Dictionary<InputDeviceDescription, RuntimeAnimatorController>();
    public List<GameObject> AllTasks { get => _allTasks; }

    //public List<Sprite> SpritePlayers { get => _spritePlayers; }
    public List<RuntimeAnimatorController> AnimationPlayers { get => _animationPlayers; }
    public Dictionary<InputDeviceDescription, RuntimeAnimatorController> DicSpritePlayer { get => _dicSpritePlayer; set => _dicSpritePlayer = value; }
    public Dictionary<string, string> InputsNamesConverterPlaystation { get => inputsNamesConverterPlaystation; set => inputsNamesConverterPlaystation = value; }
    public Dictionary<string, string> InputsNamesConverterXbox { get => inputsNamesConverterXbox; set => inputsNamesConverterXbox = value; }
    public Dictionary<string, string> InputsNamesConverterSwitch { get => inputsNamesConverterSwitch; set => inputsNamesConverterSwitch = value; }

    Dictionary<SystemLanguage, string> _languageSprite = new Dictionary<SystemLanguage, string>()
    {
        {SystemLanguage.English, "English"},
        {SystemLanguage.French, "French"},
        {SystemLanguage.Spanish, "Spanish"},
        {SystemLanguage.Portuguese, "Portugese"},
        {SystemLanguage.German, "German"}
    };


    [SerializeField] List<Sprite> _flags = new List<Sprite>();

    public enum TaskEnum
    {
        CowboyQTE,
        LaserRoom,
        RHTask,
        Decryptage,
        Duolingo,
        Tamponnage,
        Volley,
        Store,
        Matrix
    }



    private void Start()
    {
        if (DataManager.Instance == null)
        {
            Awake();
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            print("Set Data Instance");
            print(DicSpritePlayer.Count);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            print("Double DATA Manager initiate Destroy");
            Destroy(gameObject);
        }
        
    }

    public Dictionary<string, string> ChoseRightConverterDic(PlayerController player)
    {
        Dictionary<string, string> tempDic = new Dictionary<string, string>();
        switch (player.Type)
        {
            case PlayerManager.ControllerType.None:
                tempDic = inputsNamesConverterPlaystation;
                break;
            case PlayerManager.ControllerType.Playstation:
                tempDic = inputsNamesConverterPlaystation;
                break;
            case PlayerManager.ControllerType.Xbox:
                tempDic = inputsNamesConverterXbox;
                break;
            case PlayerManager.ControllerType.Switch:
                tempDic = inputsNamesConverterSwitch;
                break;
        }
        return tempDic;
    }

    public Sprite FindFlagSprite(SystemLanguage language)
    {
        string name = _languageSprite[language];
        Sprite _sprite = null;
        foreach (Sprite flag in _flags)
        {
            if (flag.name == name)
            {
                _sprite = flag;
            }
        }
        return _sprite;
    }

    public Sprite FindInputSprite(string name, PlayerManager.ControllerType type)
    {
        Sprite inputSprite = null;
        foreach (InputConfig input in _inputsData.inputs)
        {
            if(input.baseName == name)
            {
                switch (type)
                {
                    case PlayerManager.ControllerType.Xbox:
                        inputSprite = input.spriteXbox;
                        break;
                    case PlayerManager.ControllerType.Playstation:
                        inputSprite = input.spritePlaystation;
                        break;
                    case PlayerManager.ControllerType.Switch:
                        inputSprite = input.spriteSwitch;
                        break;
                    case PlayerManager.ControllerType.None:
                        inputSprite = input.spritePlaystation;
                        break;
                }

            }
        }
        return inputSprite;
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
        DestroyImmediate(gameObject);
        //return this.gameObject;
    }
}

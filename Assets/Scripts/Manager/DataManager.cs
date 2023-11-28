using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    Dictionary<string, string> inputsNamesConverter = new Dictionary<string, string>()
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

    public Dictionary<string, string> InputNamesConverter { get => inputsNamesConverter; }

    [SerializeField] List<GameObject> _allTasks = new List<GameObject>();
    [SerializeField] InputDataManager _inputsData;

    //[SerializeField] List<Sprite> _spritePlayers = new List<Sprite>();
    [SerializeField] List<RuntimeAnimatorController> _animationPlayers = new List<RuntimeAnimatorController>();
    Dictionary<InputDeviceDescription, RuntimeAnimatorController> _dicSpritePlayer = new Dictionary<InputDeviceDescription, RuntimeAnimatorController>();
    public List<GameObject> AllTasks { get => _allTasks; }

    //public List<Sprite> SpritePlayers { get => _spritePlayers; }
    public List<RuntimeAnimatorController> AnimationPlayers { get => _animationPlayers; }
    public Dictionary<InputDeviceDescription, RuntimeAnimatorController> DicSpritePlayer { get => _dicSpritePlayer; set => _dicSpritePlayer = value; }

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



    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
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
                    case PlayerManager.ControllerType.None:
                        inputSprite = input.spriteXbox;
                        break;
                }

            }
        }
        return inputSprite;
    }
}

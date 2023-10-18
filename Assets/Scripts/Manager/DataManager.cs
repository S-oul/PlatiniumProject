using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    };

    public Dictionary<string, string> InputNamesConverter { get => inputsNamesConverter; }

    [SerializeField] List<Task> _allTasks = new List<Task>();

    public List<Task> AllTasks { get => _allTasks; }

    public enum TaskEnum
    {
        CowboyQTE
    }

    
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

   


}

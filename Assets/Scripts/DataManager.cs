using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    [Header("QTE")]
    [SerializeField]float[] _timeBetweenInputsQTE = new float[5];
    [SerializeField] int _minQTENumberInputs;
    [SerializeField] int _maxQTENumberInputs;

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

    public float[] TimeBetweenInputsQTE { get => _timeBetweenInputsQTE; }
    public int MinQTENumberInputs { get => _minQTENumberInputs; }
    public int MaxQTENumberInputs { get => _maxQTENumberInputs; }

    public Dictionary<string, string> InputNamesConverter { get => inputsNamesConverter; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
}

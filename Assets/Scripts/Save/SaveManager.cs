using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.SearchService;
using System;
using Unity.VisualScripting.FullSerializer;
using TMPro;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameObject YMII;

    public static SaveManager Instance;

    string _saveFilePath = Path.Combine(Application.dataPath,"Save.txt");
    //"C:\Users\ssoul\Desktop\AllBuild\Alpha 0.4\UnityPlayer.dll"
    
    private void Awake()
    {
        print(_saveFilePath);
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            LoadSave();
        }
        else
        {
            Destroy(this);
        }
    }
    void LoadSave()
    {
        string dataloaded = "";
        if (File.Exists(_saveFilePath))
        {
            using (FileStream stream = new FileStream(_saveFilePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataloaded = reader.ReadToEnd();
                }
            }
            SaveData data = JsonUtility.FromJson<SaveData>(dataloaded);
            if (data.isSucess)
            {
                YMII.GetComponent<TextMeshProUGUI>().text = "You made it in :" + data.DaTime;
            }
            else
            {
                YMII.GetComponent<TextMeshProUGUI>().text = "You Failed in :" + data.DaTime;
            }

        }
        else
        {
            YMII.GetComponent<TextMeshProUGUI>().text = "Play\n'La Flamme' on Itch !";
        }
    }
    void CreateSave()
    {
        if (File.Exists(_saveFilePath)) File.Delete(_saveFilePath);

        SaveData data = new SaveData(Mathf.RoundToInt(GameManager.Instance.DayManager.DayTimer.DayTime), false);
        string jsonedData = JsonUtility.ToJson(data);
        print(Mathf.RoundToInt(GameManager.Instance.DayManager.DayTimer.DayTime) + " " + data + " " + _saveFilePath);
        using(FileStream stream = new FileStream(_saveFilePath, FileMode.Create))
        {
            using(StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(jsonedData);
            }
        }

        
    }
    private void OnApplicationQuit()
    {
        CreateSave();
    }
}

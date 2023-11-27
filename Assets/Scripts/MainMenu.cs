using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string _menuSceneName;
    [SerializeField] string _gameSceneName; 
    public void OpenGameScene()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

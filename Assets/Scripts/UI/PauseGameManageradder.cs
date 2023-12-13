using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameManageradder : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GameManager.Instance.PauseMenu = transform.GetChild(0).gameObject;
    }

    public void Resume()
    {
        GameManager.Instance.SetPause();
    }

    public void Retry()
    {
        Time.timeScale = 1;
        
        Destroy(GameManager.Instance.gameObject);
        Destroy(DataManager.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    public void Menu()
    {
        Time.timeScale = 1;

        GameManager.Instance.DestroySelf();
        AudioManager.Instance.DestroySelf();
        DataManager.Instance.DestroySelf();
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
        print("APP HAS QUITTED");
    }

}

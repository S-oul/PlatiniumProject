using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance;
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Slider _loadingBar;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(index);
        _loadingScreen.SetActive(true);
        while (!loading.isDone)
        {
            float value = Mathf.Clamp01(loading.progress / 0.9f);
            _loadingBar.value = value;
            yield return null;
        }
    }
}
    

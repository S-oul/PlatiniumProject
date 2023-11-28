using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] Transform _UIVolley;

    public Transform UIVolley { get => _UIVolley; set => _UIVolley = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


    }
    void Start()
    {
        UIVolley.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

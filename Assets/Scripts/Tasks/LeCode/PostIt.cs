using TMPro;
using UnityEngine;

public class PostIt : MonoBehaviour
{
    TextMeshPro text;
    string _code;

    public string Code { get => _code; set => _code = value; }
    public string SetCode(string code) { return code[0] + " " + code[1] + " " + code[2] + " " + code[3] + " "; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }
    public void Initialize()
    {
        text.text = SetCode(_code);
    }
}

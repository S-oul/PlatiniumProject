using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    DataManager _DM;
    [SerializeField] protected string _name = "";
    SpriteRenderer _spriteRenderer;
    UINpc _npcUI;

    public UINpc NPCUI { get => _npcUI; }

    private void Start()
    {
        _DM = DataManager.Instance;  
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _npcUI = GetComponent<UINpc>();
    }
}

public interface IChattyNPC
{
    List<string> dialogueTexts { get ; set; }
    void Talk(string text);
}

public interface ITaskNPC
{
    GameObject Task { get;}


    
}

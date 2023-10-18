using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    DataManager _DM;
    [SerializeField] protected string _name = "";
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _DM = DataManager.Instance;  
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }
    
    public abstract void Interact();
}

public interface IChattyNPC
{
    List<string> dialogueTexts { get ; set; }
    void Talk(string text);
}

public interface ITaskNPC
{
    Task task { get;}


    
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string _name = "";
    Sprite _spriteRenderer;
}

public interface IChattyNPC
{
    List<string> dialogueTexts { get ; set; }
    void Talk(string text);
}

public interface ITaskNPC
{
    Task dialogueTexts { get; set; }
    void Talk(string text);
}

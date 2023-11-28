using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    public bool IsUsed = false;
    public abstract void Interact(GameObject player);

}

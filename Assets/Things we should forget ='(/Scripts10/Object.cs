using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour, IPushable
{
    enum Material
    {
        Wood,
        Metal
    }

    [SerializeField] Material material;

    public void Push(Vector2 dir)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Arm")
        {
            if (material == Material.Wood)
            {
                Destroy(gameObject);
            }
        }
        
    }

    void DestroyObject()
    {
        //=> Animation or effects before destruction
        Destroy(gameObject);
    }
}

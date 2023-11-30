using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourPlayer : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    
    void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Flash());
    }
}

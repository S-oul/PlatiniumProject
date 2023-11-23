using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeSpeed = 1f; // Vitesse de fondu

    private CanvasRenderer canvasRenderer;
    public float alpha = 1f; // Transparence initiale

    private void Start()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    private void Update()
    {
        // Réduire progressivement l'alpha
        alpha -= fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha); // S'assurer que l'alpha reste entre 0 et 1
        //print("fading out");
        // Mettre à jour la transparence du carré noir
        canvasRenderer.SetAlpha(alpha);
    }
}

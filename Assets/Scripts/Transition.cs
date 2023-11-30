using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image imageToMove; // Référence à l'image à déplacer
    public Image imageToMove2;
    public float distanceToMove = 100f; // Distance à parcourir
    public float duration = 2f; // Durée du déplacement en secondes
    public bool moveRight = true; // Direction du déplacement (true pour droite, false pour gauche)
    public bool moveRight2 = true;

    private float elapsedTime = 0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 startPosition2;
    private Vector3 endPosition2;

    private bool moving = false;

    public GameObject PanEcriture;
    public GameObject PanEcriture2;

    private void Start()
    {
        // Définir la position de départ et la position d'arrivée en fonction de la direction du déplacement
        startPosition = imageToMove.rectTransform.anchoredPosition;
        endPosition = moveRight ? startPosition + new Vector3(distanceToMove, 0f, 0f) : startPosition - new Vector3(distanceToMove, 0f, 0f);

        startPosition2 = imageToMove2.rectTransform.anchoredPosition;
        endPosition2 = moveRight2 ? startPosition2 + new Vector3(distanceToMove, 0f, 0f) : startPosition2 - new Vector3(distanceToMove, 0f, 0f);
        StartCoroutine(StartingTheGame());
    }

    private void Update()
    {
        if (moving == true)
        {
            // Vérifier si le déplacement n'est pas encore terminé
            if (elapsedTime < duration)
            {
                // Calculer le temps écoulé depuis le début du déplacement
                elapsedTime += Time.deltaTime;

                // Interpoler la position de l'image en utilisant une fonction d'interpolation "ease in"
                float t = elapsedTime / duration;
                t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // Ease in function
                imageToMove.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);
                imageToMove2.rectTransform.anchoredPosition = Vector3.Lerp(startPosition2, endPosition2, t);
            }
        }
    }

    private IEnumerator StartingTheGame ()
    {
        yield return new WaitForSeconds(0.5f);
        PanEcriture.SetActive(true);
        PanEcriture2.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        print("CA MARCHE");
        moving = true;
    }
}

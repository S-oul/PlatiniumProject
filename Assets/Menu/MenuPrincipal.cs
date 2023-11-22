using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    public GameObject CamMenu;
    public GameObject CamOption;
    public GameObject CamCredit;


    public float scrollSpeed = 50f; // Vitesse de défilement du texte
    public RectTransform textTransform; // Référence au RectTransform du texte
    public Text textComponent; // Référence au composant Text

    private bool isScrolling = true; // Indicateur de défilement

    public void PlayGame ()
    {
        SceneManager.LoadScene("Bureaucratie");
    }

    public void Options ()
    {
        CamMenu.SetActive(false);
        CamCredit.SetActive(false);
        CamOption.SetActive(true);
    }

    public void OptionsMoveBack ()
    {
        CamMenu.SetActive(true);
        CamCredit.SetActive(false);
        CamOption.SetActive(false);
    }

    public void Credit ()
    {
        CamMenu.SetActive(false);
        CamCredit.SetActive(true);
        CamOption.SetActive(false);
        StartCoroutine(ScrollText());
        
    }

    public void CreditMoveBack ()
    {
        CamMenu.SetActive(true);
        CamCredit.SetActive(false);
        CamOption.SetActive(false);
        
    }

    private IEnumerator ScrollText()
    {
        // Définir la position initiale du texte à l'extérieur de l'écran
        textTransform.anchoredPosition = new Vector2(0f, -textComponent.preferredHeight);

        // Attendre une seconde avant de commencer le défilement
        yield return new WaitForSeconds(0.5f);

        while (isScrolling)
        {
            // Mettre à jour la position du texte en fonction de la vitesse de défilement
            textTransform.anchoredPosition += new Vector2(0f, scrollSpeed * Time.deltaTime);

            // Vérifier si le texte est sorti de l'écran
            if (textTransform.anchoredPosition.y > Screen.height)
            {
                // Réinitialiser la position du texte à l'extérieur de l'écran
                textTransform.anchoredPosition = new Vector2(0f, -textComponent.preferredHeight);
            }

            yield return null;
        }
    }
}

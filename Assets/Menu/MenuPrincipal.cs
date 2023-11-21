using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    public GameObject CamMenu;
    public GameObject CamOption;


    public float scrollSpeed = 50f; // Vitesse de défilement du texte
    public RectTransform textTransform; // Référence au RectTransform du texte
    public GameObject textComponent; // Référence au composant Text

    private bool isScrolling = true; // Indicateur de défilement

    public void PlayGame ()
    {
        SceneManager.LoadScene("Bureaucratie");
    }

    public void OptionsMovein ()
    {
        CamMenu.SetActive(false);
        CamOption.SetActive(true);
    }

    public void OptionsMoveBack ()
    {
        CamMenu.SetActive(true);
        CamOption.SetActive(false);
    }

    public void Credit ()
    {
        StartCoroutine(ScrollText());
    }

    private IEnumerator ScrollText()
    {
        yield return null;

        /* // Attendre une seconde avant de commencer le défilement
        yield return new WaitForSeconds(1f);

        // Définir la position initiale du texte à l'extérieur de l'écran
        textTransform.anchoredPosition = new Vector2(0f, Screen.height);

        while (isScrolling)
        {
            // Mettre à jour la position du texte en fonction de la vitesse de défilement
            textTransform.anchoredPosition -= new Vector2(0f, scrollSpeed * Time.deltaTime);

            // Vérifier si le texte est sorti de l'écran
            if (textTransform.anchoredPosition.y + textComponent.preferredHeight < 0f)
            {
                // Réinitialiser la position du texte à l'extérieur de l'écran
                textTransform.anchoredPosition = new Vector2(0f, Screen.height);
            }

            yield return null;
        }  */
    }
}

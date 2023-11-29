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
    public GameObject CamPlay;
    public GameObject CamPlay2;


    public float scrollSpeed = 50f; // Vitesse de défilement du texte
    public RectTransform textTransform; // Référence au RectTransform du texte
    public Text textComponent; // Référence au composant Text

    private bool isScrolling = true; // Indicateur de défilement

    public SpriteRenderer FadeScreen;
    public float alpha = 0f;

    public GameObject LinkedObject;

    public void PlayGame ()
    {
        CamMenu.SetActive(false);
        CamPlay.SetActive(true);
        StartCoroutine(LaunchGame());
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

    public void QuitGame ()
    {
        StartCoroutine(CloseGame(false));
    }

    private IEnumerator ScrollText()
    {
        // Définir la position initiale du texte à l'extérieur de l'écran
        textTransform.anchoredPosition = new Vector2(0f, -textComponent.preferredHeight - 80);

        // Attendre une seconde avant de commencer le défilement
        yield return new WaitForSeconds(0.5f);

        while (isScrolling)
        {
            // Mettre à jour la position du texte en fonction de la vitesse de défilement
            textTransform.anchoredPosition += new Vector2(0f, scrollSpeed * Time.deltaTime);

            // Mettre à jour la position du GameObject lié au texte
            LinkedObject.transform.position = textTransform.position;

            // Vérifier si le texte est sorti de l'écran
            if (textTransform.anchoredPosition.y > Screen.height)
            {
                // Réinitialiser la position du texte à l'extérieur de l'écran
                textTransform.anchoredPosition = new Vector2(0f, -textComponent.preferredHeight);
            }

            yield return null;
        }
    }

    private IEnumerator LaunchGame()
    {
        //deplacement des camera
        yield return new WaitForSeconds(0.5f);
        CamPlay.SetActive(false);
        CamPlay2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //Lancer la partie
        SceneManager.LoadScene("Bureaucratie");
    }

    // the image you want to fade, assign in inspector
    public Image img;

    private IEnumerator CloseGame(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);

        Application.Quit();
        //print("ca quitte le jeu");
    }
}

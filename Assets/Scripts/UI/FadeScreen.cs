using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FadeScreen : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image img;
    public float timeFade = 0.5f;

    public void TransitionJeu ()
    {
        StartCoroutine(Fade(false));
    }

    public IEnumerator Fade(bool fadeAway)
    {
        print("ca marche FADE : " + fadeAway);
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

        yield return new WaitForSeconds(timeFade);

        SceneManager.LoadScene(2);
        //print("ca change de scene");
    }
}

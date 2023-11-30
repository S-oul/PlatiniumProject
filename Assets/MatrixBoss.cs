using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatrixBoss : MonoBehaviour
{
    [SerializeField] Image _input;
    [SerializeField] Image _contour;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Animator _animator;

    public void DisplayInput(Sprite image)
    {
        _input.color = new Color32(255, 255, 255, 255);
        _input.sprite = image;
    }

    public void DisplayText(string text)
    {
        _text.text = text;
    }

    public void DisplayColorInput(Color color)
    {
        _contour.color = color;
    }

    public void ClearColorInput()
    {
        _contour.color = new Color32(255, 255, 255, 0);
    }

    public void ClearInput()
    {
        _input.color = new Color32(255, 255, 255, 0);
    }

    public void SetActiveInput(bool value)
    {
        
        _input.gameObject.SetActive(value);
        _contour.gameObject.SetActive(value);
        ClearColorInput();
    }

    public void SetActiveText(bool value)
    {

        _text.gameObject.SetActive(value);
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
    public void LoseAnimation()
    {
        _animator.SetBool("hasLost", true);
    }
    public void HitAnimation()
    {
        _animator.SetTrigger("TakeHit");
    }
}

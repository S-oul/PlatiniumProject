using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixBoss : MonoBehaviour
{
    [SerializeField] Image _input;
    [SerializeField] Animator _animator;

    public void DisplayInput(Sprite image)
    {
        _input.color = new Color32(255, 255, 255, 255);
        _input.sprite = image;
    }

    public void ClearInput()
    {
        _input.color = new Color32(255, 255, 255, 0);
    }

    public void SetActiveInput(bool value)
    {
        
        _input.gameObject.SetActive(value);
    }

    public void AttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
    public void LoseAnimation()
    {
        _animator.SetBool("hasLost", true);
    }

}

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
    [SerializeField] Transform _askForInput;
    [SerializeField] Slider _slider;
    [SerializeField] Transform _lives;
    Image _inputPlayerImage;
    Image _inputPlayerState;
    Image _inputPlayer;
    List<Image> _allLives = new List<Image>();

    /*private void Start()
    {
        foreach (Image live in _lives)
        {
            _allLives.Add(live);
        }
    }*/

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

    public void DisplayInputPlayer(Sprite input, bool isRight, GameObject player)
    {
        /*_playerInput.gameObject.SetActive(true);

        _inputPlayer.sprite = player.transform.Find("Animation").GetComponent<SpriteRenderer>().sprite;
        _inputPlayerImage.sprite = input;
        if (isRight)
        {
            _inputPlayerState.sprite = ;
        }*/
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

    public void SliderValue(float percent)
    {
        _slider.value = percent;
    }

    public void SliderActive(bool value)
    {
        _askForInput.gameObject.SetActive(value);
        _slider.gameObject.SetActive(value);
    }

    /*public void SetPlayerLife(int life)
    {
        
        foreach (Image live in _allLives)
        {
            live.gameObject.SetActive(false);
        }
        if (life > 0)
        {
            for (int i = 0; i < life; i++)
            {
                _allLives[i].gameObject.SetActive(true);
            }
        }
        
        
        
    }*/
}

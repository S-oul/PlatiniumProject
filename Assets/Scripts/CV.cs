using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CV : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController _animationPlayer;
    [SerializeField] CVChoice _cvChoice;
    bool _isChosen;
    [SerializeField] RectTransform _posCursor;
    List<GameObject> _cursorsOnCV = new List<GameObject>();
    Color _lockedColor;
    Color _unlockedColor;
    [SerializeField] Transform _validation;
    [SerializeField] Image _lockedBG;
    public bool IsChosen { get => _isChosen; set => _isChosen = value; }
    public List<GameObject> CursorsOnCV { get => _cursorsOnCV; set => _cursorsOnCV = value; }
    public RuntimeAnimatorController AnimationPlayer { get => _animationPlayer; set => _animationPlayer = value; }

    private void Start()
    {
        _lockedColor = _lockedBG.color;
        
        _unlockedColor = new Color(_lockedColor.r, _lockedColor.g, _lockedColor.b, 0);
        _lockedBG.color = _unlockedColor;
        _validation.gameObject.SetActive(false);
    }
    public void GoOnCV(GameObject cursor)
    {
        
        CursorsOnCV.Add(cursor);
        cursor.transform.SetParent(_posCursor.transform);
        UpdateCVPlayers();
        cursor.GetComponent<CursorPlayer>().CanInteract = true;
    }

    public void ExitCV(GameObject cursor)
    {
        ;
        if (cursor != null)
        {
            CursorsOnCV.Remove(cursor);
            UpdateCVPlayers();
        }
        
        
    }

    void UpdateCVPlayers()
    {
        float width = _posCursor.rect.width;
        width /= CursorsOnCV.Count + 1;
        for (int i = 0; i < CursorsOnCV.Count; i++)
        {
            CursorsOnCV[i].GetComponent<RectTransform>().localPosition = new Vector2((i + 1) * width - _posCursor.rect.width / 2, 0);
        }
    }
    public void LockCV(CursorPlayer player)
    {
        player.CvChosen = this;
        _isChosen = true;
        _validation.gameObject.SetActive(true);
        _lockedBG.color = _lockedColor;
        _validation.Find("Text").GetComponent<TextMeshProUGUI>().text = player.PlayerID;
        _cvChoice.UpdatePlayerChoseCount();
    }

    public void UnlockCV()
    {
        _isChosen = false;
        _validation.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
        _lockedBG.color = _unlockedColor;
        _validation.gameObject.SetActive(false);
        _cvChoice.UpdatePlayerChoseCount();
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


public class CursorPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerName;
    string _playerID;
    bool _isInSpawnZone = false;
    int _posID = 0;
    int _currentCVID = 0;
    int _currentZoneID = 0;
    int _horizontalValue = 0;
    int _verticalValue = 0;
    bool _canInteract;
    CVChoice _cvChoice;
    bool _hasChosenCV = false;
    CV _cvChosen;
    public bool IsInSpawnZone { get => _isInSpawnZone; set => _isInSpawnZone = value; }
    public int CurrentCVID { get => _currentCVID; set => _currentCVID = value; }
    public int CurrentZoneID { get => _currentZoneID; set => _currentZoneID = value; }
    public CVChoice CvChoice { get => _cvChoice; set => _cvChoice = value; }
    public bool CanInteract { get => _canInteract; set => _canInteract = value; }
    public string PlayerID { get => _playerID; set => _playerID = value; }
    public bool HasChosenCV { get => _hasChosenCV; set => _hasChosenCV = value; }
    public CV CvChosen { get => _cvChosen; set => _cvChosen = value; }

    public void ChangePlayerName(string name)
    {
        _playerName.text = name;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (CanInteract && !HasChosenCV)
        {
            
            if (context.performed)
            {
                CanInteract = false;
                _horizontalValue = (int)context.ReadValue<Vector2>().x;
                _verticalValue = (int)context.ReadValue<Vector2>().y;
                CheckValue();
            }
        }
        
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if(CurrentZoneID == 0)
        {
            if (context.performed)
            {
                if (!HasChosenCV)
                {
                    if (!_cvChoice.Cvs[CurrentCVID].IsChosen)
                    {
                        HasChosenCV = true;
                        _cvChoice.Cvs[CurrentCVID].LockCV(this);
                    }
                }
                else if (HasChosenCV)
                {
                    HasChosenCV = false;
                    _cvChoice.Cvs[CurrentCVID].UnlockCV();
                }
            }
        }
    }

    void CheckValue()
    {
        if(_horizontalValue == 0 && _verticalValue == 0) 
        {
            CanInteract = true;
            return; 
        }
        else if(Mathf.Abs(_horizontalValue) > Mathf.Abs(_verticalValue))
        {

            CvChoice.ChangeToCV(_horizontalValue, this);
        }
        else if (Mathf.Abs(_horizontalValue) <= Mathf.Abs(_verticalValue))
        {

            CvChoice.ChangeToZoneBase(_verticalValue, this);
        }
    }
}

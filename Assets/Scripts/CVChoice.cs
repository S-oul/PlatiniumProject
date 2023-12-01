using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CVChoice : MonoBehaviour
{
    [SerializeField] List<CV> _cvs = new List<CV>();
    Transform _zoneBase;
    [SerializeField] PlayerCursorManager _playerCursorManager;
    int _playerChosenCount;
    
    public List<CV> Cvs { get => _cvs; set => _cvs = value; }

    [SerializeField] FadeScreen fadeScreen;

    public void ChangeToCV(int value, CursorPlayer player)
    {
        if(player.CurrentZoneID == 0)
        {
            if (player.CurrentCVID + value < Cvs.Count && player.CurrentCVID + value >= 0)
            {
                print("From CV #" + player.CurrentCVID + " to CV #" + (player.CurrentCVID + value));
                Cvs[player.CurrentCVID].ExitCV(player.gameObject);
                player.CurrentCVID += value;
                Cvs[player.CurrentCVID].GoOnCV(player.gameObject);

            }
        }
        player.CanInteract = true;
    }
        

    public void ChangeToZoneBase(int value, CursorPlayer player)
    {
        if(player.CurrentZoneID + value == 1)
        {
            player.CurrentZoneID += value;
            player.IsInSpawnZone = true;
            player.transform.SetParent(_playerCursorManager.SpawnZone.transform);
            Cvs[player.CurrentCVID].CursorsOnCV.Remove(player.gameObject);
            player.CanInteract = true;
            _playerCursorManager.UpdateBaseZone();
        }
        else if(player.CurrentZoneID + value == 0)
        {
            player.CurrentZoneID += value;
            player.IsInSpawnZone = false;
            player.CurrentCVID = 0;
            Cvs[player.CurrentCVID].GoOnCV(player.gameObject);
        }
        player.CanInteract = true;
    }

    public void UpdatePlayerChoseCount()
    {
        _playerChosenCount = 0;
        foreach (PlayerInput player in _playerCursorManager.Players)
        {
            if (player.gameObject.GetComponent<CursorPlayer>().HasChosenCV)
            {
                _playerChosenCount++;
                if(_playerChosenCount == _playerCursorManager.Players.Count)
                {
                    SwitchToGame();
                }
            }
        }
    }

    void SwitchToGame()
    {
        foreach(PlayerInput player in _playerCursorManager.Players)
        {
            DataManager.Instance.DicSpritePlayer.Add(player.devices[0].description, player.GetComponent<CursorPlayer>().CvChosen.AnimationPlayer);
            
            
        }
        
        StartCoroutine(fadeScreen.Fade(false));
        //print(SceneManager.GetActiveScene().name);
        //GameManager.Instance.StartDay();
    }
}

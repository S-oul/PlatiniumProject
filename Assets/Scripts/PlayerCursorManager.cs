using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursorManager : MonoBehaviour
{
    List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] CVChoice _cvChoice;
    [SerializeField] RectTransform _spawnZone;
    [SerializeField] List<GameObject> _buttons;
    public List<PlayerInput> Players { get => players; set => players = value; }
    public RectTransform SpawnZone { get => _spawnZone; set => _spawnZone = value; }
    private void Start()
    {
        if(AudioManager.instance != null && !AudioManager.instance.Music.isPlaying)
        {
            AudioManager.instance.PlayMusic("MainMenuMusic");
        }
    }
    void OnPlayerJoined(PlayerInput player)
    {
        if(player.devices[0].name == "Keyboard")
        {
            Destroy(player.gameObject);
        }
        else
        {
            players.Add(player);
            player.gameObject.GetComponent<CursorPlayer>().IsInSpawnZone = true;
            player.gameObject.GetComponent<CursorPlayer>().CvChoice = _cvChoice;
            List<float> positionsCursor = new List<float>();
            player.transform.SetParent(SpawnZone.transform);
            UpdateBaseZone();
            player.gameObject.GetComponent<CursorPlayer>().CanInteract = true;
            CheckNameAndID();
            player.gameObject.GetComponent<CursorPlayer>().CurrentZoneID = 1;
        }
            

    }

    void OnPlayerLeft(PlayerInput player) 
    {
        players.Remove(player);
        Destroy(player.gameObject);
        UpdateBaseZone();
        CheckNameAndID();

    }

    void CheckNameAndID()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<CursorPlayer>().PlayerID = "J" + (i + 1);
            players[i].GetComponent<CursorPlayer>().ChangePlayerName(players[i].GetComponent<CursorPlayer>().PlayerID);
        }
    }

    public void UpdateBaseZone()
    {
        
        float width = SpawnZone.rect.width;

        width /= players.Count + 1;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.GetComponent<CursorPlayer>().IsInSpawnZone)
            {
                players[i].GetComponent<RectTransform>().localPosition = new Vector2((i + 1) * width - SpawnZone.rect.width / 2, 0);
            }

        }
        
    }

}

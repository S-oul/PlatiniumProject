using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerCursorManager : MonoBehaviour
{
    List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField] CVChoice _cvChoice;
    [SerializeField] RectTransform _spawnZone;
    [SerializeField] List<GameObject> _buttons;
    [SerializeField] TextMeshProUGUI _playersRemaining;
    public List<PlayerInput> Players { get => players; set => players = value; }
    public RectTransform SpawnZone { get => _spawnZone; set => _spawnZone = value; }
    private void Start()
    {
        if(AudioManager.Instance != null && !AudioManager.Instance.Music.isPlaying)
        {
            AudioManager.Instance.PlayMusic("MainMenuMusic");
        }
        _playersRemaining.text = "4 player(s) left...";
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
            CheckController(player);
        }
        if(players.Count == 4)
        {
            _playersRemaining.gameObject.SetActive(false);
        }
        else
        {
            _playersRemaining.text = (4 - players.Count) + " player(s) left...";
        }
        

    }

    void CheckController(PlayerInput player)
    {
        InputDevice gamepad = player.devices[0];
        if (gamepad is UnityEngine.InputSystem.DualShock.DualShockGamepad)
        {
            print("Play");
        }
        else if (gamepad is UnityEngine.InputSystem.XInput.XInputController)
        {
            print("xbox");
        }
        else if (gamepad is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
        {
            foreach (var item in Gamepad.all)
            {
                if ((item is UnityEngine.InputSystem.XInput.XInputController) && (Mathf.Abs((float)(item.lastUpdateTime - gamepad.lastUpdateTime)) < 0.1f))
                {
                    Debug.Log($"Switch Pro controller detected and a copy of XInput was active at almost the same time. Disabling XInput device. `{gamepad}`; `{item}`");
                    InputSystem.DisableDevice(item);
                }
            }
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
            players[i].GetComponent<CursorPlayer>().PlayerID = "P" + (i + 1);
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

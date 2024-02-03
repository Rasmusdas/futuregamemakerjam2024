using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerSpawnManager : MonoBehaviour
{
    public TextMeshProUGUI[] uiElements;
    public Color[] colors;
    public string[] colorNames;
    public static PlayerSpawnManager Instance;
    public bool started = false;
    public bool finished = false;
    public List<PlayerController> players;
    public Transform[] spawnPoints;
    public TextMeshProUGUI winnerText;
    private int _playersSpawned;

    private void Start()
    {
        players = new List<PlayerController>();
        Instance = this;
    }

    public Vector3 GetStartPosition()
    {
        return spawnPoints[_playersSpawned].transform.position;
    }

    private void Update()
    {
        if (started) GetComponent<PlayerInputManager>().DisableJoining();

        var playersWithHealth = players.FindAll(x => x.health > 0);

        if (playersWithHealth.Count == 1 && started)
        {
            finished = true;
            winnerText.gameObject.SetActive(true);
            winnerText.text = playersWithHealth[0].nameOfColor + " has won!";

            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }

        if (playersWithHealth.Count == 0 && started)
        {
            finished = true;
            winnerText.gameObject.SetActive(true);
            winnerText.text = "Tie!";

            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        players.Add(input.transform.GetComponent<PlayerController>());
        input.GetComponent<PlayerController>().Assign(colors[_playersSpawned],uiElements[_playersSpawned],colorNames[_playersSpawned]);
        uiElements[_playersSpawned].transform.parent.gameObject.SetActive(true);
        _playersSpawned++;
    }
}
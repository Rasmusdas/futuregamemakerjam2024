using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerSpawnManager : MonoBehaviour
{
    public Image[] uiElements;
    public Color[] colors;
    public string[] colorNames;
    public static PlayerSpawnManager Instance;
    public bool started = false;
    public bool finished = false;
    public List<PlayerController> players;
    public Transform[] spawnPoints;
    public TextMeshProUGUI winnerText;
    private int _playersSpawned;
    public GameObject infoText;

    public List<PlayerController> deadPlayers = new List<PlayerController>();

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
        if (started)
        {
            GetComponent<PlayerInputManager>().DisableJoining();
            infoText.SetActive(false);
        }

        var playersWithHealth = players.FindAll(x => x.health > 0);
        var playersWithoutHealth = players.FindAll(x => x.health <= 0);
        
        if (deadPlayers.Count < playersWithoutHealth.Count)
        {
            StartCoroutine(IncreaseCheerSpeed());
            var newDeadPlayers = playersWithoutHealth.FindAll(x => !deadPlayers.Contains(x));

            StartCoroutine(AnnounceDead(newDeadPlayers[0].nameOfColor));
            deadPlayers = playersWithoutHealth;
        }

        if (playersWithHealth.Count == 1 && started)
        {
            finished = true;
            winnerText.gameObject.SetActive(true);
            winnerText.text = playersWithHealth[0].nameOfColor + " has won!";

            Person.BaseCheerSpeed = 2;

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

    private IEnumerator IncreaseCheerSpeed()
    {
        Person.BaseCheerSpeed = 2;
        yield return new WaitForSeconds(3);
        Person.BaseCheerSpeed = 1;
    }

    private IEnumerator AnnounceDead(string color)
    {
        winnerText.text = color + " has died!";
        yield return new WaitForSeconds(5);

        if (!finished)
        {
            winnerText.text = "";
        }
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        players.Add(input.transform.GetComponent<PlayerController>());
        input.GetComponent<PlayerController>().Assign(colors[_playersSpawned],uiElements[_playersSpawned],colorNames[_playersSpawned]);
        uiElements[_playersSpawned].transform.gameObject.SetActive(true);
        _playersSpawned++;
    }
}
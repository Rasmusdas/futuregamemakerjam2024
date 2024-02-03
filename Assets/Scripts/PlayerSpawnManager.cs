using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerSpawnManager : MonoBehaviour
{

    public static PlayerSpawnManager Instance;
    public bool started = false;
    public List<PlayerController> players;
    public Transform[] spawnPoints;

    private int _playersSpawned;

    private void Start()
    {
        players = new List<PlayerController>();
        Instance = this;
    }

    public Vector3 GetStartPosition()
    {
        return spawnPoints[_playersSpawned++].transform.position;
    }

    private void Update()
    {
        if (started) GetComponent<PlayerInputManager>().DisableJoining();
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        players.Add(input.transform.GetComponent<PlayerController>());
    }
}
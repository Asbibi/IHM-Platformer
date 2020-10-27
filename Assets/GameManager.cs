using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject player = null;

    Vector3 spawnPosition = Vector3.right*2;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
        }
    }



    // Respawn Methods
    public static void SetSpawnPoint(Vector3 _spawnPosition)
    {
        if (instance != null)
        {
            instance.spawnPosition = _spawnPosition;
            RespawnPlayer();
        }
        else       
            Debug.LogError("Try accessing null GameManager instance at SetSpawnPoint()");       
    }
    public static void RespawnPlayer()
    {
        if (instance != null)
        {
            instance.player.transform.position = instance.spawnPosition;
        }
        else
            Debug.LogError("Try accessing null GameManager instance at RespawnPlayer()");
    }
}

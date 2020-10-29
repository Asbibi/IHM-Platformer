using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject player = null;
    ScreenShake ScreenShake = null;
    Vector3 spawnPosition = Vector3.right*2;

    bool visualFeedBacks = true;

    private void Awake()
    {
        if (instance != null)
        {
            if (player != null)
                Destroy(player);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
            visualFeedBacks = PlayerPrefs.GetInt("VisualFeedBack") != 0;
            player.GetComponent<PlayerMovement>().UpdateShowVisualFeedBack();
        }
    }



    
    public static bool GetVisualFeedBack()
    {
        return instance != null && instance.visualFeedBacks;
    }
    public static void SetVisualFeedBack(bool _vfb)
    {
        if (instance != null)
        {
            instance.visualFeedBacks = _vfb;
            instance.player.GetComponent<PlayerMovement>().UpdateShowVisualFeedBack();
        }
        else
            Debug.LogError("Try accessing null GameManager instance at SetVisualFeedBack(bool _vfb)");
    }

    public static void SetShakeCamera(ScreenShake _newScreenShake)
    {
        if (instance != null)
        {
            instance.ScreenShake = _newScreenShake;
        }
        else
            Debug.LogError("Try accessing null GameManager instance at SetShakeCamera(ScreenShake _newScreenShake)");
    }
    public static void ShakeScreen(float _duration, float _magnitude)
    {
        if (instance != null)
        {
            if (instance.ScreenShake != null && instance.visualFeedBacks)
            {
                instance.StartCoroutine(instance.ScreenShake.Shake(_duration, _magnitude));
            }
            else
                Debug.LogError("Try accessing null ScreenShake from GameManager instance at ShakeScreen() or visual fb disabled");
        }
        else
            Debug.LogError("Try accessing null GameManager instance at ShakeScreen()");
    }

    public static void SetSpawnPoint(Vector3 _spawnPosition)
    {
        if (instance != null)
        {
            instance.spawnPosition = _spawnPosition;
            instance.SpawnPlayer();
        }
        else       
            Debug.LogError("Try accessing null GameManager instance at SetSpawnPoint()");       
    }
    public static void RespawnPlayer()
    {
        if (instance != null)
        {
            if (instance.visualFeedBacks)
                ShakeScreen(0.2f, 0.8f);
            instance.SpawnPlayer();
        }
        else
            Debug.LogError("Try accessing null GameManager instance at RespawnPlayer()");
    }
    private void SpawnPlayer()
    {
        player.transform.position = instance.spawnPosition;
    }
}

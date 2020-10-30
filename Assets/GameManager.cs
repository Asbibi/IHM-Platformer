using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject player = null;
    [SerializeField] UICircleTransition circleTranstion = null;
    [SerializeField] EventSystem eventSystem = null;
    ScreenShake ScreenShake = null;
    Vector3 spawnPosition = Vector3.right*2;

    bool visualFeedBacks = true;
    bool loadingNextScene = false;

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
            DontDestroyOnLoad(circleTranstion.transform.root.gameObject);
            DontDestroyOnLoad(eventSystem.gameObject);
            visualFeedBacks = PlayerPrefs.GetInt("VisualFeedBack") != 0;
            player.GetComponent<PlayerMovement>().UpdateShowVisualFeedBack();
        }
    }



    public static void LoadScene(int i)
    {
        if (instance != null)
        {
            if (!instance.loadingNextScene)
                instance.StartCoroutine(instance.LoadSceneCoroutine(i));
        }
        else
            Debug.LogError("Try accessing null GameManager instance at LoadScene(int i)");
    }
    IEnumerator LoadSceneCoroutine(int i)
    {
        loadingNextScene = true;
        
        yield return new WaitForEndOfFrame();

        Vector3 _circlePositionStart = Camera.current.WorldToScreenPoint(player.transform.position);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();
        
        yield return new WaitForSeconds(circleTranstion.startDuration);

        Debug.Log("End of transition");
        SceneManager.LoadScene(i);
        yield return null;
        circleTranstion.gameObject.SetActive(false);
        //yield return null;
        circleTranstion.gameObject.SetActive(true);
        circleTranstion.ForceInMiddleOfTransition();

        yield return new WaitForEndOfFrame();

        _circlePositionStart = Camera.current.WorldToScreenPoint(spawnPosition);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();

        yield return new WaitForSeconds(circleTranstion.endDuration);
        loadingNextScene = false;
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

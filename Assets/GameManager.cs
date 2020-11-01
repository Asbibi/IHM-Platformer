using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject player = null;
    [SerializeField] GameObject canvas = null;
    [SerializeField] EventSystem eventSystem = null;
    UICircleTransition circleTranstion;
    PauseMenu pauseSystem;
    ScreenShake ScreenShake;
    Vector3 spawnPosition = Vector3.right*2;

    bool visualFeedBacks = true;
    bool loadingNextScene = false;

    private void Awake()
    {
        if (instance != null)
        {
            if (player != null)
                Destroy(player);
            if (canvas != null)
                Destroy(canvas);
            if (eventSystem != null)
                Destroy(eventSystem.gameObject);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(canvas);
            DontDestroyOnLoad(eventSystem.gameObject);
            circleTranstion = canvas.GetComponentInChildren<UICircleTransition>();

            if (circleTranstion == null)
                Debug.LogError("CircleTransition system not found in canvas given");
            pauseSystem = canvas.GetComponent<PauseMenu>();
            if (pauseSystem == null)
                Debug.LogError("Pause system not found in canvas given");

            visualFeedBacks = PlayerPrefs.GetInt("VisualFeedBack") != 0;
            player.GetComponent<PlayerMovement>().UpdateShowVisualFeedBack();


            circleTranstion.ForceInMiddleOfTransition();
            Vector3 _circlePositionStart = Camera.main.WorldToScreenPoint(spawnPosition);
            _circlePositionStart.z = 0;
            circleTranstion.transform.position = _circlePositionStart;
            circleTranstion.PlayTransition();
        }
    }

    #region Navigation between game and menus
    public static void Pause()
    {
        if (instance != null && instance.loadingNextScene == false)
        {
            instance.pauseSystem.PauseButtonPressed();
        }
        else
            Debug.LogError("Try accessing null GameManager instance at Pause() or during a loading scene");
    }
    public static void LeaveGameToMainMenu()
    {

        if (instance != null && instance.loadingNextScene == false)
        {
            instance.StartCoroutine(instance.LoadMainMenuCoroutine());
        }
        else
            Debug.LogError("Try accessing null GameManager instance at LeaveGameToMainMenu() or during a loading scene");
    }
    IEnumerator LoadMainMenuCoroutine()
    {
        loadingNextScene = true;
        pauseSystem.Resume();

        yield return new WaitForEndOfFrame();
        Vector3 _circlePositionStart = Camera.main.WorldToScreenPoint(player.transform.position);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();
        yield return new WaitForSeconds(circleTranstion.startDuration);
        Debug.Log("End of transition");
        
        SceneManager.LoadScene(0);
        yield return new WaitForEndOfFrame();
        CleanGameManager();
    }
    private void CleanGameManager()
    {
        Destroy(player);
        Destroy(canvas);
        Destroy(eventSystem.gameObject);
        Destroy(gameObject);
    }
    #endregion

    #region Feedbacks
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
    #endregion

    #region Spawn Player / Load Scene methods
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

        Vector3 _circlePositionStart = Camera.main.WorldToScreenPoint(player.transform.position);
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

        _circlePositionStart = Camera.main.WorldToScreenPoint(spawnPosition);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();

        yield return new WaitForSeconds(circleTranstion.endDuration);
        loadingNextScene = false;
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
    #endregion
}

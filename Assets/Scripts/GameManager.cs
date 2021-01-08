using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject player = null;
    [SerializeField] GameObject canvas = null;
    [SerializeField] Camera gameCamera = null;
    [SerializeField] EventSystem eventSystem = null;
    UICircleTransition circleTranstion;
    PauseMenu pauseSystem;
    Wind windManager;    
    ScreenShake screenShake;
    UITimeManager timeManager;
    SerialHandler serialHandler;

    Vector3 spawnPosition = Vector3.right*2;

    bool audioFeedBacks = true;
    bool visualFeedBacks = true;
    bool loadingNextScene = false;
    bool receivedLevelValues = false;

    private void Awake()
    {
        if (instance != null)
        {
            if (player != null)
                Destroy(player);
            if (canvas != null)
                Destroy(canvas);
            if (gameCamera != null)
                Destroy(gameCamera.gameObject);
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
            DontDestroyOnLoad(gameCamera.gameObject);
            DontDestroyOnLoad(eventSystem.gameObject);

            circleTranstion = canvas.GetComponentInChildren<UICircleTransition>();
            if (circleTranstion == null)
                Debug.LogError("CircleTransition system not found in canvas given");
            pauseSystem = canvas.GetComponent<PauseMenu>();
            if (pauseSystem == null)
                Debug.LogError("Pause system not found in canvas given");
            windManager = canvas.GetComponentInChildren<Wind>();
            if (windManager == null)
                Debug.LogError("Wind Manager not found in canvas given");
            screenShake = gameCamera.GetComponent<ScreenShake>();
            if (screenShake == null)
                Debug.LogError("ScreenShake system not found in camera given");
            timeManager = canvas.GetComponentInChildren<UITimeManager>();
            if (timeManager == null)
                Debug.LogError("TimeManager system not found in camera given");

            visualFeedBacks = PlayerPrefs.GetInt("VisualFeedBack") != 0;
            player.GetComponent<PlayerMovement>().UpdateShowVisualFeedBack();


            circleTranstion.ForceInMiddleOfTransition();
            Vector3 _circlePositionStart = Camera.main.WorldToScreenPoint(spawnPosition);
            _circlePositionStart.z = 0;
            circleTranstion.transform.position = _circlePositionStart;
            circleTranstion.PlayTransition();

            serialHandler = GetComponent<SerialHandler>();
            serialHandler.player = player.GetComponent<PlayerMovement>();
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
    public static float EndGame()
    {
        if (instance != null)
        {
            instance.CleanGameManager();
            return instance.timeManager.GetElapsedTime();
        }
        else
        {
            Debug.LogError("Try accessing null GameManager instance at EndGame()");
            return -1;
        }
    }
    #endregion

    #region Feedbacks
    public static bool GetAudioFeedBack()
    {
        return instance != null && instance.audioFeedBacks;
    }
    public static void SetAudioFeedBack(bool _afb)
    {
        if (instance != null)
        {
            instance.audioFeedBacks = _afb;
            instance.player.GetComponent<PlayerMovement>().UpdatePlayAudioFeedBack();
        }
        else
            Debug.LogError("Try accessing null GameManager instance at SetAudioFeedBack(bool _afb)");
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
    public static void ShakeScreen(float _duration, float _magnitude)
    {
        if (instance != null)
        {
            if (instance.screenShake != null && instance.visualFeedBacks)
            {
                instance.StartCoroutine(instance.screenShake.Shake(_duration, _magnitude));
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

        //yield return new WaitForEndOfFrame();

        Vector3 _circlePositionStart = gameCamera.WorldToScreenPoint(player.transform.position);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();

        yield return new WaitForSeconds(circleTranstion.startDuration);

        Debug.Log("End of transition");
        receivedLevelValues = false;
        SceneManager.LoadScene(i);
        yield return null;
        circleTranstion.gameObject.SetActive(false);
        circleTranstion.gameObject.SetActive(true);
        circleTranstion.ForceInMiddleOfTransition();

        while (!receivedLevelValues)
        {
            yield return new WaitForEndOfFrame();
        }

        _circlePositionStart = gameCamera.WorldToScreenPoint(spawnPosition);
        _circlePositionStart.z = 0;
        circleTranstion.transform.position = _circlePositionStart;
        circleTranstion.PlayTransition();

        yield return new WaitForSeconds(circleTranstion.endDuration);
        loadingNextScene = false;
    }


    public static void SetLevelValues(LevelValuesScriptable _levelValues)
    {
        if (instance != null)
        {
            instance.spawnPosition = _levelValues.spawnPosition;
            instance.windManager.SetWind(_levelValues.windVector, instance.player.GetComponent<PlayerMovement>());
            instance.gameCamera.transform.position = _levelValues.cameraPosition;
            instance.gameCamera.orthographicSize = _levelValues.cameraSize;
            instance.SpawnPlayer();
            instance.receivedLevelValues = true;
        }
        else
            Debug.LogError("Try accessing null GameManager instance at SetLevelValues()");
    }
    public static void RespawnPlayer()
    {
        if (instance != null)
        {
            if (instance.visualFeedBacks)
                ShakeScreen(0.2f, 0.8f);
            instance.SpawnPlayer();
            instance.serialHandler.NotifyDeath();
        }
        else
            Debug.LogError("Try accessing null GameManager instance at RespawnPlayer()");
    }
    private void SpawnPlayer()
    {
        player.GetComponent<PlayerMovement>().replacePlayer(instance.spawnPosition);
    }
    #endregion

    public static void SetSlowMotion(bool slowMotion)
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (slowMotion)
            {
                Time.timeScale = 0.3f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}

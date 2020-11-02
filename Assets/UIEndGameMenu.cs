using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGameMenu : MonoBehaviour
{
    [SerializeField] Text timeText = null;
    [SerializeField] GameObject recordText = null;

    private void Awake()
    {
        float _timeFinal = GameManager.EndGame();
        if (_timeFinal <= 0)
            _timeFinal = Mathf.Infinity;
        float _previousRecord = PlayerPrefs.GetFloat("Record");
        if (_previousRecord <= 0)
            _previousRecord = Mathf.Infinity;

        timeText.text = ((int)_timeFinal).ToString() + "s";
        if (_previousRecord > _timeFinal)
        {
            PlayerPrefs.SetFloat("Record", _timeFinal);
            recordText.SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

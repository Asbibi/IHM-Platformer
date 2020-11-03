using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text RecordText = null;
    [SerializeField] GameObject HelpMenu = null;
    [SerializeField] GameObject HelpBackButton = null;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Record") <= 0)
            RecordText.text = "-";
        else
            RecordText.text = ((int)PlayerPrefs.GetFloat("Record")).ToString() + "s";

        if (PlayerPrefs.GetInt("PlayerPrefCreated") == 0)   // First game
        {
            HelpMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(HelpBackButton);
            gameObject.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ResetRecord()
    {
        PlayerPrefs.SetFloat("Record", -1);
        RecordText.text = "-";
    }
}

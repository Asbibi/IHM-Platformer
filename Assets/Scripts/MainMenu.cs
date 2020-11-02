using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Text RecordText;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Record") <= 0)
            RecordText.text = "-";
        else
            RecordText.text = ((int)PlayerPrefs.GetFloat("Record")).ToString() + "s";
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

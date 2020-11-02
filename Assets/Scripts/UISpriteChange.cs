using UnityEngine;
using UnityEngine.UI;

public class UISpriteChange : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField]
    private string playerPref = null;
    private Image sr;



    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<Image>();
        sr.sprite = sprites[PlayerPrefs.GetInt(playerPref)];
    }
}

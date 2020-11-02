using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] private string playerPref = null;
    private SpriteRenderer sr;


    public void SetSprite(int _value)
    {        
        sr.sprite = sprites[_value];
    }


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();   
        sr.sprite = sprites[PlayerPrefs.GetInt(playerPref)];
    }
}

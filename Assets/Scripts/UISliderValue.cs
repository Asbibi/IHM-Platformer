using UnityEngine.UI;
using UnityEngine;

public class UISliderValue : MonoBehaviour
{
    Text text;


    void Start()
    {
        text = GetComponent<Text>();
    }

    public void ShowSliderValue(float _value)
    {
        text.text = _value.ToString();
    }
}

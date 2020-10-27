using UnityEngine.UI;
using UnityEngine;

public class UISliderValue : MonoBehaviour
{
    public void ShowSliderValue(float _value)
    {
        GetComponent<Text>().text = _value.ToString();
    }
}

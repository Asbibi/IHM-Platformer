using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConvertSliderToInvert : MonoBehaviour
{
    [SerializeField] UISliderValue textValue = null;
    [SerializeField] UIParameterManager uiParameterManager = null;
    [SerializeField] Slider slider = null;
    private float convertedValue;

    public void ConvertValue(float _rawValue)
    {
        float _rawAbs = Mathf.Abs(_rawValue);
        if (_rawValue >= 0)
            convertedValue = _rawValue + 1;
        else
            convertedValue = 1 / (_rawAbs+1);
        textValue.ShowSliderValue(convertedValue);
        uiParameterManager.SetWallAirControl(convertedValue);
    }
    public float GetConvertedValue()
    {
        return convertedValue;
    }
    public void SetConvertedValue(float _newValue)
    {
        convertedValue = _newValue;
        textValue.ShowSliderValue(convertedValue);
        float _rawValue;
        if (_newValue < 1)
        {
            _rawValue = -1 * ((1 / _newValue) - 1);
        }
        else
        {
            _rawValue = _newValue - 1;
        }

        slider.value = _rawValue;
    }
}

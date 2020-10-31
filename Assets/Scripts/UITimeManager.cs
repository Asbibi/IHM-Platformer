using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeManager : MonoBehaviour
{
    private Text chrono;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        chrono = gameObject.GetComponent<Text>();        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        chrono.text = ((int)elapsedTime).ToString();
    }
}

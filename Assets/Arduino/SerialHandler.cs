using System;
using System.Collections;
using System.IO.Ports;
using UnityEngine;

public class SerialHandler : MonoBehaviour
{
    private SerialPort _serial;

    // Common default serial device on a Linux machine
    [SerializeField] private string serialPort = "COM4"; //" / dev/ttyACM0";
    [SerializeField] private int baudrate = 115200;
    
    public PlayerMovement player;
    bool previousGrounded = false;
    

    void Start()
    {   
        _serial = new SerialPort(serialPort, baudrate);
        _serial.Open();
    }


    void Update()
    {
        if (player != null)
        {
            NotifyGrounded(player.GetGrounded());



            // Prevent blocking if no message is available as we are not doing anything else
            // Alternative solutions : set a timeout, read messages in another thread, coroutines, futures...
            if (_serial.BytesToRead <= 0) return;

            var message = _serial.ReadLine();

            // Arduino sends "\r\n" with println, ReadLine() removes Environment.NewLine which will not be 
            // enough on Linux/MacOS.
            if (Environment.NewLine == "\n")
            {
                message = message.Trim('\r');
            }

            
            string[] words = message.Split('_');

            switch (words[0])
            {
                case "G":
                    Debug.Log("Gravity received with " + words[1]);
                    player.SetGravityMultiplier((float)(int.Parse(words[1]) - 430) / 860);
                    break;
                case "SL":
                    Debug.Log("SlowMotion received");
                    GameManager.SetSlowMotion(true);
                    break;
                case "NSL":
                    Debug.Log("UnSlowMotion received");
                    GameManager.SetSlowMotion(false);
                    break;
            }
        }
    }


    
    private void OnDestroy()
    {
        _serial.Close();
    }




    public void NotifyDeath()
    {
        Debug.Log("Death notified to Arduino");
        //StartCoroutine(notifyDeath());
        _serial.WriteLine("D");
    }
    IEnumerator notifyDeath()
    {
        for (int i=0; i<60; i++)
        {
            _serial.WriteLine("D");
            yield return null;
        }
    }

    public void NotifyGrounded(bool newState)
    {
        if (previousGrounded != newState)
        {
            _serial.WriteLine(newState ? "GRD_ON" : "GRD_OFF");
            previousGrounded = newState;
        }
    }
}

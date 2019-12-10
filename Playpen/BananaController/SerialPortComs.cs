using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class SerialPortComs : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM6", 9600);

    // Start is called before the first frame update
    void Start()
    {
        stream.Open();
        stream.ReadTimeout = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (stream.IsOpen)
        {
            try
            {
                int readValue = (int)stream.ReadByte();
            }
            catch (System.Exception)
            {
                //handle errors
            }
            stream.BaseStream.Flush(); //Clear the serial information to ensure we get new information.
        }
    }

    void useBananaInput(int value_sent_by_arduino)
    {
        //this is where you put code that makes use of the input
        Debug.Log("read value is: " + value_sent_by_arduino);
    }
}

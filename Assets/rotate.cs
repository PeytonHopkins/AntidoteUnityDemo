using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using UnityEditor;

public class rotate : MonoBehaviour
{
    //private SerialPort port;
    public float rotSpeed = 10f;

    public float deadZone = 0.01f;
    private float w, x, y, z, lw, lx, ly, lz, dw, dx, dy, dz;
    private string[] variables = new string[4];

    private Quaternion desiredQuat;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        /*
        port = new SerialPort("COM4", 9600, Parity.None);
        port.ReadTimeout = 80;
        port.DtrEnable = true;
        port.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
        port.Open();
        */
    }


    void OnMessageArrived(string msg)
    {
        //Debug.Log("Message arrived: " + msg);

        variables = msg.Split('\t');

        w = float.Parse(variables[0]);
        x = float.Parse(variables[1]);
        y = float.Parse(variables[2]);
        z = float.Parse(variables[3]);

        dw = w - lw;
        dx = x - lx;
        dy = y - ly;
        dz = z - lz;

        if (Math.Abs(dw) < deadZone)
        {
            w = lw;
        }
        if (Math.Abs(dx) < deadZone)
        {
            x = lx;
        }
        if (Math.Abs(dy) < deadZone)
        {
            y = ly;
        }
        if (Math.Abs(dz) < deadZone)
        {
            z = lz;
        }

        desiredQuat = new Quaternion(-y, z, x, w);
        // = new Quaternion(x,y,z,w);

        lw = w;
        lx = x;
        ly = y;
        lz = z;

        //Debug.Log($"{dw} {dx} {dy} {dz}");
    }

    private void Update()
    {
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, desiredQuat, 10 * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Math.Abs(Input.mouseScrollDelta.y) > 0)
        {
            cam.transform.localPosition  = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z + (float)(Input.mouseScrollDelta.y * 0.25));
        }
        
    }

    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}

﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;

public class Rotate : MonoBehaviour {

    public string portName = "COM3";
    public int baudRate = 115200;
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;
    private SerialPort sp = null;

    private Thread processThread;
    private Thread receiveThread;

    private List<byte> buffer = new List<byte>(4096);

    private Queue<PacketData> dataQueue = new Queue<PacketData>();
    

    public static Quaternion quat = new Quaternion();
    public static PacketData packetData = new PacketData();


	// Use this for initialization
	void Start () {

        //Quaternion qq = GameObject.Find("EthanLeftLeg").transform.rotation;
        //Debug.Log("start: "+qq.w.ToString()+" " + qq.x.ToString()+" " + qq.y.ToString()+" " + qq.z.ToString());
        //Debug.Log("start: " + qq.eulerAngles.ToString());

        OpenPort();

        receiveThread = new Thread(new ThreadStart(receiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        processThread = new Thread(new ThreadStart(processData));
        processThread.IsBackground = true;
        processThread.Start();
	}

    public void OpenPort()
    {
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 500;
        try
        {
            sp.Open();
            sp.DiscardInBuffer();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void ClosePort()
    {
        try
        {
            sp.Close();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// 接收数据线程
    /// </summary>
    protected void receiveData()
    {
        byte[] dataBytes = new byte[1024];
        int bytesToRead = 0;

        while (true)
        {
            if (null != sp && sp.IsOpen)
            {
                try
                {
                    bytesToRead = sp.Read(dataBytes, 0, dataBytes.Length);
                    lock (buffer) 
                    { 
                        for (int i = 0; i < bytesToRead; i++)
                        {
                            buffer.Add(dataBytes[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// 数据处理线程，从串口接收数据函数的buffer中读出数据并处理
    /// </summary>
    protected void processData()
    {
        while (true)
        {
            if (buffer.Count > 24)
            {
                if (buffer[0] == 0x80 && (buffer[1] == 0x0A || buffer[1]==0x0B || buffer[1]==0x0C || buffer[1]==0x0D))
                {
                    PacketData tempPacket = new PacketData();
                    for (int i = 0; i < 24; i++)
                    {
                        tempPacket.data[i] = buffer[i];
                    }

                    lock (dataQueue)
                    {
                        dataQueue.Enqueue(tempPacket);
                    }
                    
                    lock (buffer) 
                    { 
                        buffer.RemoveRange(0, 23);
                    }
                }
                else
                {
                    lock (buffer) 
                    { 
                        buffer.RemoveAt(0);
                    }
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        ClosePort();
        buffer.Clear();
        dataQueue.Clear();
        buffer = null;
        dataQueue = null;
        GC.Collect();
    }

    private void NormalizeQuat(ref Quaternion q)//归一化
    {
	    float N = q.x*q.x + q.y*q.y + q.z*q.z + q.w*q.w;
	    if (N == 1.0f)
		    return;

	    N = Mathf.Sqrt(N);
	    // Too close to zero.
	    if (N < 0.000001f)
		    return;

	    N = 1.0f / N;
	    q.x *= N;
	    q.y *= N;
	    q.z *= N;
	    q.w *= N;
    }

    private void InverseQuat(ref Quaternion q)
    {
        q.x = -q.x;
        q.y = -q.y;
        q.z = -q.z;
    }


	void FixedUpdate ()
    {
        if (dataQueue.Count > 0)
        {
            //Debug.Log(dataQueue.Count);

            lock (dataQueue)
            {
                packetData = dataQueue.Dequeue();
            }
            ProcessNode(ref packetData);
            
        }
	}

    private void ProcessNode(ref PacketData p)
    {
        Quaternion q = new Quaternion();
        q.w = FixedToFloat(Combine(p.data[4], p.data[5]));
        q.x = FixedToFloat(Combine(p.data[6], p.data[7]));
        q.y = -FixedToFloat(Combine(p.data[10], p.data[11]));
        q.z = FixedToFloat(Combine(p.data[8], p.data[9]));

        NormalizeQuat(ref q);

        //Debug.Log("process: " + q.w.ToString() + " " + q.x.ToString() + " " + q.y.ToString() + " " + q.z.ToString());
        //Debug.Log("process: " + q.eulerAngles.ToString());
        //InverseQuat(ref q);


        switch (p.data[1])
        {
            case 0x0A:
                GameObject.Find("EthanLeftUpLeg").transform.rotation = q;
                break;

            case 0x0B:
                GameObject.Find("EthanLeftLeg").transform.rotation = q;
                break;

            case 0x0C:
                GameObject.Find("EthanLeftLeg").transform.rotation = q;
                break;

            case 0x0D:
                GameObject.Find("EthanRightLeg").transform.rotation = q;
                break;

            default:
                Debug.Log(packetData.data[1].ToString() + "no such node");
                break;
        }
    }

    /// <summary>
    /// 定点转浮点
    /// </summary>
    /// <param name="fixedValue">short型定点数</param>
    /// <returns>浮点型数</returns>
    private static float FixedToFloat(short fixedValue)
    {
        return ((float)(fixedValue) / (float)(1 << ((int)15)));
    }

    /// <summary>
    /// 将两个字节连起来组成16位short型数据
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static short Combine(byte a, byte b)
    {
        return (short)((short)((short)a << 8) | (short)b);
    }
}

public class PacketData
{
    public byte[] data = new byte[24];
}
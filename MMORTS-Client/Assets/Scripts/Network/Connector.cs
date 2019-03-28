using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Connector
{
    private static int port = 31337;
    private static string serverAddress = "127.0.0.1";
    private static Socket socket;
    private static byte[] _recieveBuffer = new byte[8142];

    public static bool IsConnected
    {
        get
        {
            if (socket != null)
                return socket.Connected;
            else
                return false;
        }
    }

    public static void ConnectToServer()
    {
        if (IsConnected)
        {
            CloseConnection();
        }
        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(ipPoint);
            Debug.Log("connected with: " + serverAddress.ToString()+":" + port);
            socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);    
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    public static void CloseConnection()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        Debug.Log("connection with: " + serverAddress.ToString() + ":" + port + " closed.");
    }

    public static void SendMessage(string message)
    {
        SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
        byte[] data = Encoding.UTF8.GetBytes(message);
        socketAsyncData.SetBuffer(data,0, data.Length);
        Debug.Log("message sent: " + message);
        socket.SendAsync(socketAsyncData);
    }

    private static void ReceiveCallback(IAsyncResult AR)
    {
        int recieved = socket.EndReceive(AR);

        if (recieved <= 0)
            return;

        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);

        StringBuilder builder = new StringBuilder();
        builder.Append(Encoding.UTF8.GetString(recData));
        //Dictionary<string, System.Object> values = JsonConvert.DeserializeObject<Dictionary<string, System.Object>>(builder.ToString());
        //List<string> values = JsonConvert.DeserializeObject<List<string>>(builder.ToString());
        Debug.Log("server answered: " + builder.ToString());
        
        socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }
}

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
    private static int port;
    private static string serverAddress;
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
        serverAddress = Config.ServerAdress;
        port = Config.Port;
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
        processAnswer(builder.ToString());

        socket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    private static void processAnswer(string answer)
    {
        bool print = true;
        Dictionary<string, object> answerD = new Dictionary<string, object>();
        try
        {
            answerD = JsonConvert.DeserializeObject<Dictionary<string, object>>(answer);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return;
        }
        if (answerD.ContainsKey("c"))
        {
            if (answerD.ContainsKey("e"))
            {
                Debug.Log(answerD["e"]);
            }
            else
            {
                if (answerD.ContainsKey("r"))
                {
                    switch (answerD["c"])
                    {
                        case "login":
                            if (OnAnswerRecieve!=null)
                            {
                                OnAnswerRecieve((string)answerD["r"]);
                            }
                            break;
                        case "map":
                            if (OnUnitsUpdate!=null)
                            {
                                OnUnitsUpdate(answerD["r"]);
                                print = true;
                            }
                            break;
                        case "get_unit_types":
                            if (OnUnitsTypesUpdate != null)
                            {
                                OnUnitsTypesUpdate(answerD["r"]);
                            }
                            break;
                        default:
                            Debug.Log("Unrecognised command");
                            break;
                    }
                }
                else
                {
                    switch (answerD["c"])
                    {
                        case "spawn_unit":
                            break;
                        default:
                            Debug.Log("Not contains an answer");
                            break;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Not a command answer");
        }
        if (print)
        {
            Debug.Log("server answered: " + answer);
        }
        return;
    }
    public delegate void AnswerRecieved(string anwser);
    public static event AnswerRecieved OnAnswerRecieve;

    public delegate void UnitsUpdated(object anwser);
    public static event UnitsUpdated OnUnitsUpdate;

    public delegate void UnitsTypesUpdated(object anwser);
    public static event UnitsTypesUpdated OnUnitsTypesUpdate;

    public delegate void SuccessRecieved();
    public static event SuccessRecieved OnSuccessRecieve;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;
using UnityEditor;

public class Client : MonoBehaviour
{


    public string host;
    public int port;
    //public Manager manager;

    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private bool stop = false;
    public APIManager apiManager = new APIManager();

    public void Connect()
    {
        client = new TcpClient(host, port);
        stream = client.GetStream();
        receiveThread = new Thread(ReceiveMessage);
        receiveThread.Start();
    }

    private void ReceiveMessage()
    {

        while (!stop)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            StringBuilder stringBuilder = new StringBuilder();

            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                stringBuilder.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } while (stream.DataAvailable);

            if (stringBuilder.Length > 0)
            {
                string json = stringBuilder.ToString();

                RequestMessage message = RequestMessage.FromJson(json);

                CallAPI(message);
            }
        }
    }

    private void CallAPI(RequestMessage message)
    {
        apiManager.Call(message.api, message.parameter);
    }

    public void ShutdownSocket(string parameter)
    {
        stop = true;
        receiveThread.Join();
        client.Close();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void Send(ResponseMessage response)
    {
        //NetworkStream stream = client.GetStream();
        byte[] data = Encoding.UTF8.GetBytes(response.ToJson());
        stream.Write(data, 0, data.Length);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Action<string> checkAction = Check;
        //apiManager.Register("check", checkAction);
        Connect();
        Action<string> shutdown = ShutdownSocket;
        apiManager.Register("shutdown", shutdown);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
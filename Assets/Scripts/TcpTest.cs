using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class TcpTest : MonoBehaviour
{
    [SerializeField]
    private string ip;
    [SerializeField]
    private int port;

    public InputField inputMsg;
    public Button SendBtn;
    public Text textBox;

    private StreamReader reader;
    private StreamWriter writer;

    private Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        SendBtn.onClick.AddListener(SendMessage);
    }

    void Update()
    {
        while(messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            textBox.text += message + "\n";
        }
    }

    public void Server()
    {
        Thread thread = new Thread(Server_open);
        thread.IsBackground = true;
        thread.Start();
    }

    void Server_open()
    {
        TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
        server.Start();
        print("server open");

        TcpClient client = server.AcceptTcpClient();
        reader = new StreamReader(client.GetStream());
        writer = new StreamWriter(client.GetStream());
        writer.AutoFlush = true;

        while (client.Connected)
        {
            string readdata = reader.ReadLine();
            if (!string.IsNullOrEmpty(readdata))
            {
                lock (messageQueue)
                {
                    messageQueue.Enqueue($"Client: {readdata}");
                }
            }
        }
    }

    public void Client()
    {
        Thread thread = new Thread(Client_connect);
        thread.IsBackground = true;
        thread.Start();
    }

    void Client_connect()
    {
        TcpClient client = new TcpClient(ip, port);
        print("client connect");

        reader = new StreamReader(client.GetStream());
        writer = new StreamWriter(client.GetStream());
        writer.AutoFlush = true;

        while (client.Connected)
        {
            string readdata = reader.ReadLine();
            if (!string.IsNullOrEmpty(readdata))
            {
                lock (messageQueue)
                {
                    messageQueue.Enqueue($"Server: {readdata}");
                }
            }
        }
    }

    public void SendMessage()
    {
        if (writer != null && !string.IsNullOrEmpty(inputMsg.text))
        {
            string message = inputMsg.text;
            writer.WriteLine(message);

            lock (messageQueue)
            {
                messageQueue.Enqueue($"Me: {message}");
            }

            inputMsg.text = string.Empty;
        }
        else
        {
            print("Message is null");
        }
    }
}

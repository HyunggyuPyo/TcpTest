using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TcpStart : MonoBehaviour
{
    public Button serverBtn;
    public Button clientBtn;

    private void Start()
    {
        serverBtn.onClick.AddListener(ServerScene);
        clientBtn.onClick.AddListener(ClientScene);
    }

    public void ServerScene()
    {
        SceneManager.LoadScene("TCP_Server");
    }

    public void ClientScene()
    {
        SceneManager.LoadScene("TCP_Client");
    }
}

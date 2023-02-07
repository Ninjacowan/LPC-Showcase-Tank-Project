using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class networkUI : MonoBehaviour
{
    NetworkManager networkManager;
    public Button hostButton;
    public Button joinButton;
    public Text ipInput;
    public bool debugging;
    
    // Start is called before the first frame update
    void Start()
    { 
        networkManager = GetComponent<NetworkManager>();
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);
    }
    void Update()
    {
        if (hostButton == null)
        {

            try
            {
                hostButton = GameObject.Find("hostButton").GetComponent<Button>();
                hostButton.onClick.AddListener(Host);
            }
            catch
            {
                //Debug.Log("'hostButton' not found!");
            }

        }
        if (joinButton == null)
        {
            try
            {
                joinButton = GameObject.Find("joinButton").GetComponent<Button>();
                joinButton.onClick.AddListener(Join);
            }
            catch
            {
                //Debug.Log("'joinButton' not found!");
            }

        }
        if (ipInput == null)
        {
            try
            {
                ipInput = GameObject.Find("input text").GetComponent<Text>();
            }
            catch
            {
                //Debug.Log("'input text' not found!");
            }
        }
    }
    void Host()
    {
        
        bool noConnection = (networkManager.client == null || networkManager.client.connection == null ||
               networkManager.client.connection.connectionId == -1);
        Debug.Log(noConnection);
        if (!networkManager.IsClientConnected() && !NetworkServer.active && networkManager.matchMaker == null)
        {
            if (noConnection)
            {
                networkManager.StartHost();
            }
        }
        
    }
    void Join()
    {
        if (debugging)
        {
            Debug.Log("join button");
        }
        try
        {
            networkManager.networkAddress=ipInput.text;
            networkManager.StartClient();
        }
        catch (Exception a)
        {

        }
    }
}

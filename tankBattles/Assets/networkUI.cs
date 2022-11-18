using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class networkUI : MonoBehaviour
{
    NetworkManager networkManager;
    public Button hostButton;
    public Button joinButton;
    public bool debuging;
    
    // Start is called before the first frame update
    void Start()
    {
   if(hostButton == null)
        {
            try
            {
                hostButton = GameObject.Find("hostButton").GetComponent<Button>();
            }
            catch
            { }
            
        }
        else
        {
            
        }
        if(joinButton == null)
        {
            try
            {
                joinButton = GameObject.Find("joinButton").GetComponent<Button>();
            }
            catch { }
        }
        else
        {
            
        }
        DontDestroyOnLoad(joinButton);
        DontDestroyOnLoad(hostButton);
        joinButton.onClick.AddListener(Join);
        hostButton.onClick.AddListener(Host);
        networkManager = GetComponent<NetworkManager>();

    }

    // Update is called once per frame
    
    void Update()
    {
        
     
    }
    void Host()
    {
       
        bool noConnection = (networkManager.client == null || networkManager.client.connection == null ||
               networkManager.client.connection.connectionId == -1);

        if (!networkManager.IsClientConnected() && !NetworkServer.active && networkManager.matchMaker == null)
        {
            if (noConnection)
            {


                networkManager.maxConnections = 2;
              
                    networkManager.StartHost();
              
            }
        }
        
    }
    void Join()
    {
        if (debuging)
        {
            Debug.Log("join button");
        }
        try
        {
            networkManager.StartClient();
        }
        catch (Exception a)
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InstantHost : MonoBehaviour
{
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>(true);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

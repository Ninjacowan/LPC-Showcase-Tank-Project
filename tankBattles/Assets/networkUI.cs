using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class networkUI : MonoBehaviour
{
    NetworkManager networkManager;
    public Button hostButton;
    public Button joinButton;
    public bool debuging;
    
    // Start is called before the first frame update
    void Start()
    {
        hostButton.Equals(GameObject.Find("hostButton"));
        hostButton.onClick.AddListener(Host);
        joinButton.Equals(GameObject.Find("joinButton"));
        joinButton.onClick.AddListener(Join);

        networkManager = GetComponent<NetworkManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Host()
    {
        if (debuging)
        {
            Debug.Log("host button");
        }
        networkManager.maxConnections = 2;
        networkManager.StartHost();
    }
    void Join()
    {
        if (debuging)
        {
            Debug.Log("join button");
        }
        networkManager.StartClient();
    }
}

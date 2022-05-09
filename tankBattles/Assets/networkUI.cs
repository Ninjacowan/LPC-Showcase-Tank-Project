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
    public text idInput;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<NetworkManager>();
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Host()
    {
        networkManager.maxConnections = 2;
        networkManager.StartHost();
    }
    void Join()
    {
        networkManager.networkAddress = idInput.text;
        networkManager.StartClient();
    }
}

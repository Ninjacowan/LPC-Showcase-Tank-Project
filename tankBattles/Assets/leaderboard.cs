using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Sirenix.OdinInspector;
public class leaderboard : NetworkBehaviour
{
    public gameManager gameManager;
    public GameObject leaderRect;
    public Text redKillCounter;
    public Text blueKillCounter;
    public Text greenKillCounter;

    [ReadOnly] public bool IsServer;
    [ReadOnly] public bool IsClient;
    [ReadOnly] public bool IsLocalPlayer;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<gameManager>(true);
        blueKillCounter.gameObject.SetActive(false);
        greenKillCounter.gameObject.SetActive(false);
        leaderRect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        IsServer = isServer;
        IsClient = isClient;
        IsLocalPlayer = isLocalPlayer;
        if (gameManager.playerCount > 1)
        {
            blueKillCounter.gameObject.SetActive(true);
        }
        else if(gameManager.playerCount > 2)
        {
            greenKillCounter.gameObject.SetActive(true);
        }

        redKillCounter.text = "Red Player - " + gameManager.redKills;
        blueKillCounter.text = "Blue Player - "+ gameManager.blueKills;
        greenKillCounter.text = "Green Player - "+gameManager.greenKills;

        if (isServer)
        {
            RpcCounter();
        }
        else
        {
            CmdCounter();
        }
        if(gameManager.redKills > gameManager.blueKills && gameManager.redKills > gameManager.greenKills)
        {
            leaderRect.SetActive(true);
            leaderRect.transform.position = new Vector2(1758, redKillCounter.transform.position.y+16);
        }
        if (gameManager.blueKills > gameManager.greenKills && gameManager.blueKills > gameManager.redKills)
        {
            leaderRect.SetActive(true);
            leaderRect.transform.position = new Vector2(1758, blueKillCounter.transform.position.y+16);
        }
        if (gameManager.greenKills > gameManager.blueKills && gameManager.greenKills > gameManager.blueKills)
        {
            leaderRect.SetActive(true);
            leaderRect.transform.position = new Vector2(1758, greenKillCounter.transform.position.y+16);
        }
        else
        {
            leaderRect.SetActive(false);
        }
    }
    [Command]
    void CmdCounter()
    {
        RpcCounter();
    }
    [ClientRpc]
    void RpcCounter()
    {
        redKillCounter.text = "Red Player - " + gameManager.redKills;
        blueKillCounter.text = "Blue Player - " + gameManager.blueKills;
        greenKillCounter.text = "Green Player - " + gameManager.greenKills;
    }
}

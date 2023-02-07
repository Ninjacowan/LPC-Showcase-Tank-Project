using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;

public class KillCounter : NetworkBehaviour
{
    [ReadOnly] public gameManager gameManager;
    [ReadOnly] public bool IsServer;
    [ReadOnly] public bool IsClient;
    [ReadOnly] public bool IsLocalPlayer;

    void Start()
    {
        gameManager = FindObjectOfType<gameManager>();
    }
    void Update()
    {
        IsServer = isServer;
        IsClient = isClient;
        IsLocalPlayer = isLocalPlayer;
    }
    public void UpdateKills(int r, int g, int b)
    {
        if (isServer)
        {
            RpcClientUpdateKills(r, g, b);
            Debug.Log("RPC Method");
        }
        else
        {
            CmdUpdateKills(r, g, b);
            Debug.Log("Command Method");
        }
    }

    [Command]
    public void CmdUpdateKills(int r, int g, int b)
    {
        gameManager.UpdateKillCounter(r, g, b);
    }
    [ClientRpc]
    public void RpcClientUpdateKills(int r, int g, int b)
    {
        gameManager.UpdateKillCounter(r, g, b);
    }
}

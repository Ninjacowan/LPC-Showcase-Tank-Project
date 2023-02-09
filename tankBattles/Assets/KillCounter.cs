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
        gameManager = FindObjectOfType<gameManager>(true);
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
    private void CmdUpdateKills(int r, int g, int b)
    {
        gameManager.UpdateKillCounter(r, g, b);
    }
    [ClientRpc]
    private void RpcClientUpdateKills(int r, int g, int b)
    {
        gameManager.UpdateKillCounter(r, g, b);
    }
    public bool AddDamage(string color, int damage)
    {
        if (isServer)
        {
            RpcAddDamage(color, damage);
        }
        else
        {
            CmdAddKills(color, damage);
        }
        return false;
    }
    //ADD RETURNS
    [Command]
    private void CmdAddKills(string color, int damage)
    {
        gameManager.AddDamage(color, damage);
    }
    [ClientRpc]
    private void RpcAddDamage(string color, int damage)
    {
        gameManager.AddDamage(color, damage);
    }
}

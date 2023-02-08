using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class gameManager : NetworkBehaviour
{
    public NetworkManager networkManager;
    [InfoBox(" Remember to add NetworkStartPosition components to all new Spawnpoints")]
    public List<Transform> spawnPoints;
    public List<Material> colors;
    [ReadOnly] public int playerCount = 0;
    [ReadOnly] public List<tankControlOverhaul> players;
    [Range(0, 100), SyncVar, OnValueChanged("RpcUpdateRedHealth")] public float redHealth = 100;
    [Range(0, 100), SyncVar, OnValueChanged("RpcUpdateGreenHealth")] public float greenHealth = 100;
    [Range(0, 100), SyncVar, OnValueChanged("RpcUpdateBlueHealth")] public float blueHealth = 100;
    [SyncVar] public int redKills = 0;
    [SyncVar] public int greenKills = 0;
    [SyncVar] public int blueKills = 0;
    void OnAwake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerCount=players.Count;
    }

    public void UpdateKillCounter(int r, int g, int b)
    {
        redKills = redKills + r;
        greenKills = greenKills + g;
        blueKills = blueKills + b;
    }
    public bool AddDamage(string color, int damage)
    {
        if (color == "Flag_Red")
        {
            redHealth -= damage;
            return redHealth == 0;
        }
        else if (color == "Flag_Green")
        {
            greenHealth -= damage;
            return greenHealth == 0;
        }
        else if (color == "Flag_Blue")
        {
            blueHealth -= damage;
            return blueHealth == 0;
        }
        else
        {
            return false;
        }
    }
    public float GetHealth(string color)
    {
        if (color == "Flag_Red")
        {
            return redHealth;
        }
        else if (color == "Flag_Green")
        {
            return greenHealth;
        }
        else if (color == "Flag_Blue")
        {
            return blueHealth;
        }
        else
        {
            return 0;
        }
    }
    [ClientRpc]
    private void RpcUpdateRedHealth()
    {
        redHealth = redHealth;
    }
    [ClientRpc]
    private void RpcUpdateBlueHealth()
    {
        blueHealth = blueHealth;
    }
    [ClientRpc]
    private void RpcUpdateGreenHealth()
    {
        greenHealth = greenHealth;
    }
    
    
}

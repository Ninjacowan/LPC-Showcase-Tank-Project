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
    [SyncVar] public int redKills = 0;
    [SyncVar] public float blueKills = 0;
    [SyncVar] public int greenKills = 0;
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
}

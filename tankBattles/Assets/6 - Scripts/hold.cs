using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class hold : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Behaviour[] behaviors;
    public GameObject[] gameObjects;
    public GameObject playerPrefab;
    void Start()
    { 
        if (!playerPrefab.GetComponentInChildren<NetworkBehaviour>().isLocalPlayer)
        { 
            for (int i = 0; i < behaviors.Length; i++)
            {
                behaviors[i].enabled = false;
            }

            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
        }
        else
        {

           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Handle : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Behaviour[] things;

    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i=0;i<things.Length;i++)
            {
                things[i].enabled = false;
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

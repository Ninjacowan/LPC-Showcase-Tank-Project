using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class hold : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Behaviour[] a;
    public GameObject[] b;
    GameObject get;
    public GameObject localplayer;
    void Start()
    {
        get = GameObject.FindGameObjectWithTag("Player");

        if (!localplayer.GetComponent<NetworkBehaviour>().isLocalPlayer)
        {


            for (int i = 0; i < a.Length; i++)
            {
                a[i].enabled = false;
            }

            for (int i = 0; i < b.Length; i++)
            {
                b[i].SetActive(false);
            }
        }
        else
        {

            Camera Cam = GetComponentInChildren<Camera>(); ;
            if (Cam != null)
            {
                Cam.enabled = true;
            }

            if (get != null)
            {
                get.SetActive(false);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }
}

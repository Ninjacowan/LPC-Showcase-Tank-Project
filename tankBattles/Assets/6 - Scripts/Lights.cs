using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;
public class Lights : NetworkBehaviour
{
    // Start is called before the first frame update

    public Light turretLight;
    public Light hullLight;
    public Material lightOn;
    public Material lightOff;
    public MeshRenderer meshRenderer1;
    public MeshRenderer meshRenderer2;

    [ReadOnly] public bool IsServer;
    [ReadOnly] public bool IsClient;
    [ReadOnly] public bool IsLocalPlayer;
    [ReadOnly]public bool lightActive = false;

    [SyncVar]
    [Range(0, 10)] public float brightness;
    void Start()
    {
        //meshRenderer1=turretLight.gameObject.GetComponent<MeshRenderer>();
        //meshRenderer2=hullLight.gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        IsServer = isServer;
        IsClient = isClient;
        IsLocalPlayer = isLocalPlayer;
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (lightActive)
            {
                brightness = 0;
                turretLight.intensity = 0;
                hullLight.intensity = 0;
                Material[] mats = meshRenderer1.materials;
                mats[0] = lightOff;
                meshRenderer1.materials = mats;
                meshRenderer2.materials = mats;
            }
            else
            {
                brightness = 6;
                turretLight.intensity = brightness;
                hullLight.intensity = brightness;
                Material[] mats = meshRenderer1.materials;
                mats[0] = lightOn;
                meshRenderer1.materials = mats;
                meshRenderer2.materials = mats;
            }

            if (isServer)
            {
                Rpclights(brightness);
                
            }
            else
            {
                Cmdlights(brightness);
            }
        }
    }
    [Command]
    void Cmdlights(float bright2)
    {

        Rpclights(bright2);
    }

    [ClientRpc]
    void Rpclights(float bright3)
    {
        turretLight.intensity = bright3;
        hullLight.intensity = bright3;
        if (lightActive)
        {
            Material[] mats = meshRenderer1.materials;
            mats[0] = lightOff;
            meshRenderer1.materials = mats;
            meshRenderer2.materials = mats;
            lightActive = !lightActive;
        }
        else
        {
            Material[] mats = meshRenderer1.materials;
            mats[0] = lightOn;
            meshRenderer1.materials = mats;
            meshRenderer2.materials = mats;
            lightActive = !lightActive;
        }
        
    }

}

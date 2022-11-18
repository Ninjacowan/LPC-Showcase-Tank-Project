using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Lights : NetworkBehaviour
{
    // Start is called before the first frame update

    public Light turretLight;
    public Light hullLight;
    bool light_acitve = true;

    [SyncVar]
    float bright;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (light_acitve)
            {
                bright = 0;
                turretLight.intensity = bright;
                hullLight.intensity = bright;
                light_acitve = false;
            }
            else
            {
                bright = 8;
                turretLight.intensity = bright;
                hullLight.intensity = bright;
                light_acitve = true;
            }

            if (isServer)
            {
                Rpclights(bright);
            }
            else
            {
                Cmdlights(bright);
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
    }

}

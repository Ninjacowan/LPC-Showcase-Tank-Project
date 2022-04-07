using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nightLight : MonoBehaviour
{
    public Light thisLight;
    public float mainLightIntensity = .03f;

    // Start is called before the first frame update
    void Start()
    {
        thisLight.intensity = mainLightIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

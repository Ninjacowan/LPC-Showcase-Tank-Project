using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class explosiveLight : NetworkBehaviour  
{
    [ReadOnly] public Light light;
    [ReadOnly] public float startTime;
    [ReadOnly] public float endTime;
    [ReadOnly] public float timeElapsed;
    [ReadOnly] public bool started = false;
    private void Update()
    {
        if (started)
        {
            timeElapsed = startTime - Time.deltaTime;
            light.intensity = light.intensity - .25f;
        }
    }
    private void Awake()
    {
        startTime = Time.deltaTime;
        endTime = Time.deltaTime+1.5f;
        started = true;

    }
}

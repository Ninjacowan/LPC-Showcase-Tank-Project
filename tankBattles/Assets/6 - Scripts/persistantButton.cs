using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistantButton : MonoBehaviour
{


    void Awake()
    {
     
        DontDestroyOnLoad(transform.gameObject); // set to dont destroy
    }
}


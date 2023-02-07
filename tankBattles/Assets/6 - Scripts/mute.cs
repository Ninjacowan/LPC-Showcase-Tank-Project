using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mute : MonoBehaviour
{
    public AudioSource mainMusic;
    bool ismuted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (ismuted)
            {
                mainMusic.volume = 0.1f;
                ismuted = false;
            }
            else
            {
                mainMusic.volume = 0;
                ismuted=true;
            }
        }
    }
}

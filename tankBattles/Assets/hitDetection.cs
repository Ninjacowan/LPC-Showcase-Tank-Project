using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitDetection : MonoBehaviour
{
    public tankControlOverhaul player;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bullet" || collision.gameObject.name == "bullet(Clone)")
        {
            player.health = player.health - 10;
            Debug.Log("Ouch bowser");

        }
    }
}

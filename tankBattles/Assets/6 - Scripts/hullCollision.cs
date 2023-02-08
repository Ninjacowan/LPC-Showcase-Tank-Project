using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class hullCollision : MonoBehaviour
{
    [PreviewField]public tankControlOverhaul controls;
    [PreviewField] public KillCounter killCounter;
    [PreviewField] public gameManager gameManager;
    public GameObject tank;
    public bool debug = false;
    public bool killAdded = false;
    public bullet projectile;
    private void Start()
    {
        gameManager = FindObjectOfType<gameManager>(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "bullet" || collision.gameObject.name == "bullet(Clone)")
        {
            projectile = collision.gameObject.GetComponent<bullet>();
            projectile.tag = collision.gameObject.tag;
            if (debug) { Debug.Log(projectile.tag); }
            if(killCounter.AddDamage(controls.tankColor.name, 25))
            {
                int r = 0;
                int g = 0;
                int b = 0;
                if (projectile.tag == "Flag_Red")
                {
                    r++;
                }
                else if (projectile.tag == "Flag_Green")
                {
                    g++;
                }
                else if (projectile.tag == "Flag_Blue")
                {
                    b++;
                }
                killCounter.UpdateKills(r, g, b);
            }
        }   
    }
}

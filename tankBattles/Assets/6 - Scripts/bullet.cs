using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bullet : NetworkBehaviour
{
    public bool debugging;
    public string tankColor;
    public Rigidbody body;
    public GameObject shell;
    public GameObject explosionSpawner;
    public AudioSource sound;
    [SyncVar]public int damage = 10;
    public Light light;
    public float explosiveStrength = 1f;
    public float explosiveRadius = 1f;
    public float upwardsModifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (debugging)
        {
            Debug.Log(collision.gameObject.name);
        }
        body.constraints = RigidbodyConstraints.FreezePosition;
        light.enabled = false;
        Vector3 explosionPos = transform.position;
        GameObject Explosion = Instantiate(explosionSpawner,explosionPos,Quaternion.identity);
        Rigidbody rb = Explosion.GetComponent<Rigidbody>();
        rb.AddExplosionForce(explosiveStrength, explosionPos, explosiveRadius, upwardsModifier);
        Destroy(Explosion,1.5f);
        Destroy(gameObject,.5f);

    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject shell;
    public GameObject explosionSpawner;
    public AudioSource sound;
    public float explosiveStrength = 1f;
    public float explosiveRadius = 1f;
    public float upwardsModifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 explosionPos = transform.position;
        GameObject Explosion = Instantiate(explosionSpawner,explosionPos,Quaternion.identity);
        Rigidbody rb = Explosion.GetComponent<Rigidbody>();
        rb.AddExplosionForce(explosiveStrength, explosionPos, explosiveRadius, upwardsModifier);
        Destroy(Explosion,7f);
        Destroy(gameObject, 3);

    }
    

}

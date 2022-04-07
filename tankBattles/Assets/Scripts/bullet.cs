using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject shell;
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
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Vector3 explosionPos = transform.position;
        Debug.Log("CONTACT");
        rb.AddExplosionForce(explosiveStrength, explosionPos, explosiveRadius, upwardsModifier);
        Destroy(gameObject,3);
    }

    private void OnCollisionEnter2(Collision collision)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Vector3 explosionPos = transform.position;
        Debug.Log("CONTACT");
        rb.AddExplosionForce(explosiveStrength, explosionPos, explosiveRadius, upwardsModifier);
        Destroy(gameObject,3);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [Range(0, 100)] public float explosiveForce;
    Rigidbody rb;
    void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.AddExplosionForce(explosiveForce*10, new Vector3(0, 0, 0), 5);
        Debug.Log("Boom");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boudary : MonoBehaviour
{
    float yPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody theRB = gameObject.GetComponent<Rigidbody>();
        yPosition = theRB.position.y;
        if (yPosition < -5)
        {
            Destroy(gameObject);
        }
    }
}

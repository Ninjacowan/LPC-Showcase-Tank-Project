using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankBodyMove : MonoBehaviour
{
    public Rigidbody theRB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = theRB.transform.position;
    }
}

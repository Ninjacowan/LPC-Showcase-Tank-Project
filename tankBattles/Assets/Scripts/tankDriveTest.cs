using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankDriveTest : MonoBehaviour
{ 
    public Rigidbody theRB;

    public float forwardAccel = 8f, reverseAccel = 8f, maxSpeed = 50f, turnStrength = 180f;

    private float speedInput = 0f, turnInput = 0f;

    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
            Debug.Log(speedInput);
        } else if (Input.GetAxis("Vertical") < 0) 
        {
            Debug.Log(speedInput);
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
            
        }
        transform.position = new Vector3(theRB.transform.position.x, theRB.transform.position.y, theRB.transform.position.z-1);
        

    }

    private void FixedUpdate() 
    {
        if (Mathf.Abs(speedInput) > 0)
        {
            theRB.AddForce(transform.forward * speedInput);
        }
    }
}

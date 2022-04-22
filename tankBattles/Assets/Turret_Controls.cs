using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controls : MonoBehaviour
{
    public GameObject cameraController;
    public GameObject turret;
    //public GameObject Target;
    public float TurretSpeed = 1.0f;
    public float sensitivity = 200.0f;
    public float cameraRotation;
    public float turretRotation;
    public float distance;
    public float L;
    public float R;
    public string turnDirection = "NONE";
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;     // set to default default
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
    }

    // Update is called once per frame
    void Update()
    {
        cameraController.transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0);
         
        cameraRotation = cameraController.transform.rotation.eulerAngles.y;
        turretRotation = turret.transform.rotation.eulerAngles.y;
        distance = cameraRotation - turretRotation;
        /*if (cameraRotation < turretRotation)
        {
            L = (360 - turretRotation) + cameraRotation;
            R = turretRotation - cameraRotation;
        }
        else if(cameraRotation > turretRotation) 
        {
            L = (360 - cameraRotation) + turretRotation;
            R = cameraRotation - turretRotation;
        }
        

        

        if (distance > 1 || distance < -1)
        {
            if (L < R)
            {
                turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);
                turnDirection = "COUNTER-CLOCKWISE";
            }
            else if (L > R)
            {
                turret.transform.Rotate(Time.deltaTime * -TurretSpeed, 0, 0);
                turnDirection = "CLOCKWISE";
            }
        }
        else 
        {
            turnDirection = "NONE";
        }
        
        /*if (turretRotation < cameraRotation - 1)
        {
            turret.transform.Rotate(Time.deltaTime * -TurretSpeed, 0, 0);
        }
        else if (turretRotation > cameraRotation + 1)
        {
            turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);
        }
        else if (cameraRotation >= 0 && cameraRotation <= 1)
        {
            turret.transform.Rotate(cameraRotation, 0, 0);
        }*/
    }

    Quaternion lookAtSlowly(Transform t, Vector3 target, float speed)
    {

        //(t) is the gameobject transform
        //(target) is the location that (t) is going to look at
        //speed is the quickness of the rotation
        Transform g = t;
        Vector3 relativePos = target - g.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos) * Quaternion.Inverse(Quaternion.Euler(0, -90, 0));
        Debug.Log(Quaternion.Lerp(g.rotation, toRotation, speed * Time.deltaTime));
        return Quaternion.Lerp(g.rotation, toRotation, speed * Time.deltaTime);
    }
}

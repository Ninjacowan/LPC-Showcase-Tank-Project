using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Controls : MonoBehaviour
{
    public GameObject cameraController;
    public GameObject turret;
    public float TurretSpeed = 1.0f;
    public float sensitivity = 200.0f;
    public float cameraRotation;
    public float turretRotation;
    public float distance;
    public float clockwiseAngle;
    public float counterClockWiseAngle;
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

        if (cameraRotation < turretRotation)
        {
            clockwiseAngle = (360 - turretRotation) + cameraRotation;
            counterClockWiseAngle = turretRotation - cameraRotation;
        }
        else if(cameraRotation > turretRotation) 
        {
            counterClockWiseAngle = (360 - cameraRotation) + turretRotation;
            clockwiseAngle = cameraRotation - turretRotation;
        }

        if (distance > 1 || distance < -1)
        {
            if (clockwiseAngle > counterClockWiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);
                turnDirection = "COUNTER-CLOCKWISE";
            }
            else if (clockwiseAngle < counterClockWiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * -TurretSpeed, 0, 0);
                turnDirection = "CLOCKWISE";
            }
        }
        else 
        {
            turnDirection = "NONE";
        }
        
    }

    
}

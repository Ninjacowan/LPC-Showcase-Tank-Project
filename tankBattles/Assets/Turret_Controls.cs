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
    public float angleA;
    public float angleB;
    public string turnDirection = "NONE";
    public GameObject tar;

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

        //STOP
        /*
        Transform target = tar.transform;
        var strength = 1f;
        Vector3 hold;
        hold = new Vector3(target.position.x+90 , target.position.y, turret.transform.position.z);
        var targetRotation = Quaternion.LookRotation(hold - turret.transform.position);
        var str = Mathf.Min(strength * Time.deltaTime, 1);
        turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, targetRotation, str);
        */

        if (cameraRotation < turretRotation)
        {
            angleA = (360 - turretRotation) + cameraRotation;
            angleB = turretRotation - cameraRotation;
        }
        else if(cameraRotation > turretRotation) 
        {
            angleA = (360 - cameraRotation) + turretRotation;
            angleB = cameraRotation - turretRotation;
        }
        

        

        if (distance > 1 || distance < -1)
        {
            if (angleA > angleB)
            {
                turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);
                turnDirection = "COUNTER-CLOCKWISE";
            }
            else if (angleA < angleB)
            {
                turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);
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

    
}

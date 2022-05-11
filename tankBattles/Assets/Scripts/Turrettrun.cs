using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrettrun : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraController;
    public GameObject cameraY;
    public GameObject turret;
    private float cameraRotation;
    private float turretRotation;
    private float distance;
    private float clockwiseAngle;
    private float counterClockWiseAngle;
    public float TurretSpeed = 1.0f;
    public float sensitivity = 200.0f;
    // Update is called once per frame
    void Update()
    {
        //cameraY.transform.Rotate(0f, 0f, (Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity));
        cameraController.transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0f);




        cameraController.transform.position = new Vector3(turret.transform.position.x, turret.transform.position.y + 2.4f, turret.transform.position.z);

        cameraRotation = cameraController.transform.rotation.eulerAngles.y;
        turretRotation = turret.transform.rotation.eulerAngles.y;
        distance = cameraRotation - turretRotation;

        if (cameraRotation < turretRotation)
        {
            clockwiseAngle = (360 - turretRotation) + cameraRotation;
            counterClockWiseAngle = turretRotation - cameraRotation;
        }
        else if (cameraRotation > turretRotation)
        {
            counterClockWiseAngle = (360 - cameraRotation) + turretRotation;
            clockwiseAngle = cameraRotation - turretRotation;
        }

        if (distance > 1 || distance < -1)
        {
            if (clockwiseAngle > counterClockWiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * TurretSpeed, 0, 0);

            }
            else if (clockwiseAngle < counterClockWiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * -TurretSpeed, 0, 0);

            }
        }





    }
}

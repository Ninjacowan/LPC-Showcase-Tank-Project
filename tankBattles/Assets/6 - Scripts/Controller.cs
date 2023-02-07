using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour 
{
    Rigidbody theRB;
    //PUBLIC VARIABLES
    public string tankName;
    public float forwardAccel = 6f;
    public float reverseAccel = 6f;
    public float maxSpeed = 50f;
    public float speedInput = 0f;
    public float turnStrength;
    public float TreadRotationRate;
    public float TurretSpeed;
    public float maxGroundDistance = 2f;
    public float intialBulletSpeed = 1f;
    public float lightIntensity = 10f;
    public float sensitivity;
    public float cameraRotation;
    public float turretRotation;
    public float distance;
    public float clockwiseAngle;
    public float counterClockWiseAngle;
    public string turnDirection = "NONE";


    //PUBLIC OBJECTS
    public GameObject cameraController;
    public GameObject bullet;
    public GameObject bulletSpawner;
    public GameObject bullet_y;
    public GameObject turret;
    public Light tankLight;
    public TextMesh tankTitle;

    //PUBLIC MATERIALS
    public Material lightOFF;
    public Material lightON;

    //VECTORS
    Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    Vector3 offsetTurret;

    //PRIVATE VARIABLES
    float DTG;
    float TurretAngle = 0;
    float maxTurretAngle = 360;
    float minTurretAngle = -360;
    float lastFrameTurretAngle;
    float start_angle;
    float yPosition;
    
    private bool lightActivated = true;

    //PRIVATE MISCELLANEOUS
    Transform boneTurretRotation;
    Renderer tredRenderer;
    Transform boneTurretCannon;

    void Start()
    {
        
        

        intialBulletSpeed = intialBulletSpeed * 1000f;
        Cursor.lockState = CursorLockMode.None;     // set to default default
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
        start_angle = cameraController.transform.localRotation.eulerAngles.x;


        theRB = GetComponent<Rigidbody>();
        Collider col = GetComponent<CapsuleCollider>();
        DTG = col.bounds.extents.y;
        tredRenderer = GetComponentInChildren<Renderer>();
        boneTurretRotation = transform.Find("Root/connectBone001/TurretRotate");
        offsetTurret = boneTurretRotation.localEulerAngles;

    }
    void Update()
    {
        //RAYCAST GROUNDCHECK
        Vector3 forward = transform.TransformDirection(-Vector3.up) * maxGroundDistance;
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), forward, Color.green);

        yPosition = theRB.position.y;
        if (yPosition < -5)
        {
            Destroy(gameObject);
        }

        //TURRET ROTATION ANIMATION

        if (TurretAngle != lastFrameTurretAngle)
        {
            if (TurretAngle > maxTurretAngle)
            {
                TurretAngle = maxTurretAngle;
            }
            else if (TurretAngle < minTurretAngle)
            {
                TurretAngle = minTurretAngle;
            }
            boneTurretRotation.localEulerAngles = new Vector3(offsetTurret.x, offsetTurret.y + TurretAngle, offsetTurret.z);
            lastFrameTurretAngle = TurretAngle;
            
        }



        //CAMERA AND TURRET MOVEMENT AND ROTATION
        cameraController.transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0);

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

    


    //MAIN BARREL DEPRESSION




        TurretAngle = turret.transform.localRotation.eulerAngles.z;

       

        //MOVEMENT INPUTS

        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0 && IsGrouded())
        {
            if (tredRenderer.enabled)
            {
                float var;
                Vector2 uvAnimationRate = new Vector2(TreadRotationRate, 0.0f);
                if (theRB.velocity.z < theRB.velocity.x)
                {
                    var = theRB.velocity.x;
                }
                else
                {
                    var = theRB.velocity.z;
                }
                uvOffset += (uvAnimationRate * Time.deltaTime * (var/12));
                tredRenderer.material.mainTextureOffset = uvOffset;
            }
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 100f;
            
        }
        else if (Input.GetAxis("Vertical") < 0 && IsGrouded())
        {

            if (tredRenderer.enabled)
            {
               float var;
                Vector2 uvAnimationRate = new Vector2(TreadRotationRate, 0.0f);
                if (theRB.velocity.z < theRB.velocity.x)
                {
                    var = theRB.velocity.z;
                }
                else
                {
                    var = theRB.velocity.x;
                }
                uvOffset += (uvAnimationRate * Time.deltaTime * (var/12));
                tredRenderer.material.mainTextureOffset = uvOffset;
            }
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 100f;
            
        }

        if (yPosition < -5)
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.A) &&  IsGrouded() )
        {
            
            transform.Rotate(0,(-turnStrength*Time.deltaTime), 0);
            speedInput = speedInput / 2;
        }
        else if (Input.GetKey(KeyCode.D) && IsGrouded() )
        {
           
            transform.Rotate(0,(turnStrength*Time.deltaTime), 0);
            speedInput = speedInput / 2;
        }
        //FLASHLIGHT
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (lightActivated)
            {
                tankLight.intensity = 0;
                tankLight.GetComponent<MeshRenderer>().material = lightOFF;
                lightActivated = false;
            }
            else
            {
                tankLight.intensity = lightIntensity;
                tankLight.GetComponent<MeshRenderer>().material = lightON;
                lightActivated = true;
            }
        }

        //MAIN CANNON


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawn = new Vector3(bulletSpawner.transform.position.x, bullet_y.transform.position.y , bulletSpawner.transform.position.z);
            
            GameObject cBullet = Instantiate(bullet, spawn, Quaternion.identity);
            cBullet.transform.rotation = bulletSpawner.transform.rotation;
            Rigidbody rig = cBullet.GetComponent<Rigidbody>();
            rig.AddForce(cBullet.transform.forward * intialBulletSpeed);
            
        }

        if (speedInput > maxSpeed)
        {
            Debug.Log("Speed Maximum");
            speedInput = maxSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(speedInput) > 0 && theRB.velocity.x < 14 && theRB.velocity.z < 14)
        { 
            theRB.AddForce(theRB.transform.forward * speedInput);
        }

        
    }
    

    bool IsGrouded()
    {
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        return Physics.Raycast(transform.position + offset,-Vector3.up,maxGroundDistance);
    }
}
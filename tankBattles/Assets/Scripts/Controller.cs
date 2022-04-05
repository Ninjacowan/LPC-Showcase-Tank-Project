using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour 
{
    Rigidbody theRB;
    public float forwardAccel = 6f, reverseAccel = 6f, maxSpeed = 50f, turnStrength;
    float DTG;
    private float speedInput = 0f;
    Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    public float maxGroundDistance = 2f;
    public float TreadRotationRate;
    float TurretAngle = 0;
    Vector3 offsetTurret;
    Transform boneTurretRotation;
    float maxTurretAngle = 360;
    float minTurretAngle = -360;
    float lastFrameTurretAngle;
    Renderer tredRenderer;
    Transform boneTurretCannon;
    public GameObject cameraRotator;

    public float intialBulletSpeed = 1f;

    public GameObject bullet;
    public GameObject bulltt_spawner;
    public GameObject bullet_y;

    public float turretRotationSpeed = 15; 
    public GameObject tankTurret;
    public GameObject mainCannon;
    float start_angle;

    public GameObject tankLightPosition;
    public GameObject lightON;
    public GameObject lightOFF;
    private bool lightActivated = false;



    void Start()
    {
        intialBulletSpeed = intialBulletSpeed * 8000f;
        Cursor.lockState = CursorLockMode.None;     // set to default default
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
        start_angle = cameraRotator.transform.localRotation.eulerAngles.x;


        theRB = GetComponent<Rigidbody>();
        Collider col = GetComponent<CapsuleCollider>();
        DTG = col.bounds.extents.y;
        tredRenderer = GetComponentInChildren<Renderer>();
        boneTurretRotation = transform.Find("Root/connectBone001/TurretRotate");
        offsetTurret = boneTurretRotation.localEulerAngles;
    }
    void Update()
    {
        Vector3 forward = transform.TransformDirection(-Vector3.up) * maxGroundDistance;
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), forward, Color.green);



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



        //mouse
        tankTurret.transform.Rotate(0, 0, (Input.GetAxis("Mouse X") * Time.deltaTime * turretRotationSpeed));

        Debug.Log(cameraRotator.transform.localRotation.eulerAngles);
        if(cameraRotator.transform.localRotation.eulerAngles.x + -Input.GetAxis("Mouse Y") * Time.deltaTime * turretRotationSpeed > start_angle && cameraRotator.transform.localRotation.eulerAngles.x + -Input.GetAxis("Mouse Y") * Time.deltaTime * turretRotationSpeed < start_angle + 70)
        {
        cameraRotator.transform.Rotate(-(Input.GetAxis("Mouse Y") * Time.deltaTime * turretRotationSpeed), 0, 0);
        }
        

        //MAIN BARREL DEPRESSION




        TurretAngle = tankTurret.transform.localRotation.eulerAngles.z;

       

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
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 8000f;
            
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
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 8000f;
            
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
                Vector3 lightSpawn = new Vector3(tankLightPosition.transform.position.x, tankLightPosition.transform.position.y, tankLightPosition.transform.position.z);
                GameObject Light = Instantiate(lightOFF, lightSpawn, Quaternion.identity, tankLightPosition.transform);
                Light.transform.rotation = tankLightPosition.transform.rotation;
                Destroy(lightON);
                lightActivated = false;

            }
            else
            {
                Vector3 lightSpawn = new Vector3(tankLightPosition.transform.position.x, tankLightPosition.transform.position.y, tankLightPosition.transform.position.z);
                GameObject Light = Instantiate(lightON, lightSpawn,Quaternion.identity,tankLightPosition.transform);
                Light.transform.rotation = tankLightPosition.transform.rotation;
                Destroy(lightOFF);
                lightActivated = true;
            }
        }

        //MAIN CANNON


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawn = new Vector3(bulltt_spawner.transform.position.x, bullet_y.transform.position.y , bulltt_spawner.transform.position.z);
            
            GameObject cBullet = Instantiate(bullet, spawn, Quaternion.identity);
            cBullet.transform.rotation = bulltt_spawner.transform.rotation;
            Rigidbody rig = cBullet.GetComponent<Rigidbody>();
            rig.AddForce(cBullet.transform.forward * intialBulletSpeed);
            
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
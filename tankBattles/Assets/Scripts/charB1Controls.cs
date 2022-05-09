using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charB1Controls : MonoBehaviour
{
    #region Game Objects
    public GameObject cameraController;
    public GameObject cameraY;
    public GameObject turret;
    public GameObject Bullet;
    public GameObject bulletSpawner;
    public GameObject bullet_y;
    public Light tankLight;
    public GameObject playerCrosshair;
    public GameObject turretCrosshair;
    public AudioSource engine;
    public AudioSource cannon;
    public UnityEngine.UI.Text healthUI; 
    #endregion

    #region Public Variables
    public float forwardAccel = 6f;
    public float reverseAccel = 6f;
    public float maxSpeed = 50f;
    public float turnStrength;
    public float TreadRotationRate = 3f;
    public float maxGroundDistance = 4f;
    public float intialBulletSpeed = 1f;
    public float fireRate = 1f;
    public float lightIntensity = 10f;
    public float TurretSpeed = 1.0f;
    public float sensitivity = 200.0f;
    public float health = 100;
    public float damage;
    #endregion

    #region Materials
    public Material lightOFF;
    public Material lightON;
    #endregion

    #region Private Variables
    float DTG;
    float start_angle;
    float yPosition;
    private bool lightActivated = false;

    public float speedInput = 0f;
    private float cameraRotation;
    private float turretRotation;
    private float distance;
    private float clockwiseAngle;
    private float counterClockWiseAngle;
    private float nextFire;
    #endregion

    #region Vectors
    Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    Vector3 offsetTurret;
    #endregion

    #region Miscellaneous
    Transform boneTurretRotation;
    Renderer tredRenderer;
    Transform boneTurretCannon;
    Rigidbody theRB;
    TextEditor textEditor;
    
    
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;     // set to default default
        Cursor.lockState = CursorLockMode.Confined; // keep confined in the game window
        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen

        theRB = GetComponent<Rigidbody>();
        Collider col = GetComponent<CapsuleCollider>();
        DTG = col.bounds.extents.y;
        tredRenderer = GetComponentInChildren<Renderer>();
        boneTurretRotation = transform.Find("Root/connectBone001/TurretRotate");
        start_angle = cameraY.transform.localRotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(-Vector3.up) * maxGroundDistance;
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), forward, Color.green);

        yPosition = theRB.position.y;
        if (yPosition < -5)
        {
            Destroy(gameObject);
        }

        #region Turret Rotation


        //cameraY.transform.Rotate(0f, 0f, (Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity));
        cameraController.transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0f);
        
        
        
        
        cameraController.transform.position = new Vector3(turret.transform.position.x,turret.transform.position.y+2.4f, turret.transform.position.z);

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
        
       


        #endregion

        #region Movement
        speedInput = 0f;
        if (Input.GetKey(KeyCode.W) && IsGrouded())
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
                uvOffset += (uvAnimationRate * Time.deltaTime * (var / 12));
                tredRenderer.material.mainTextureOffset = uvOffset;
            }
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 100f;

        }
        else if (Input.GetKey(KeyCode.S) && IsGrouded())
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
                uvOffset += (uvAnimationRate * Time.deltaTime * (var / 12));
                tredRenderer.material.mainTextureOffset = uvOffset;
            }
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 100f;

        }
        if (speedInput > maxSpeed)
        {
            speedInput = maxSpeed;
        }
        else if (speedInput < -maxSpeed/1.1)
        {
            speedInput = -maxSpeed/1.1f;
        }

        if (yPosition < -5)
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.A) && IsGrouded())
        {

            transform.Rotate(0, (-turnStrength * Time.deltaTime), 0);
            speedInput = speedInput / 1.1f;
            
        }
        else if (Input.GetKey(KeyCode.D) && IsGrouded())
        {

            transform.Rotate(0, (turnStrength * Time.deltaTime), 0);
            speedInput = speedInput / 1.1f;
            
        }
        #endregion

        #region Flashlight
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
        #endregion

        #region Turret Cannon
        if (Input.GetMouseButtonDown(0)&& Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 spawn = new Vector3(bulletSpawner.transform.position.x, bullet_y.transform.position.y, bulletSpawner.transform.position.z);
            cannon.Play();
            GameObject cBullet = Instantiate(Bullet, spawn, Quaternion.identity);
            cBullet.transform.rotation = bulletSpawner.transform.rotation;
            Rigidbody rig = cBullet.GetComponent<Rigidbody>();
            rig.AddForce(cBullet.transform.forward * intialBulletSpeed);

        }
        #endregion

        #region User Interface

        turretCrosshair.transform.position = new Vector3((-distance*6)+786,turretCrosshair.transform.position.y,turretCrosshair.transform.position.z);
        #endregion
        
        

        engine.volume = Mathf.Abs(Input.GetAxis("Vertical")) + .2f; ;
        if(engine.volume > .70)
        {
            engine.volume = .70f;
        }

    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(speedInput) > 0 && theRB.velocity.x < 14 && theRB.velocity.z < 14)
        {
            theRB.AddForce(theRB.transform.forward * speedInput);
        }
        else
        {
            
        }
        healthUI.text = health.ToString();


    }


    bool IsGrouded()
    {
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        return Physics.Raycast(transform.position + offset, -Vector3.up, maxGroundDistance);
    }
    private void OnCollisionEnter(Collision collision)
    {

     
        if (collision.gameObject.name == "bullet")
        {
            health = health-damage;

        }
        
    }


}
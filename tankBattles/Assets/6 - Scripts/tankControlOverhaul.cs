using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;
using UnityEngine.UI;

public class tankControlOverhaul : NetworkBehaviour
{
    #region Variables
    [Title("Player Stats")]
    public Material tankColor;
    public SkinnedMeshRenderer flag;
    [ProgressBar(0, 100, 0, 1, 0, Height = 30, ColorGetter = "GetHealthBarColor")] public float health = 100f;
    public bool isAlive = true;
    [Range(0, 200)] public float help = 100;
    [Range(0, 20)] public float help2 = 15;
    public GameObject explosion;

    [Title("User Interface")]
    public Image visor;
    public GameObject playerCrosshair;
    [HorizontalGroup(width:.5f,PaddingLeft =.5f,GroupID ="top")]
    [HideLabel] public Vector2 playerCrosshairPosition;
    public GameObject turretCrosshair;
    [HorizontalGroup(width: .5f, PaddingLeft = .5f,GroupID ="bottom")]
    [HideLabel] public Vector2 turretCrosshairPosition;
    public Vector2 mosPos;
    public UnityEngine.UI.Text healthUI;
    public RectTransform needle;
    public RectTransform meterIndicator;
    public Canvas canvas;
    public NetworkManager networkManager;
    public Camera camera;
    public bool lockCamera = false;

    [Title("Movement")]
    [LabelText("Rigid Body"), InlineEditor(InlineEditorModes.GUIAndHeader)] public Rigidbody theRB;
    [InlineEditor(InlineEditorModes.LargePreview)] public GameObject tankBody;
    [TabGroup("Tank Movement", true), Range(0, 10), LabelText("Forward Acceleration")] public float forwardAcc;
    [TabGroup("Tank Movement"), Range(0, 10), LabelText("Reverse Acceleration")] public float reverseAcc;
    [TabGroup("Tank Movement"), LabelText("Maximum Speed"), MinMaxSlider(-500000, 500000, true)] public Vector2 maxSpeed;
    [TabGroup("Tank Movement"), Range(0, 50)] public float turnStrength;
    [TabGroup("Tank Movement"), Range(0, 10), LabelText("Maximum Distance from Ground")] public float maxGroundDistance;
    [TabGroup("Tank Movement")] public bool isTouchingGround;
    [TabGroup("Tank Movement"), Range(0, 10)] public float treadRotationRate = 3;
    [TabGroup("Tank Movement"), ReadOnly] public float speedInput;
    [TabGroup("Tank Movement"), ReadOnly] public float forwardInput;
    [TabGroup("Tank Movement")] public AudioSource engineAudioSource;
    Renderer tredRenderer;
    Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    [TabGroup("Turret Rotation", true)] public float rotationSpeed;
    [TabGroup("Turret Rotation")] public GameObject cameraController;
    [TabGroup("Turret Rotation")] public GameObject cameraY;
    [TabGroup("Turret Rotation")] public GameObject turret;
    [TabGroup("Turret Rotation"), Range(0, 10)] public float sensitivity;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float cameraRotation;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float turretRotation;
    [TabGroup("Turret Rotation"), Range(-360, 360), ReadOnly, SuffixLabel("°")] public float distance;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float clockwiseAngle;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float counterClockwiseAngle;

    [TitleGroup("Main Cannon", "Adjust settings for the main cannon")]
    [InlineEditor(InlineEditorModes.GUIOnly)]public Transform turretBarrel;
    [Range(-1000, 1000)] public float adjusted;
    [MinMaxSlider(-10, 10,showFields: true),Space(15)] public Vector2 range;
    public float adjustedPosition;
    [ReadOnly] public float relativePlayerCrosshairPos;
    public bool eulerUnits = false;
    [ReadOnly] public float turretBarrelRotation;
    [ReadOnly, Range(0, 10)] public float adjustedBarrelElevation;
    public float barrelSpeed = 5;
    public GameObject bulletSpawner;
    public GameObject bulletY;
    [ReadOnly] public float nextFire = 0;
    [Range(0, 5)] public float fireRate = 1f;
    public float intialBulletSpeed = 5000f;
    [PreviewField] public GameObject bulletPrefab;
    [PreviewField] public AudioSource cannonAudio;
    public shoot shootScript;
    public float damage = 10;

    [TitleGroup("Respawning")]
    public gameManager gameManager;
    public Transform deathArea;
    public Transform hiddenPos;
    [Range(0, 10)] public int cooldown;
    private float respawnTime = 5;
    private float adjustedSensitivity;
    private float adjustedRotationSpeed;

    #endregion
    Collision lastCollision;
    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>(true);

    }
    void Start()
    {
        visor.enabled = false;
        adjustedSensitivity = sensitivity;
        adjustedRotationSpeed = rotationSpeed;
        gameManager = FindObjectOfType<gameManager>(true);
        gameManager.players.Add(this);
        tankColor = gameManager.colors[gameManager.players.Count - 1];
        Material[] mats = flag.materials;
        mats[0] = tankColor;
        flag.materials = mats;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        tredRenderer = GetComponentInChildren<Renderer>();
        //Debug.Log("Half of Screen Height: "+Screen.height / 2);
        turretBarrel.rotation.SetEulerAngles(270, turretBarrel.rotation.eulerAngles.y, turretBarrel.rotation.eulerAngles.z);
        deathArea = FindObjectOfType<gameManager>(true).transform;
        hiddenPos = GameObject.Find("hiddenPos").transform;

        
    }
    void Update()
    {
        isTouchingGround=IsGrounded();
        forwardInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.C))
        {
            camera.fieldOfView = 30;
            adjustedSensitivity = sensitivity / 2;
            adjustedRotationSpeed = rotationSpeed / 4;
            visor.enabled = true;

        }
        else
        {
            camera.fieldOfView = 60;
            adjustedSensitivity = sensitivity;
            adjustedRotationSpeed = rotationSpeed;
            visor.enabled = false;
        }

        needle.eulerAngles = new Vector3(0, 0, (100 - health) * 2.67f + 133);

        //Draws the ray that checks to see if the tank is grounded
        Vector3 forward = tankBody.transform.TransformDirection(-Vector3.up) * maxGroundDistance;
        Debug.DrawRay(tankBody.transform.position + new Vector3(0.0f, 1.0f, 0.0f), forward, Color.green);
        UpdateHealth(tankColor.name);
        if (isAlive)
        {
            if (!lockCamera)
            {
                #region Turret rotation


                //Rotates the camera to where the mouse is pointing
                cameraController.transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * adjustedSensitivity * 100, 0f);
                //Moves the camera so the turret is always in view
                cameraController.transform.position = new Vector3(turret.transform.position.x, turret.transform.position.y + 1f, turret.transform.position.z);

                //Calculates the distance from where the camera is facing to where the turret is facing
                cameraRotation = cameraController.transform.rotation.eulerAngles.y;
                turretRotation = turret.transform.rotation.eulerAngles.y;
                distance = cameraRotation - turretRotation;

                //Calculates how far the turret would have to rotate in both directions to determine the fastest way to get to the camera
                if (cameraRotation < turretRotation)
                {
                    clockwiseAngle = (360 - turretRotation) + cameraRotation;
                    counterClockwiseAngle = turretRotation - cameraRotation;
                }
                else if (cameraRotation > turretRotation)
                {
                    counterClockwiseAngle = (360 - cameraRotation) + turretRotation;
                    clockwiseAngle = cameraRotation - turretRotation;
                }

                if (distance > 1 || distance < -1)
                {
                    if (clockwiseAngle < counterClockwiseAngle)
                    {
                        turret.transform.Rotate(0, Time.deltaTime * adjustedRotationSpeed, 0);
                    }
                    else if (clockwiseAngle > counterClockwiseAngle)
                    {
                        turret.transform.Rotate(0, Time.deltaTime * -adjustedRotationSpeed, 0);
                    }
                    
                }
                else
                {
                    distance = 0;
                }

                #endregion
            }
            #region Movement
            //FIXME

            speedInput = 0;
            if (Input.GetKey(KeyCode.W)&&IsGrounded())
            {
                if (tredRenderer.enabled)
                {
                    float var;
                    Vector2 uvAnimationRate = new Vector2(treadRotationRate, 0.0f);
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
                speedInput = Input.GetAxis("Vertical") * forwardAcc * 100000;
            }
            else if (Input.GetKey(KeyCode.S) && IsGrounded())
            {
                if (tredRenderer.enabled)
                {
                    float var;
                    Vector2 uvAnimationRate = new Vector2(treadRotationRate, 0.0f);
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
                speedInput = Input.GetAxis("Vertical") * reverseAcc * 100000f;
            }

            if (Input.GetKey(KeyCode.A) && IsGrounded())
            {

                tankBody.transform.Rotate(0, (-turnStrength * Time.deltaTime), 0);
                theRB.velocity = theRB.velocity / 1.001f;

            }
            else if (Input.GetKey(KeyCode.D) && IsGrounded())
            {

                tankBody.transform.Rotate(0, (turnStrength * Time.deltaTime), 0);
                theRB.velocity = theRB.velocity / 1.001f;

            }
            if (speedInput > maxSpeed.y)
            {
                speedInput = maxSpeed.y;
            }
            else if (speedInput < maxSpeed.x)
            {
                speedInput = maxSpeed.x;
            }
            #endregion

            #region Flashlight (Disabled)
            /**
            if (Input.GetKeyDown(flashlightToggle))
            {
                lightActivated = !lightActivated;
            }

            if (!lightActivated)
            {
                for(int i=0; i < lightSources.Count; i++)
                {
                    lightSources[i].intensity = 0;
                    lightSources[i].GetComponent<MeshRenderer>().material = lightOFF;
                }
            }
            if (lightActivated)
            {
                for (int i = 0; i < lightSources.Count; i++)
                {
                    lightSources[i].intensity = lightIntensity;
                    lightSources[i].GetComponent<MeshRenderer>().material = lightON;
                }
            }
            **/
            #endregion

            adjustedPosition = playerCrosshair.transform.position.y / 10 - 45;
            relativePlayerCrosshairPos = playerCrosshair.transform.position.y + adjusted;
            if (eulerUnits)
            {
                turretBarrelRotation = turretBarrel.localRotation.eulerAngles.z;
            }
            else
            {
                turretBarrelRotation = turretBarrel.localRotation.z;
            }
            playerCrosshairPosition = playerCrosshair.transform.position;
            turretCrosshairPosition = new Vector2(turretCrosshair.transform.position.z, turretCrosshair.transform.position.y);
            mosPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            playerCrosshair.transform.position = new Vector2(playerCrosshair.transform.position.x, Input.mousePosition.y);
            turretCrosshair.transform.localPosition = new Vector2((-distance * 15), adjustedBarrelElevation * 15 - 118);


            if (adjustedBarrelElevation > adjustedPosition)
            {
                if (adjustedBarrelElevation > 0)
                {
                    adjustedBarrelElevation = adjustedBarrelElevation - barrelSpeed;
                }
            }
            if (adjustedBarrelElevation < adjustedPosition)
            {
                if (adjustedBarrelElevation < 10)
                {
                    adjustedBarrelElevation = adjustedBarrelElevation + barrelSpeed;
                }
            }
            if(Mathf.Abs(distance) < 1)
            turretBarrel.transform.localEulerAngles = new Vector3(turretBarrel.localEulerAngles.x, turretBarrel.localEulerAngles.y, adjustedBarrelElevation-5);
            meterIndicator.localPosition = new Vector3(meterIndicator.localPosition.x, adjustedBarrelElevation * help2 - help, 0);
        }
        else
        {
            speedInput = 0;
            cameraController.transform.position = deathArea.position;
            cameraController.transform.rotation = deathArea.rotation;
            canvas.enabled = false;
            tankBody.transform.position = hiddenPos.position;
            lockCamera = true;
            shootScript.enabled = false;
            
            if (Time.time > respawnTime)
            {
                int i = Random.Range(0, gameManager.spawnPoints.Count);
                tankBody.transform.position = gameManager.spawnPoints[i].position;
                tankBody.transform.rotation = gameManager.spawnPoints[i].rotation;
                cameraController.transform.position = gameManager.spawnPoints[i].position;
                cameraController.transform.rotation = gameManager.spawnPoints[i].rotation;
                shootScript.enabled = true;
                canvas.enabled = true;
                tankBody.active = true;
                lockCamera = false;
                isAlive = true;
            }
            
        }
        engineAudioSource.volume = Mathf.Abs(Input.GetAxis("Vertical"))/10 + .1f; ;
        if (engineAudioSource.volume > .4f)
        {
            engineAudioSource.volume = .4f;
        }
        /**
        if (health <= 0 && isAlive)
        {

            respawnTime = Time.time + cooldown;
            isAlive = false;
            Vector3 explosionPos = tankBody.transform.position;
            GameObject Explosion = Instantiate(explosion, explosionPos, Quaternion.identity);
            Destroy(Explosion, 1.5f);


        }
        **/

    }
    bool IsGrounded()
    {
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        return Physics.Raycast(tankBody.transform.position + offset, tankBody.transform.TransformDirection(-Vector3.up) * maxGroundDistance,3);
        
    }
    void FixedUpdate()
    {
        
        if (Mathf.Abs(speedInput) > 0&&isAlive)
        {
            if (theRB.velocity.magnitude < 3.5)
            {
                theRB.AddForce(theRB.transform.right * speedInput);
            }
        }
        else if(Mathf.Abs(speedInput) < 0 &&isAlive)
        {
            if (theRB.velocity.magnitude < 3.5)
            {
                theRB.AddForce(theRB.transform.right * speedInput);
            }
        }
        if (tankBody.transform.position.y <= -10)
        {
            health = 0;
        }
        healthUI.text = health.ToString();
        
    }
    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
    
    public void UpdateHealth(string color)
    {
        if (isServer)
        {
            RpcUpdateHealth(color);
        }
        else
        {
            CmdUpdateHealth(color);
        }
    }
    [Command]
    private void CmdUpdateHealth(string color)
    {
        Debug.Log("Command Update Health, Color: " + color + " r: " + gameManager.redHealth + " b:" + gameManager.blueHealth);
        if (color == "Flag_Red")
        {
            health = gameManager.redHealth;
        }
        else if (color == "Flag_Green")
        {
            health = gameManager.greenHealth;
        }
        else if (color == "Flag_Blue")
        {
            health = gameManager.blueHealth;
        }
    }
    [ClientRpc]
    private void RpcUpdateHealth(string color)
    {
        Debug.Log("ClientRPC Update Health, Color: " + color + " r: " + gameManager.redHealth + " b:" + gameManager.blueHealth);
        if (color == "Flag_Red")
        {
            health = gameManager.redHealth;
        }
        else if (color == "Flag_Green")
        {
            health = gameManager.greenHealth;
        }
        else if (color == "Flag_Blue")
        {
            health = gameManager.blueHealth;
        }
    }
    /**
    public bool TakeDamage(int damage)
    {
        if (isServer)
        {
            return RpcTakeDamage(damage);
        }
        else
        {
            return CmdTakeDamage(damage);
        }
    }
    private bool CmdTakeDamage(int damage)
    {
        health -= damage;
        UpdateHealth(health);
        return health == 0;
    }
    private bool RpcTakeDamage(int damage)
    {
        health -= damage;
        UpdateHealth(health);
        return health == 0;
    }
    **/
}
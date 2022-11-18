using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Networking;

public class tankControlOverhaul : MonoBehaviour
{

    #region Variables
    [Title("Player Stats")]
    [ProgressBar(0, 100, 0, 1, 0, Height = 30, ColorGetter = "GetHealthBarColor")] public float health =100f;
    [Title("Movement")]
    [LabelText("Rigid Body"),InlineEditor(InlineEditorModes.GUIAndHeader)]public Rigidbody theRB;
    [InlineEditor(InlineEditorModes.LargePreview)]public GameObject tankBody;
    [TabGroup("Tank Movement",true), Range(0, 10), LabelText("Forward Acceleration")] public float forwardAcc;
    [TabGroup("Tank Movement"), Range(0, 10),LabelText("Reverse Acceleration")] public float reverseAcc;
    [TabGroup("Tank Movement"),LabelText("Maximum Speed"), MinMaxSlider(-500000,500000,true)] public Vector2 maxSpeed;
    [TabGroup("Tank Movement"), Range(0, 50)] public float turnStrength;
    [TabGroup("Tank Movement"), Range(0, 10),LabelText("Maximum Distance from Ground")] public float maxGroundDistance;
    [TabGroup("Tank Movement"), Range(0, 10)] public float treadRotationRate = 3;
    [TabGroup("Tank Movement"), ReadOnly] public float speedInput;
    Renderer tredRenderer;
    Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    [TabGroup("Turret Rotation",true)] public float rotationSpeed;
    [TabGroup("Turret Rotation")] public GameObject cameraController;
    [TabGroup("Turret Rotation")] public GameObject cameraY;
    [TabGroup("Turret Rotation")] public GameObject turret;
    [TabGroup("Turret Rotation"), Range(0, 300)] public float sensitivity;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float cameraRotation;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float turretRotation;
    [TabGroup("Turret Rotation"), Range(-360, 360), ReadOnly, SuffixLabel("°")] public float distance;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float clockwiseAngle;
    [TabGroup("Turret Rotation"), Range(0, 360), ReadOnly, SuffixLabel("°")] public float counterClockwiseAngle;

    [TitleGroup("Main Cannon","Adjust settings for the main cannon")]
    public GameObject bulletSpawner;
    public GameObject bulletY;
    [Range(0, 5)] public float fireRate = 1f;
    public float intialBulletSpeed = 5000f;
    [PreviewField] public GameObject bulletPrefab;
    [PreviewField] public AudioSource cannonAudio;

    


    [TitleGroup("Flashlights","Adjust settings for lights on the tank")]
    [InfoBox("If you need to change Range or Spot Angle, you must edit the light source directly.")]
    public List<Light> lightSources;
    [TabGroup("Light Group","Light Settings",true)]
    [Title("Light Intensity", null, TitleAlignments.Centered, false, false)]
    [ProgressBar(0, 10, 1, 1, 1, Height = 30), HideLabel] public float lightIntensity;
    bool lightActivated = false;
    [TabGroup("Light Group", "Light Settings")]
    [DisableIf("lightActivated")]
    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    private void ActivateLight()
    {
        this.lightActivated = true;
    }
    [TabGroup("Light Group", "Light Settings")]
    [EnableIf("lightActivated")]
    [Button(ButtonSizes.Large), GUIColor(1, 0, 0)]
    private void DeactivateLight()
    {
        this.lightActivated = false;
    }
    [TabGroup("Light Group", "Materials",true), PreviewField] public Material lightON;
    [TabGroup("Light Group", "Materials"),PreviewField] public Material lightOFF;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        tredRenderer = GetComponentInChildren<Renderer>();
    }
    void Update()
    {
        //Draws the ray that checks to see if the tank is grounded
        Vector3 forward = transform.TransformDirection(-Vector3.up) * maxGroundDistance;
        Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f, 0.0f), forward, Color.green);

        #region Turret Rotation
        //Rotates the camera to where the mouse is pointing
        cameraController.transform.Rotate(0f, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0f);
        //Moves the camera so the turret is always in view
        cameraController.transform.position = new Vector3(turret.transform.position.x, turret.transform.position.y + 2.4f, turret.transform.position.z);

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
            if (clockwiseAngle > counterClockwiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0);
            }
            else if (clockwiseAngle < counterClockwiseAngle)
            {
                turret.transform.Rotate(Time.deltaTime * -rotationSpeed, 0, 0);
            }
        }
        #endregion

        #region Movement
        
        speedInput = 0;
        if (Input.GetKey(KeyCode.W) && IsGrounded())
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
            speedInput = speedInput / 1.1f;
            theRB.velocity = theRB.velocity / 1.5f;

        }
        else if (Input.GetKey(KeyCode.D) && IsGrounded())
        {

            tankBody.transform.Rotate(0, (turnStrength * Time.deltaTime), 0);
            speedInput = speedInput / 1.1f;
            theRB.velocity = theRB.velocity / 1.1f;

        }
        
        if(speedInput > maxSpeed.y)
        {
            speedInput = maxSpeed.y;
        }
        else if(speedInput < maxSpeed.x)
        {
            speedInput = maxSpeed.x;
        }
        
        #endregion
    }
    bool IsGrounded()
    {
        Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
        return Physics.Raycast(transform.position + offset, -Vector3.up, maxGroundDistance);
    }
    void FixedUpdate()
    {
        if (Mathf.Abs(speedInput) > 0)
        {
            if(theRB.velocity.x < 2 && theRB.velocity.z < 2)
            {
                theRB.AddForce(theRB.transform.forward * speedInput);
            }
            
        }
        


    }
    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }


}


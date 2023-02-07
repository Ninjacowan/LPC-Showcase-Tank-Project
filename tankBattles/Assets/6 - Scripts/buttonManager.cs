using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Sirenix.OdinInspector;


public class buttonManager : MonoBehaviour
{
    public NetworkManager networkManager;
    public Camera mainCamera;
    [Range(10,30)]public float cameraSpeed = 20.0f;
    public bool debugging;


    [Title("Start Menu Elements")]
    public Transform mainPosition;
    public Button startButton;
    public Button exitButton;
    public Button settingsButton;
    public GameObject[] startMenuElements;
    [Title("Setting Menu Elements")]
    public Transform settingsPosition;
    public Button backSettingsButton;
    public Button backPlayButton;
    public GameObject[] settingMenuElements;
    [Title("Play Menu Elements")]
    public Transform playPosition;
    public Button hostButton;
    public Button joinButton;
    public Text ipInput;
    public GameObject[] playMenuElements;

    private float startTime;
    private float journeyLength;
    private float distanceCovered;
    private float fractionOfJourney;
    private bool startButtonSelected;
    private bool exitButtonSelected;
    private bool settingsButtonSelected;
    private bool backSettingsButtonSelected;
    private bool backPlayButtonSelected;
    
    void Start()
    {

        Cursor.visible = true;
        settingsButtons(false);
        playButtons(false);
        

        
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;    // set to default default
        Cursor.lockState = CursorLockMode.None; // keep confined in the game window
        Cursor.lockState = CursorLockMode.None;   // keep confined to center of screen
        networkManager = FindObjectOfType<NetworkManager>();
    }


    void Update()
    {
        /*if (hostButton == null)
        {
            try
            {
                hostButton = GameObject.Find("hostButton").GetComponent<Button>();
            }
            catch
            { }

        }
        else
        {
            hostButton.onClick.AddListener(Host);
        }
        if (joinButton == null)
        {
            try
            {
                joinButton = GameObject.Find("joinButton").GetComponent<Button>();
            }
            catch { }
        }
        else
        {
            joinButton.onClick.AddListener(Join);
        }*/
        startButton.onClick.AddListener(startTask);
        settingsButton.onClick.AddListener(settingsTask);
        backSettingsButton.onClick.AddListener(backTaskS);
        exitButton.onClick.AddListener(endTask);
        backPlayButton.onClick.AddListener(backTaskP);
        hostButton.onClick.AddListener(Host);
        //joinButton.onClick.AddListener(Join);

        if (startButtonSelected)
        {

            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(mainPosition.position, playPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(mainPosition.rotation, playPosition.rotation, fractionOfJourney);
            if(distanceCovered > journeyLength)
            {
                startButtonSelected = false;
                playButtons(true);
            }
        }
        else if (settingsButtonSelected)
        {
            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(mainPosition.position, settingsPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(mainPosition.rotation, settingsPosition.rotation, fractionOfJourney);
            if (distanceCovered > journeyLength)
            {
                settingsButtonSelected = false;
                settingsButtons(true);
            }
        }
        else if (backSettingsButtonSelected)
        {
            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(settingsPosition.position, mainPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(settingsPosition.rotation, mainPosition.rotation, fractionOfJourney);
            if (distanceCovered > journeyLength)
            {
                backSettingsButtonSelected = false;
                mainButtons(true);
            }
        }
        else if (backPlayButtonSelected)
        {
            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(playPosition.position, mainPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(playPosition.rotation, mainPosition.rotation, fractionOfJourney);
            if (distanceCovered > journeyLength)
            {
                backPlayButtonSelected = false;
                mainButtons(true);
            }
        }
        else if (exitButtonSelected)
        {
            Application.Quit(0);
        }

    }
    void startTask()
    {
        startButtonSelected = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(mainPosition.position, playPosition.position);
        mainButtons(false);
    }
    void settingsTask()
    {
        settingsButtonSelected = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(mainPosition.position, settingsPosition.position);
        mainButtons(false);
    }
    void backTaskS()
    {
        backSettingsButtonSelected = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(settingsPosition.position, mainPosition.position);
        settingsButtons(false);
    }
    void backTaskP()
    {
        backPlayButtonSelected = true;
        startTime= Time.time;
        journeyLength = Vector3.Distance(playPosition.position, mainPosition.position);
        playButtons(false);
    }
    void endTask()
    {
        exitButtonSelected = true;
    }
    void mainButtons(bool val)
    {
        for(int i = 0; i < startMenuElements.Length; i++)
        {
            startMenuElements[i].SetActive(val);
        }
        
    }
    void playButtons(bool val)
    {
        for(int i = 0; i < playMenuElements.Length; i++)
        {
            playMenuElements[i].SetActive(val);
        }
    }
    void settingsButtons(bool val)
    {
        for(int i = 0; i < settingMenuElements.Length; i++)
        {
            settingMenuElements[i].SetActive(val);
        }
    }

    void Host()
    {
        //networkManager.networkAddress = "localhost";
        //networkManager.StartHost();
    }
    void Join()
    { 
        //networkManager.networkAddress = ipInput.ToString();
        //Debug.Log(ipInput.text);
        //networkManager.StartClient();
    }
    
}


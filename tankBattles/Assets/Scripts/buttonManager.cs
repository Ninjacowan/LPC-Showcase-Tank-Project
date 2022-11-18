using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


public class buttonManager : MonoBehaviour
{
    public NetworkManager NetworkManager;
    public bool debugging;
    public Camera mainCamera;
    #region Buttons   
    public Button startButton;
    public Button exitButton;
    public Button settingsButton;
    public Button backSettingsButton;
    public Button backPlayButton;
    //public Button hostButton;
    //public Button joinButton;
    //public Text addressInput;
    public GameObject[] settingMenuElements;
    public GameObject[] startMenuElements;
    public GameObject[] playMenuElements;
    #endregion

    #region Positions
    public Transform mainPosition;
    public Transform playPosition;
    public Transform settingsPosition;
    public float cameraSpeed = 1.0f;
    #endregion

    #region Private
    private float startTime;
    private float journeyLength;
    private float distanceCovered;
    private float fractionOfJourney;
    private bool startButtonSelected;
    private bool exitButtonSelected;
    private bool settingsButtonSelected;
    private bool backSettingsButtonSelected;
    private bool backPlayButtonSelected;

    
    #endregion
    
    void Start()
    {
        
        
        settingsButtons(false);
        playButtons(false);
        

        
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;    // set to default default
        Cursor.lockState = CursorLockMode.None; // keep confined in the game window
        Cursor.lockState = CursorLockMode.None;   // keep confined to center of screen
        NetworkManager = FindObjectOfType<NetworkManager>();
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
        //hostButton.onClick.AddListener(Host);
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

    /*void Host()
    {
        if (debugging)
        {
            Debug.Log("host button");
        }
        NetworkManager.maxConnections = 2;
        if(addressInput.ToString().Length < 4)
        {
            NetworkManager.networkPort =  int.Parse(addressInput.ToString());
        }
        else
        {
            NetworkManager.networkPort = 7777;
        }
        ConnectionConfig defaultConfig = NetworkManager.connectionConfig;
        NetworkManager.StartHost(defaultConfig, 2);
        
    }
    void Join()
    {
        if (debugging)
        {
            Debug.Log("join button");
        }
        if(addressInput.ToString().Length < 4)
        {
            NetworkManager.networkPort =  int.Parse(addressInput.ToString());
        }
        else
        {
            NetworkManager.networkPort = 7777;
        }
        
    }
    */
}


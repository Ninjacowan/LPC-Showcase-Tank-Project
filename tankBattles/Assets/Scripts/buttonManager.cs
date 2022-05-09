using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonManager : MonoBehaviour
{
    public Camera mainCamera;
    #region Buttons
    public Button startButton;
    public Button exitButton;
    public Button settingsButton;
    public Button backSettingsButton;
    public Button backPlayButton;
    public Button hostButton;
    public Button joinButton;
    public GameObject addressInput;
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

    private GameObject temp1;
    private GameObject temp2;
    private GameObject temp3;
    private GameObject temp4;
    private GameObject temp5;
    private GameObject temp6;
    private GameObject temp7;
    private GameObject temp8;
    #endregion
    
    void Start()
    {
        temp1 = GameObject.Find("startButton");
        temp2 = GameObject.Find("settingsButton");
        temp3 = GameObject.Find("exitButton");
        temp4 = GameObject.Find("backButtonSettings");
        temp5 = GameObject.Find("backButtonPlay");
        temp6 = GameObject.Find("joinButton");
        temp7 = GameObject.Find("hostButton");
        temp4.SetActive(false);
        temp5.SetActive(false);
        temp6.SetActive(false);
        temp7.SetActive(false);
        addressInput.SetActive(false);
    }

    
    void Update()
    {
        startButton.onClick.AddListener(startTask);
        settingsButton.onClick.AddListener(settingsTask);
        backSettingsButton.onClick.AddListener(backTaskS);
        exitButton.onClick.AddListener(endTask);
        backPlayButton.onClick.AddListener(backTaskP);

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
        temp1.SetActive(val);
        temp2.SetActive(val);
        temp3.SetActive(val);
        
    }
    void playButtons(bool val)
    {
        temp5.SetActive(val);
        temp6.SetActive(val);
        temp7.SetActive(val);
        addressInput.SetActive(val);
    }
    void settingsButtons(bool val)
    {
        
        temp4.SetActive(val);
    }
}


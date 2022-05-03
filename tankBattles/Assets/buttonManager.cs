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
    public Button backButton;
    #endregion

    #region Positions
    public Transform mainPosition;
    public Transform playPosition;
    public Transform settingsPosition;
    public Transform tempPos;
    public float cameraSpeed = 1.0f;
    #endregion

    #region Private
    private float startTime;
    public float journeyLength;
    public float distanceCovered;
    public float fractionOfJourney;
    public bool startButtonSelected;
    public bool exitButtonSelected;
    public bool settingsButtonSelected;
    public bool backButtonSelected;
    #endregion
    public float distance;
    void Start()
    {
        
    }

    
    void Update()
    {
        startButton.onClick.AddListener(startTask);
        settingsButton.onClick.AddListener(settingsTask);
        backButton.onClick.AddListener(backTask);
        exitButton.onClick.AddListener(endTask);

        if (startButtonSelected)
        {
            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(mainPosition.position, playPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(mainPosition.rotation, playPosition.rotation, fractionOfJourney);
            if(distanceCovered > journeyLength)
            {
                startButtonSelected = false;
                distanceCovered = 0;
                journeyLength = 0;
                fractionOfJourney = 0;
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
            }
        }
        else if (backButtonSelected)
        {
            distanceCovered = (Time.time - startTime) * cameraSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(tempPos.transform.position, mainPosition.position, fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(tempPos.transform.rotation, mainPosition.rotation, fractionOfJourney);
            if (distanceCovered > journeyLength)
            {
                backButtonSelected = false;
                tempPos = null;
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
    }
    void settingsTask()
    {
        settingsButtonSelected = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(mainPosition.position, settingsPosition.position);
    }
    void backTask()
    {
        tempPos = mainCamera.transform;
        backButtonSelected = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(tempPos.position, playPosition.position);
    }
    void endTask()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class startButtonMenu : MonoBehaviour
{
    public Button sButton;
    public GameObject mainCamera;
    public Transform endCameraPos;
    private Transform startCameraPos;
    public float speed = 1.0f;
    private float startTime;
    private float journeyLength;
    private float distanceCovered;
    private float fractionOfJourney;
    private bool buttonSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        startCameraPos = mainCamera.transform;
        sButton.onClick.AddListener(TaskOnClick);

        
        journeyLength = Vector3.Distance(startCameraPos.position, endCameraPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonSelected)
        {
            distanceCovered = (Time.time - startTime)*speed;
            fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(startCameraPos.position, endCameraPos.position,fractionOfJourney);
            mainCamera.transform.rotation = Quaternion.Lerp(startCameraPos.rotation, endCameraPos.rotation,fractionOfJourney);
        }
        
    }
    void TaskOnClick()
    {
        buttonSelected = true;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RocketFlare : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    [MinMaxSlider(0, 120, true)] public Vector2 delayRange;
    [ReadOnly] public float nextFlareTime;
    public AudioSource flareLaunchAudio;
    public AudioSource flareBurnAudio;
    public float speed = 10f;
    private float startTime;
    private float journeyLength;
    private float distanceCovered;
    private float fractionOfJourney;
    private bool help;

    // Start is called before the first frame update
    void Start()
    {
        nextFlareTime = Time.time + Random.Range(delayRange.x, delayRange.y);
        //this.gameObject.SetActive(false);
        journeyLength = Vector3.Distance(startPosition.position, endPosition.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextFlareTime)
        {
            if (help)
            {
                startTime = Time.time;
                help = false;
                flareLaunchAudio.Play();
                flareBurnAudio.Play();
            }
            //this.gameObject.SetActive(true);
            distanceCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distanceCovered / journeyLength;
            this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, fractionOfJourney);
            if (distanceCovered > journeyLength)
            {
                help = true;
                nextFlareTime = Time.time + Random.Range(delayRange.x, delayRange.y);
                this.transform.position = startPosition.position;
                flareBurnAudio.Stop();
                //this.gameObject.SetActive(false);
            }
        }
    }
}

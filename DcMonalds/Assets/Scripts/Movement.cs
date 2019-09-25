using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Quaternion startRotation;
    public Quaternion endRotation;

    public float lerpSpeed = 1.0f;
    public bool alwaysLerping;
    public bool turn;


    private bool lerping;
    private float lerpStartTime;
    private float journeyLength;
    private Vector3 startMarker;
    private Vector3 endMarker;


    // Use this for initialization
    void Start()
    {
        if (alwaysLerping)
        {
            StartLerp(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (lerping)
        {
            float distCovered = (Time.time - lerpStartTime) * lerpSpeed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);

            if (turn)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, fracJourney);
            }

            if (Mathf.Approximately(transform.position.x, endMarker.x)
                && Mathf.Approximately(transform.position.z, endMarker.z))
            {
                lerping = false;
            }


        }
        else if (alwaysLerping)
        {
            Vector3 temp = startMarker;
            startMarker = endMarker;
            endMarker = temp;
            StartLerp(false);
        }


    }

    public void SetStartMarker(Vector3 startPos)
    {
        startMarker = startPos;
    }

    public void SetEndMarker(Vector3 endPos)
    {
        endMarker = endPos;
    }
    public void StartLerp(bool keepLerping)
    {
        lerping = true;
        lerpStartTime = Time.time;
        journeyLength = Vector3.Distance(startMarker, endMarker);

        if (keepLerping)
        {
            alwaysLerping = true;
        }
    }
}

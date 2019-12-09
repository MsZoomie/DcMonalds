using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [HideInInspector]
    public bool tap;
    [HideInInspector]
    public bool swipeLeft;
    [HideInInspector]
    public bool swipeRight;
    [HideInInspector]
    public bool swipeUp;
    [HideInInspector]
    public bool swipeDown;

    private bool isDraging;

    private Vector2 startPoint;
    private Vector2 swipeDelta;

    public float minSwipeDist = 100;

    private void Update()
    {
        tap = false;
        swipeLeft = false;
        swipeRight = false;
        swipeUp = false;
        swipeDown = false;

        #region Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            isDraging = true;
            tap = true;
            startPoint = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                tap = true;
                startPoint = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended
                || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }
        #endregion


        //if there is a swipe, calculate distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startPoint;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startPoint;
            }

           

            //if swipe is long enough, calculate direction
            if (swipeDelta.magnitude >= minSwipeDist)
            {
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                  
                    //horizontal
                    if (x < 0)
                        swipeLeft = true;
                    else
                        swipeRight = true;
                }
                else
                {
                    
                    //vertical
                    if (y < 0)
                        swipeDown = true;
                    else
                        swipeUp = true;
                }

                Reset();
            }
        }
    }


    private void Reset()
    {
        startPoint = Vector2.zero;
        swipeDelta = Vector2.zero;
        isDraging = false;
    }

}

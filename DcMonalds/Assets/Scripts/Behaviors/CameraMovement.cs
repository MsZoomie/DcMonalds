using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public bool onlyFollowZ;

    private Vector3 offset;
    
   
    private void Start()
    {
        SetThisOffset();
    }

    private void LateUpdate()
    {
        if (onlyFollowZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + offset.z);
        }
        else
        {
            transform.position = player.transform.position + offset;
        }
    }

    void SetThisOffset()
    {
        offset = transform.position - player.transform.position;
    }

}

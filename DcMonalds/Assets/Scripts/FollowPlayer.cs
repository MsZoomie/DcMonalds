using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not for camera movement!
/// Only GameObjects
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject[] followers;

    private float offset = 1.0f;


    private void LateUpdate()
    {
        float dist = player.transform.position.x - Camera.main.transform.position.x;

        if (dist >= offset)
        {
            foreach (var follower in followers)
            {
                follower.transform.position = Vector3.Lerp(follower.transform.position,
                                                         follower.transform.position + (Vector3.right * dist),
                                                         Time.deltaTime);
            }
        }
    }
}

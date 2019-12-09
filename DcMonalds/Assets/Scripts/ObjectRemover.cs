using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Level>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}

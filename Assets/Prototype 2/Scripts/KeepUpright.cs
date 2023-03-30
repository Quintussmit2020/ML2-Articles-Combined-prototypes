using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    void Start()
    {
        // Determine the current up direction of the object
        Vector3 upDirection = transform.up;

        // Create a rotation that aligns the object's up direction with the world up direction
        Quaternion targetRotation = Quaternion.FromToRotation(upDirection, Vector3.up);

        // Add a 180-degree rotation around the Z-axis
        targetRotation *= Quaternion.Euler(0, 0, 180);

        // Apply the rotation to the object's transform
        transform.rotation = targetRotation * transform.rotation;
    }
}
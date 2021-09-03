using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraFollow : MonoBehaviour
{
    public Transform transformToFolow;

    void LateUpdate()
    {
        UpdateFollow();
    }

    public void UpdateFollow()
    {
        if (transformToFolow)
        {
            transform.position = transformToFolow.position;
            transform.rotation = transformToFolow.rotation;
        }
    }
}
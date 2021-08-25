using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform transformToFolow;

    Vector3 localPositionFollow;

    private void Awake()
    {
        localPositionFollow =  transform.position - transformToFolow.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, transformToFolow.position + localPositionFollow, 1f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Range (0,1)][SerializeField] float lerp = 1;
    public Transform transformToFolow;

    [SerializeField] Vector3 localPositionFollow;

    void LateUpdate()
    {
        UpdateFollow();
    }

    public void UpdateFollow()
    {
        transform.position = Vector3.Lerp(transform.position, transformToFolow.position + localPositionFollow, lerp);
    }
}
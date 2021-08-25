using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorControlTransform : MonoBehaviour
{
    [SerializeField] Transform transformToControl;

#if UNITY_EDITOR

    private void Update()
    {
        if (!transformToControl)
            return;
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            transform.position = transformToControl.position;
            transform.rotation = transformToControl.rotation;
        }
    }
    void FixedUpdate()
    {
        if (!transformToControl)
            return;
        if (UnityEditor.EditorApplication.isPlaying)
        {
            transformToControl.position = transform.position;
            transformToControl.rotation = transform.rotation;
        }
        else
        {
            transform.position = transformToControl.position;
            transform.rotation = transformToControl.rotation;
        }
    }
#endif
}

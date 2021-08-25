using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    [SerializeField] Transform targetLimb;
    ConfigurableJoint configurableJoint;

    Quaternion targetInitRotation;

    private void Awake()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
        targetInitRotation = targetLimb.localRotation;
    }

    private void FixedUpdate()
    {
        configurableJoint.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * targetInitRotation;
    }
}

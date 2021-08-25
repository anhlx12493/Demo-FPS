using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHumanBehaviour : MonoBehaviour
{
    public Vector3 positionHipStart, position_IK_FootLeftStart, position_IK_FootRightStart, positionHipEnd, position_IK_FootLeftEnd, position_IK_FootRightEnd;

    public void SetPositionsStart()
    {
        HumanAnimationCreater creater = FindObjectOfType<HumanAnimationCreater>();
        if (!creater)
        {
            throw new System.Exception("HumanAnimationCreater not found");
        }
        positionHipStart = creater.humanControl.hip.localPosition;
        position_IK_FootLeftStart = creater.humanControl.IK_FootLeft.localPosition;
        position_IK_FootRightStart = creater.humanControl.IK_FootRight.localPosition;
    }

    public void SetPositionsEnd()
    {
        HumanAnimationCreater creater = FindObjectOfType<HumanAnimationCreater>();
        if (!creater)
        {
            throw new System.Exception("HumanAnimationCreater not found");
        }
        positionHipEnd = creater.humanControl.hip.localPosition;
        position_IK_FootLeftEnd = creater.humanControl.IK_FootLeft.localPosition;
        position_IK_FootRightEnd = creater.humanControl.IK_FootRight.localPosition;
    }
}

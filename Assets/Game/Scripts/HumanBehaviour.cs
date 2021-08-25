using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanControl
{
    public Transform hip, IK_FootLeft, IK_FootRight;
}

public abstract class HumanBehaviour
{
    [HideInInspector]
    public HumanControl control, targetControl;
    [HideInInspector]
    public Animator animator;
    public void Set(Animator animator, HumanControl control, HumanControl targetControl)
    {
        this.animator = animator;
        this.control = control;
        this.targetControl = targetControl;
    }


    public abstract void OnFixedUpdate();
}

[System.Serializable]
public class HumanWalk : HumanBehaviour
{
    bool isLeftStep, isStepUp;

    public InputHumanBehaviour InputWalkLeft, InputWalkRight;

    Vector3 maxHitHeight = new Vector3(0,1f,0);
    float scaleIKFootLeft,scaleIKFootRight,scaleHip;
    string checkAnimationName;
    float smoothChange;

    Vector3 positionControlHip, positionControlIKLegLeft, positionControlIkLegRight;

    public void Set(Animator animator, HumanControl control, HumanControl targetControl)
    {
        base.Set(animator, control, targetControl);
        checkAnimationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        animator.speed = 1.2f;
        positionControlHip = targetControl.hip.position;
        positionControlIKLegLeft = targetControl.IK_FootLeft.position;
        positionControlIkLegRight = targetControl.IK_FootRight.position;
    }


    public override void OnFixedUpdate()
    {
        CheckSide();
        UpdatePositionControl();
        UpdateScales();
        if (isLeftStep)
        {
            LeftStep();
        }
        else
        {
            RightStep();
        }
    }

    private void LeftStep()
    {

        targetControl.hip.transform.position = positionControlHip + Vector3.Lerp(GetHipPositionStart(), GetPositionHipTarget(), scaleHip) - Vector3.Lerp(GetHipPositionStart(), GetHipPositionEnd(), scaleHip);
        targetControl.hip.rotation = Quaternion.Lerp(targetControl.hip.rotation, control.hip.rotation, smoothChange);

        targetControl.IK_FootLeft.transform.position = positionControlIKLegLeft + Vector3.Lerp(GetIKLEftPositionStart(), GetPositionTarget(), scaleIKFootLeft) - Vector3.Lerp(GetIKLEftPositionStart(), GetIKLeftPositionEnd(), scaleIKFootLeft);
        targetControl.IK_FootLeft.rotation = Quaternion.Lerp(targetControl.IK_FootLeft.rotation, control.IK_FootLeft.rotation, smoothChange);

        targetControl.IK_FootRight.transform.position = positionControlIkLegRight + Vector3.Lerp(GetIKRightPositionStart(), GetPositionIKOtherFootTarget(), scaleIKFootRight) - Vector3.Lerp(GetIKRightPositionStart(), GetIKRightPositionEnd(), scaleIKFootRight);
        targetControl.IK_FootRight.rotation = Quaternion.Lerp(targetControl.IK_FootRight.rotation, control.IK_FootRight.rotation, smoothChange);
    }

    private void RightStep()
    {

        targetControl.hip.transform.position = positionControlHip + Vector3.Lerp(GetHipPositionStart(), GetPositionHipTarget(), scaleHip) - Vector3.Lerp(GetHipPositionStart(), GetHipPositionEnd(), scaleHip);
        targetControl.hip.rotation = Quaternion.Lerp(targetControl.hip.rotation, control.hip.rotation, smoothChange);

        targetControl.IK_FootRight.transform.position = positionControlIkLegRight + Vector3.Lerp(GetIKRightPositionStart(), GetPositionTarget(), scaleIKFootRight) - Vector3.Lerp(GetIKRightPositionStart(), GetIKRightPositionEnd(), scaleIKFootRight);
        targetControl.IK_FootRight.rotation = Quaternion.Lerp(targetControl.IK_FootRight.rotation, control.IK_FootRight.rotation, smoothChange);

        targetControl.IK_FootLeft.transform.position = positionControlIKLegLeft + Vector3.Lerp(GetIKLEftPositionStart(), GetPositionIKOtherFootTarget(), scaleIKFootLeft) - Vector3.Lerp(GetIKLEftPositionStart(), GetIKLeftPositionEnd(), scaleIKFootLeft);
        targetControl.IK_FootLeft.rotation = Quaternion.Lerp(targetControl.IK_FootLeft.rotation, control.IK_FootLeft.rotation, smoothChange);
    }

    private void UpdatePositionControl()
    {
        smoothChange = animator.speed * 0.3f;
        positionControlHip = Vector3.Lerp(positionControlHip, control.hip.position, smoothChange);
        positionControlIKLegLeft = Vector3.Lerp(positionControlIKLegLeft, control.IK_FootLeft.position, smoothChange);
        positionControlIkLegRight = Vector3.Lerp(positionControlIkLegRight, control.IK_FootRight.position, smoothChange);
    }


    private float GetAnimationProcess()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
    }

    private Vector3 GetHipPositionEnd()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.positionHipEnd + animator.transform.position;
        }
        else
        {
            return InputWalkRight.positionHipEnd + animator.transform.position;
        }
    }

    private Vector3 GetIKRightPositionEnd()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.position_IK_FootRightEnd + animator.transform.position;
        }
        else
        {
            return InputWalkRight.position_IK_FootRightEnd + animator.transform.position;
        }
    }

    private Vector3 GetIKLeftPositionEnd()
    {
        if (isLeftStep)
        {
            Debug.DrawLine(InputWalkLeft.position_IK_FootLeftEnd, InputWalkLeft.position_IK_FootLeftEnd + new Vector3(0, -10, 0), Color.blue);
            return InputWalkLeft.position_IK_FootLeftEnd + animator.transform.position;
        }
        else
        {
            return InputWalkRight.position_IK_FootLeftEnd + animator.transform.position;
        }
    }

    void CheckSide()
    {
        if (isLeftStep == animator.GetCurrentAnimatorStateInfo(0).IsName(checkAnimationName))
        {
            UpdateBodyPosition();
            isLeftStep = !isLeftStep;
        }
    }

    void UpdateBodyPosition()
    {
        if (isLeftStep)
        {
            animator.transform.position += GetPositionTarget() - animator.transform.position - InputWalkRight.position_IK_FootLeftStart;
        }
        else
        {
            animator.transform.position += GetPositionTarget() - animator.transform.position - InputWalkLeft.position_IK_FootRightStart;
        }
        positionControlHip = targetControl.hip.position;
        positionControlIKLegLeft = targetControl.IK_FootLeft.position;
        positionControlIkLegRight = targetControl.IK_FootRight.position;
    }

    void UpdateScales()
    {
        scaleHip = GetScaleHip();
        scaleIKFootLeft = GetScaleIKFootLeft();
        scaleIKFootRight = GetScaleIKFootRight();
    }

    private float GetScaleHip()
    {
        float raitioHipOnLastStep;
        if (isStepUp)
        {
            raitioHipOnLastStep = 0.5f;
        }
        else
        {
            raitioHipOnLastStep = 1f;
        }
        if (isLeftStep)
        {
            scaleHip = (positionControlHip.z - InputWalkLeft.positionHipStart.z - animator.transform.position.z) / (InputWalkLeft.positionHipEnd.z - InputWalkLeft.positionHipStart.z);
            scaleHip = (scaleHip - (1f - raitioHipOnLastStep)) * (1f / raitioHipOnLastStep);
            return scaleHip;
        }
        else
        {
            scaleHip = (positionControlHip.z - InputWalkRight.positionHipStart.z - animator.transform.position.z) / (InputWalkRight.positionHipEnd.z - InputWalkRight.positionHipStart.z);
            scaleHip = (scaleHip - (1f - raitioHipOnLastStep)) * (1f / raitioHipOnLastStep);
            return scaleHip;
        }
    }

    private float GetScaleIKFootLeft()
    {
        if (isLeftStep)
        {
            return (positionControlIKLegLeft.z - InputWalkLeft.position_IK_FootLeftStart.z - animator.transform.position.z) / (InputWalkLeft.position_IK_FootLeftEnd.z - InputWalkLeft.position_IK_FootLeftStart.z);
        }
        else
        {
            float raitioIKFootLeftOnLastStep = 0.5f;
            scaleIKFootLeft = (positionControlIKLegLeft.z - InputWalkRight.position_IK_FootLeftStart.z - animator.transform.position.z) / (InputWalkRight.position_IK_FootLeftEnd.z - InputWalkRight.position_IK_FootLeftStart.z);
            scaleIKFootLeft = (scaleIKFootLeft - (1f - raitioIKFootLeftOnLastStep)) * (1f / raitioIKFootLeftOnLastStep);
            return scaleHip;
        }
    }

    private float GetScaleIKFootRight()
    {
        if (isLeftStep)
        {
            float raitioIKFootRightOnLastStep = 0.5f;
            scaleIKFootRight = (positionControlIkLegRight.z - InputWalkLeft.position_IK_FootRightStart.z - animator.transform.position.z) / (InputWalkLeft.position_IK_FootRightEnd.z - InputWalkLeft.position_IK_FootRightStart.z);
            scaleIKFootRight = (scaleIKFootRight - (1f - raitioIKFootRightOnLastStep)) * (1f / raitioIKFootRightOnLastStep);
            return scaleIKFootRight;
        }
        else
        {
            return (positionControlIkLegRight.z - InputWalkRight.position_IK_FootRightStart.z - animator.transform.position.z) / (InputWalkLeft.position_IK_FootRightEnd.z - InputWalkRight.position_IK_FootLeftStart.z);
        }
    }

    private Vector3 GetHipPositionStart()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.positionHipStart + animator.transform.position;
        }
        else
        {
            return InputWalkRight.positionHipStart + animator.transform.position;
        }
    }

    private Vector3 GetIKRightPositionStart()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.position_IK_FootRightStart + animator.transform.position;
        }
        else
        {
            return InputWalkRight.position_IK_FootRightStart + animator.transform.position;
        }
    }

    private Vector3 GetIKLEftPositionStart()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.position_IK_FootLeftStart + animator.transform.position;
        }
        else
        {
            return InputWalkRight.position_IK_FootLeftStart + animator.transform.position;
        }
    }

    private Vector3 GetPositionTarget()
    {
        if (isLeftStep) {
            if (Physics.Raycast(InputWalkLeft.position_IK_FootLeftEnd + animator.transform.position + maxHitHeight, Vector3.down, out RaycastHit hit, float.MaxValue))
            {
                isStepUp = hit.point.y - animator.transform.position.y >=  0;
                return hit.point;
            }
            return InputWalkLeft.position_IK_FootLeftEnd + animator.transform.position;
        }
        else
        {
            if (Physics.Raycast(InputWalkRight.position_IK_FootRightEnd + animator.transform.position + maxHitHeight, Vector3.down, out RaycastHit hit, float.MaxValue))
            {
                isStepUp = hit.point.y - animator.transform.position.y >= 0;
                return hit.point;
            }
            return InputWalkRight.position_IK_FootRightEnd + animator.transform.position;
        }
    }

    private Vector3 GetPositionHipTarget()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.positionHipEnd + GetPositionTarget() - InputWalkLeft.position_IK_FootLeftEnd;
        }
        else
        {
            return InputWalkRight.positionHipEnd + GetPositionTarget() - InputWalkRight.position_IK_FootRightEnd;
        }
    }

    private Vector3 GetPositionIKOtherFootTarget()
    {
        if (isLeftStep)
        {
            return InputWalkLeft.position_IK_FootRightEnd + GetPositionTarget() - InputWalkLeft.position_IK_FootLeftEnd;
        }
        else
        {
            return InputWalkRight.position_IK_FootLeftEnd + GetPositionTarget() - InputWalkRight.position_IK_FootRightEnd;
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitFootOnTheGround : MonoBehaviour
{

    Animator animator;
    [Range (0,1)]
    [SerializeField] float speed, moveLerp, fixDistanceFootToGround,distanceStep,distanceFoot,maxTallestStep;
    [Range(1, 20)]
    [SerializeField] int maxNumberRaycast;
    [SerializeField] LayerMask layer;
    [SerializeField] Rigidbody rigidMove;

    float sum;
    float lastSpeed;

    Vector3 positionIKLeftFoot, positionIKRightFoot, distanceBodyUp;

    AnimationCurve curveLeft,curveRight;

    RaycastHit hit;

    bool isOnLeftStep, isOnRightStep, isLeftStepBehind, isBodyUp;

    float scaseUp = 2;

    Transform rootTransformLeft,rootTransformRight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sum = GetSumOfEquation(Mathf.Clamp(0.1f * speed, 0.01f, 1f));
        animator.speed = speed;
        rootTransformLeft = new GameObject().transform;
        rootTransformRight = new GameObject().transform;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (speed != lastSpeed)
        {
            lastSpeed = speed;
            animator.speed = speed;
            sum = GetSumOfEquation(Mathf.Clamp(0.1f * speed,0.01f,1f));
        }
        //Vector3 localPositionFoot = new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x) * distanceFoot;
        //Vector3 position = transform.position + localPositionFoot;
        //float distanceRaycast = maxTallestStep / maxNumberRaycast;
        //Vector3 position1, position2, position3, position4;
        //position1 = position;
        //position2 = position;
        //bool noHit = true;
        //for (int i = 0; i < maxNumberRaycast; i++)
        //{
        //    position.y += distanceRaycast;
        //    if (Physics.Raycast(position, transform.forward * distanceStep, out hit, distanceStep))
        //    {
        //        noHit = false;
        //        position2 = hit.point;
        //        Debug.DrawLine(position, hit.point, Color.red);
        //    }
        //    else
        //    {
        //        Vector3 stepOnPosition = transform.forward * distanceStep + transform.position + localPositionFoot;
        //        stepOnPosition.y = position.y;
        //        if (Physics.Raycast(stepOnPosition, Vector3.down, out hit, 2))
        //        {
        //            position4 = hit.point;
        //            Debug.DrawLine(hit.point + Vector3.up, hit.point, Color.red);
        //            position2.y = stepOnPosition.y;
        //            if (noHit)
        //            {
        //                position2 = Vector3.Lerp(position2, stepOnPosition, 0.33f);
        //            }
        //            position3 = Vector3.Lerp(stepOnPosition,position2,0.5f);
        //            Debug.DrawLine(position1, position2, Color.green);
        //            Debug.DrawLine(position2, position3, Color.green);
        //            Debug.DrawLine(position3, position4, Color.green);
        //        }
        //        break;
        //    }
        //}
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (speed == 0)
            return;
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);


        if (isOnLeftStep)
        {
            positionIKLeftFoot = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
            rootTransformLeft.rotation = transform.rotation;
            positionIKLeftFoot.y = curveLeft.Evaluate(rootTransformLeft.InverseTransformPoint(positionIKLeftFoot).z) + fixDistanceFootToGround;
            if (animator.GetIKPosition(AvatarIKGoal.LeftFoot).y < positionIKLeftFoot.y)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, positionIKLeftFoot);
            }
            isLeftStepBehind = false;
        }
        else
        {
            if (isOnRightStep)
            {
                positionIKRightFoot = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                rootTransformRight.rotation = transform.rotation;
                positionIKRightFoot.y = curveRight.Evaluate(rootTransformRight.InverseTransformPoint(positionIKRightFoot).z) + fixDistanceFootToGround;
                if (animator.GetIKPosition(AvatarIKGoal.RightFoot).y < positionIKRightFoot.y)
                {
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, positionIKRightFoot);
                }
                isLeftStepBehind = true;
            }
            else
            {
                if (isLeftStepBehind)
                {
                    positionIKLeftFoot = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                    rootTransformLeft.rotation = transform.rotation;
                    positionIKLeftFoot.y = curveLeft.Evaluate(rootTransformLeft.InverseTransformPoint(positionIKLeftFoot).z) + fixDistanceFootToGround;
                    if (animator.GetIKPosition(AvatarIKGoal.LeftFoot).y < positionIKLeftFoot.y)
                    {
                        animator.SetIKPosition(AvatarIKGoal.LeftFoot, positionIKLeftFoot);
                    }

                    positionIKRightFoot = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                    rootTransformRight.rotation = transform.rotation;
                    positionIKRightFoot.y = curveRight.Evaluate(rootTransformRight.InverseTransformPoint(positionIKRightFoot).z) + fixDistanceFootToGround;
                    if (animator.GetIKPosition(AvatarIKGoal.RightFoot).y < positionIKRightFoot.y)
                    {
                        animator.SetIKPosition(AvatarIKGoal.RightFoot, positionIKRightFoot);
                    }
                }
                else
                {
                    positionIKRightFoot = animator.GetIKPosition(AvatarIKGoal.RightFoot);
                    rootTransformRight.rotation = transform.rotation;
                    positionIKRightFoot.y = curveRight.Evaluate(rootTransformRight.InverseTransformPoint(positionIKRightFoot).z) + fixDistanceFootToGround;
                    if (animator.GetIKPosition(AvatarIKGoal.RightFoot).y < positionIKRightFoot.y)
                    {
                        animator.SetIKPosition(AvatarIKGoal.RightFoot, positionIKRightFoot);
                    }

                    positionIKLeftFoot = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
                    rootTransformLeft.rotation = transform.rotation;
                    positionIKLeftFoot.y = curveLeft.Evaluate(rootTransformLeft.InverseTransformPoint(positionIKLeftFoot).z) + fixDistanceFootToGround;
                    if (animator.GetIKPosition(AvatarIKGoal.LeftFoot).y < positionIKLeftFoot.y)
                    {
                        animator.SetIKPosition(AvatarIKGoal.LeftFoot, positionIKLeftFoot);
                    }
                }


            }
        }

        if (isBodyUp)
        {
            isBodyUp = false;
            scaseUp = 0;
        }
        if (scaseUp <= 1f)
        {
            transform.position += distanceBodyUp * scaseUp * (1 - scaseUp) / sum;
            scaseUp += 0.1f * speed;
        }
        SetPositionIKLeft();
        SetPositionIKRight();
        Vector3 position = rigidMove.transform.position;
        position.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, position, moveLerp);
    }

    private void LateUpdate()
    {
        rigidMove.transform.position = transform.position;
    }

    private void SetPositionIKRight()
    {

    }

    private void SetPositionIKLeft()
    {

    }

    public void OnLeftFootStepOnGround()
    {
        isOnLeftStep = false;
    }

    public void OnRightFootStepOnGround()
    {
        isOnRightStep = false;
    }

    public void OnLeftStepStart()
    {
        isOnLeftStep = true;
        SetCurveToLeftStep();
        SetCurveToRightStep();
    }

    public void OnRightStepStart()
    {
        isOnRightStep = true;
        SetCurveToLeftStep();
        SetCurveToRightStep();
    }

    public void OnBodyUp()
    {
        isBodyUp = true;
    }

    float GetSumOfEquation(float speed)
    {
        float sum = 0;
        for(float x = speed; x <= 1f; x += speed)
        {
            sum += (1f - x) * x;
        }
        return sum;
    }

    void SetCurveToLeftStep()
    {
        Vector3 localPositionFoot = new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x) * distanceFoot;
        Vector3 position = transform.position + localPositionFoot;
        float distanceRaycast = maxTallestStep / maxNumberRaycast;
        Vector3 position1, position2, position3, position4;
        position1 = position;
        position2 = position;
        bool noHit = true;
        for (int i = 0; i < maxNumberRaycast; i++)
        {
            position.y += distanceRaycast;
            if (Physics.Raycast(position, transform.forward * distanceStep, out hit, distanceStep, layer))
            {
                noHit = false;
                position2 = hit.point;
            }
            else
            {
                Vector3 stepOnPosition = transform.forward * distanceStep + transform.position + localPositionFoot;
                stepOnPosition.y = position.y;
                if (Physics.Raycast(stepOnPosition, Vector3.down, out hit, 2, layer))
                {
                    position4 = hit.point;
                    position2.y = stepOnPosition.y;
                    if (noHit)
                    {
                        position2 = Vector3.Lerp(position2, stepOnPosition, 0.33f);
                    }
                    distanceBodyUp.y = position4.y - position1.y;
                    position3 = Vector3.Lerp(stepOnPosition, position2, 0.5f);
                    Debug.DrawLine(position1, position2, Color.green,5f);
                    Debug.DrawLine(position2, position3, Color.green, 5f);
                    Debug.DrawLine(position3, position4, Color.green, 5f);
                    curveLeft = new AnimationCurve(new Keyframe[] { new Keyframe(transform.InverseTransformPoint(position1).z, position1.y - fixDistanceFootToGround), new Keyframe(transform.InverseTransformPoint(position2).z, position2.y), new Keyframe(transform.InverseTransformPoint(position3).z, position3.y), new Keyframe(transform.InverseTransformPoint(position4).z, position4.y, -1, 1) });
                }
                break;
            }
        }
        rootTransformLeft.position = transform.position;
    }

    void SetCurveToRightStep()
    {
        Vector3 localPositionFoot = new Vector3(transform.forward.z, transform.forward.y, -transform.forward.x) * distanceFoot;
        Vector3 position = transform.position + localPositionFoot;
        float distanceRaycast = maxTallestStep / maxNumberRaycast;
        Vector3 position1, position2, position3, position4;
        position1 = position;
        position2 = position;
        bool noHit = true;
        for (int i = 0; i < maxNumberRaycast; i++)
        {
            position.y += distanceRaycast;
            if (Physics.Raycast(position, transform.forward * distanceStep, out hit, distanceStep, layer))
            {
                noHit = false;
                position2 = hit.point;
            }
            else
            {
                Vector3 stepOnPosition = transform.forward * distanceStep + transform.position + localPositionFoot;
                stepOnPosition.y = position.y;
                if (Physics.Raycast(stepOnPosition, Vector3.down, out hit, 2, layer))
                {
                    position4 = hit.point;
                    position2.y = stepOnPosition.y;
                    if (noHit)
                    {
                        position2 = Vector3.Lerp(position2, stepOnPosition, 0.33f);
                    }
                    distanceBodyUp.y = position4.y - position1.y;
                    position3 = Vector3.Lerp(stepOnPosition, position2, 0.5f);
                    Debug.DrawLine(position1, position2, Color.blue,5f);
                    Debug.DrawLine(position2, position3, Color.blue, 5f);
                    Debug.DrawLine(position3, position4, Color.blue, 5f);
                    curveRight = new AnimationCurve(new Keyframe[] { new Keyframe(transform.InverseTransformPoint(position1).z, position1.y - fixDistanceFootToGround), new Keyframe(transform.InverseTransformPoint(position2).z, position2.y), new Keyframe(transform.InverseTransformPoint(position3).z, position3.y), new Keyframe(transform.InverseTransformPoint(position4).z, position4.y, -1, 1) });
                }
                break;
            }
        }
        rootTransformRight.position = transform.position;
    }
}

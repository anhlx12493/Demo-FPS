using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Man : Character
{
    [Range(0, 1)]
    [SerializeField] float speed, fixDistanceFootToGround;
    [SerializeField] LayerMask layerHit;

    RaycastHit hit;
    Animator animator;
    float positionIKLeftY, positionIKRightY, distanceYBetweenStandFootAndBody;
    float lastSpeed;
    float speedFootUp = 0.1f;
    bool isFootLeftStanding, isFootRightStanding;

    Transform test;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = speed;
        test = new GameObject().transform;
    }

    private void Update()
    {
        if (speed != lastSpeed)
        {
            lastSpeed = speed;
            animator.speed = speed;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

        Vector3 positionIKLeft = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 positionIKRight = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        if (isFootLeftStanding)
        {
            distanceYBetweenStandFootAndBody = transform.position.y - positionIKLeft.y;
        }
        if (isFootRightStanding)
        {
            distanceYBetweenStandFootAndBody = transform.position.y - positionIKRight.y;
        }
        if (Physics.Raycast(positionIKLeft + transform.forward * 0.1f + Vector3.up, Vector3.down,out hit, 3f, layerHit))
        {
            Debug.DrawLine(positionIKLeft + transform.forward * 0.1f + Vector3.up, hit.point,Color.green,5);
            if(positionIKLeftY < hit.point.y)
            {
                positionIKLeftY += speedFootUp * speed; 
                if (positionIKLeftY > hit.point.y)
                {
                    positionIKLeftY = hit.point.y;
                }

            }
            else if (positionIKLeftY > hit.point.y)
            {
                positionIKLeftY -= speedFootUp * speed;
                if (positionIKLeftY < hit.point.y)
                {
                    positionIKLeftY = hit.point.y;
                }
            }
            if (positionIKLeft.y < positionIKLeftY + fixDistanceFootToGround)
            {
                positionIKLeft.y = positionIKLeftY + fixDistanceFootToGround;
            }
            else {
                if (isFootLeftStanding)
                {
                    distanceYBetweenStandFootAndBody -= positionIKLeft.y - positionIKLeftY - fixDistanceFootToGround;
                } 
            }
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, positionIKLeft);
        }

        if(Physics.Raycast(positionIKRight + transform.forward * 0.1f + Vector3.up, Vector3.down,out hit, 3f, layerHit))
        {
            Debug.DrawLine(positionIKRight + transform.forward * 0.1f + Vector3.up, hit.point, Color.blue,5);
            if (positionIKRightY < hit.point.y)
            {
                positionIKRightY += speedFootUp * speed; 
                if (positionIKRightY > hit.point.y)
                {
                    positionIKRightY = hit.point.y;
                }

            }
            else if (positionIKRightY > hit.point.y)
            {
                positionIKRightY -= speedFootUp * speed;
                if (positionIKRightY < hit.point.y)
                {
                    positionIKRightY = hit.point.y;
                }
            }
            if (positionIKRight.y < positionIKRightY + fixDistanceFootToGround)
            {
                positionIKRight.y = positionIKRightY + fixDistanceFootToGround;
            }
            else
            {
                if (isFootRightStanding)
                {
                    distanceYBetweenStandFootAndBody -= positionIKRight.y - positionIKRightY - fixDistanceFootToGround;
                }
            }
            animator.SetIKPosition(AvatarIKGoal.RightFoot, positionIKRight);
        }

        if (Physics.Raycast(transform.position + transform.forward + Vector3.up * 3, Vector3.down, out hit, 6, layerHit))
        {
            if (isRunning)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.Cross(hit.normal, new Vector3(-transform.forward.z, transform.forward.y,transform.forward.x)).normalized), 1f * speed);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x)).normalized), 1f * speed);
            }
        }

        if (isFootLeftStanding)
        {
            Vector3 bodyPosition = transform.position;
            bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y + distanceYBetweenStandFootAndBody;
            transform.position = Vector3.Lerp(transform.position,bodyPosition,0.3f * speed);
        }
        if (isFootRightStanding)
        {
            Vector3 bodyPosition = transform.position;
            bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.RightFoot).y + distanceYBetweenStandFootAndBody;
            transform.position = Vector3.Lerp(transform.position, bodyPosition, 0.3f * speed);
        }
    }


    public void OnLeftFootStepOnGround()
    {
        isFootLeftStanding = true;
        isFootRightStanding = false;
    }

    public void OnRightFootStepOnGround()
    {
        isFootRightStanding = true;
        isFootLeftStanding = false;
    }


    public void OnLeftFootOutGround()
    {
        isFootLeftStanding = false;
    }

    public void OnRightFootOutGround()
    {
        isFootRightStanding = false;
    }

    public void OnLeftStepStart()
    {
    }

    public void OnRightStepStart()
    {
    }

    public void OnBodyUp()
    {
    }
}

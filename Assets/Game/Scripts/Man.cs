using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Man : Character
{
    [Range(0, 1)]
    [SerializeField] float speed, fixDistanceFootToGround;
    [SerializeField] LayerMask layerHit;
    [SerializeField] Rigidbody rigidCollider;

    RaycastHit hit;
    Animator animator;
    float positionIKLeftY, positionIKRightY, distanceYBetweenStandFootAndBody;
    float lastSpeed;
    float speedFootUp = 0.1f;
    float speedMove = 0f;
    bool isFootLeftStanding, isFootRightStanding;

    bool isMoveLeft, isMoveRight, isMoveUp, isMoveDown, isFaster, isLowwer;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = speed;
    }

    private void Update()
    {
        if (speed != lastSpeed)
        {
            lastSpeed = speed;
            animator.speed = speed;
        }
        ChekMove();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isBehaviourAbleChange)
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
            if (Physics.Raycast(positionIKLeft + transform.forward * 0.1f + Vector3.up, Vector3.down, out hit, 3f, layerHit))
            {
                Debug.DrawLine(positionIKLeft + transform.forward * 0.1f + Vector3.up, hit.point, Color.green, 5);
                if (positionIKLeftY < hit.point.y)
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
                else
                {
                    if (isFootLeftStanding)
                    {
                        distanceYBetweenStandFootAndBody -= positionIKLeft.y - positionIKLeftY - fixDistanceFootToGround;
                    }
                }
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, positionIKLeft);
            }

            if (Physics.Raycast(positionIKRight + transform.forward * 0.1f + Vector3.up, Vector3.down, out hit, 3f, layerHit))
            {
                Debug.DrawLine(positionIKRight + transform.forward * 0.1f + Vector3.up, hit.point, Color.blue, 5);
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
                if (behaviour == Behaviour.run)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.Cross(hit.normal, new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x)).normalized), 1f * speed);
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.Cross(Vector3.up, new Vector3(-transform.forward.z, transform.forward.y, transform.forward.x)).normalized), 1f * speed);
                }
            }
            Vector3 bodyPosition;
            if (isFootLeftStanding)
            {
                bodyPosition = transform.position;
                bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y + distanceYBetweenStandFootAndBody;
                transform.position = Vector3.Lerp(transform.position, bodyPosition, 0.3f * speed);
            }
            if (isFootRightStanding)
            {
                bodyPosition = transform.position;
                bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.RightFoot).y + distanceYBetweenStandFootAndBody;
                transform.position = Vector3.Lerp(transform.position, bodyPosition, 0.3f * speed);
            }
        }
        transform.position = rigidCollider.transform.position;
    }

    private void FixedUpdate()
    {
        if (isBehaviourAbleChange)
        {
            rigidCollider.transform.position = transform.position;
        }
    }


    public void OnLeftFootStepOnGround()
    {
        isFootLeftStanding = true;
        isFootRightStanding = false;
        CheckJumpDown();
    }

    public void OnRightFootStepOnGround()
    {
        isFootRightStanding = true;
        isFootLeftStanding = false;
        CheckJumpDown();
    }


    public void OnLeftFootOutGround()
    {
        isFootLeftStanding = false;
        CheckJumpDown();
    }

    public void OnRightFootOutGround()
    {
        isFootRightStanding = false;
        CheckJumpDown();
    }

    public void OnLeftStepStart()
    {
        CheckJumpDown();
    }

    public void OnRightStepStart()
    {
        CheckJumpDown();
    }

    public void OnBodyUp()
    {
    }

    void CheckJumpDown()
    {
        if (isBehaviourAbleChange)
        {
            if (Physics.Raycast(transform.position + transform.forward * 0.1f + Vector3.up, Vector3.down, out hit, 100, layerHit))
            {
                if (transform.position.y - hit.point.y > 1)
                {
                    isBehaviourAbleChange = false;
                    SetBehaviour(Behaviour.jumpDown);
                    animator.SetTrigger("JumpDown");
                    StartCoroutine(JumpFling());
                }
            }
        }
    }

    IEnumerator JumpFling()
    {
        Rigidbody physic = new GameObject().AddComponent<Rigidbody>();
        physic.transform.position = transform.position;
        physic.AddForce(transform.forward * 10);
        float startPositionYFall = transform.position.y;
        while (Physics.Raycast(transform.position, Vector3.down, out hit, 100, layerHit))
        {
            rigidCollider.transform.position = physic.transform.position;
            if (startPositionYFall - transform.position.y > 1 && physic.transform.position.y <= hit.point.y)
            {
                animator.SetTrigger("Landing");
                transform.position = rigidCollider.transform.position = hit.point;
                Destroy(physic.gameObject);
                isBehaviourAbleChange = true;
                positionIKLeftY = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y;
                positionIKRightY = animator.GetIKPosition(AvatarIKGoal.RightFoot).y;
                yield break;
            }
            yield return null;
        }
    }

    void ChekMove()
    {
        if (isMoveUp)
        {
            if (isFaster)
            {
                speedMove += 0.1f;
                if (speedMove > 2)
                    speedMove = 2;
                animator.SetFloat("Speed", speedMove);
                SetBehaviour(Behaviour.run);
            }
            else
            {
                if (speedMove > 1)
                {
                    speedMove -= 0.1f;
                    if (speedMove < 1)
                        speedMove = 1;
                }
                else
                {
                    speedMove = 1;
                }
                animator.SetFloat("Speed", speedMove);
                SetBehaviour(Behaviour.walk);
            }
        }
        else
        {
            speedMove -= 0.1f;
            if (speedMove < 0)
                speedMove = 0;
            animator.SetFloat("Speed", speedMove);
            SetBehaviour(Behaviour.idle);
        }
        isMoveLeft = false;
        isMoveRight = false;
        isMoveUp = false;
        isMoveDown = false;
        isFaster = false;
        isLowwer = false;
    }

    public override void MoveLeft()
    {
        isMoveLeft = true;
    }

    public override void MoveRight()
    {
        isMoveRight = true;
    }

    public override void MoveUp()
    {
        isMoveUp = true;
    }

    public override void MoveDown()
    {
        isMoveDown = true;
    }

    public override void Faster()
    {
        isFaster = true;
    }

    public override void Lowwer()
    {
        isLowwer = true;
    }
}

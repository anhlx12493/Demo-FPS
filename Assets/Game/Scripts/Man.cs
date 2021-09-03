using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Man : Character
{
    [Range(0, 1)]
    [SerializeField] float speed, fixDistanceFootToGround, speedBody;
    [SerializeField] Rigidbody rigidCollider;

    Animator animator;
    float positionIKLeftY, positionIKRightY, distanceYBetweenStandFootAndBody;
    float lastSpeed;
    float speedFootUp = 0.1f;
    bool isFootLeftStanding, isFootRightStanding;


    string lastTrigger;

    Transform copyTransform;

    private void Awake()
    {
        SetUp();
        animator = GetComponent<Animator>();
        animator.speed = speed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (speed != lastSpeed)
        {
            lastSpeed = speed;
            animator.speed = speed;
        }
        MoveNormal();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (gunUsing)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetLookAtWeight(1);
            gunUsing.transformGunHolder.LookAt(targetAim);
            gunUsing.transformAim.LookAt(targetAim);
            animator.SetLookAtPosition(targetAim);
            animator.SetIKPosition(AvatarIKGoal.RightHand, gunUsing.positionGrip);
            animator.SetIKRotation(AvatarIKGoal.RightHand, gunUsing.rotationGrip);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, gunUsing.positionForend);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, gunUsing.rotationForend);
        }
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
            Vector3 bodyPosition;
            if (isFootLeftStanding)
            {
                bodyPosition = transform.position;
                bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y + distanceYBetweenStandFootAndBody;
                transform.position = Vector3.MoveTowards(transform.position, bodyPosition, speedBody * speed);
            }
            if (isFootRightStanding)
            {
                bodyPosition = transform.position;
                bodyPosition.y = animator.GetIKPosition(AvatarIKGoal.RightFoot).y + distanceYBetweenStandFootAndBody;
                transform.position = Vector3.MoveTowards(transform.position, bodyPosition, speedBody * speed);
            }
            transform.position = rigidCollider.transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (isBehaviourAbleChange)
        {
            rigidCollider.transform.position = transform.position;
        }
        else
        {
            if (copyTransform)
            {
                transform.position = copyTransform.position;
            }
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
                    BehaviourJumpDown();
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
        copyTransform = physic.transform;
        float startPositionYFall = transform.position.y;
        while (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 100, layerHit))
        {
            if (startPositionYFall - transform.position.y > 1 && physic.transform.position.y <= hit.point.y)
            {
                SetAnimationStrigger("Landing");
                transform.position = rigidCollider.transform.position = hit.point;
                Destroy(physic.gameObject);
                positionIKLeftY = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y;
                positionIKRightY = animator.GetIKPosition(AvatarIKGoal.RightFoot).y;
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("JumpDownLanding"));
                ActiveBehaviour(true);
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        SetAnimationStrigger("Landing");
        positionIKLeftY = animator.GetIKPosition(AvatarIKGoal.LeftFoot).y;
        positionIKRightY = animator.GetIKPosition(AvatarIKGoal.RightFoot).y;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("JumpDownLanding"));
        ActiveBehaviour(true);
    }

    void SetAnimationStrigger(string name)
    {
        animator.ResetTrigger(lastTrigger);
        lastTrigger = name;
        animator.SetTrigger(lastTrigger);
    }

    void ActiveBehaviour(bool isActive)
    {
        isBehaviourAbleChange = isActive;
    }

    bool AimGun()
    {
        if (!gunUsing)
            return false;
        float angle = transform.rotation.eulerAngles.y - rotationLook.eulerAngles.y;
        if (angle < 0)
        {
            angle += 360;
            if (angle < 90)
            {
                animator.SetFloat("HipRotate", 1 - angle / 90f);
            }
            else if (angle < 180)
            {
                animator.SetFloat("HipRotate", 0);
            }
            else
            {
                animator.SetFloat("HipRotate", 1);
            }
        }
        else if (angle < 90)
        {
            animator.SetFloat("HipRotate", 1 - angle / 90f);
        }
        else
        {
            animator.SetFloat("HipRotate", 0);
        }
        return true;
    }

    protected override void BehaviourIdle()
    {
        if (AimGun())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 0.3f);
        }
        if (behaviour == Behaviour.idle || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.idle);
        SetAnimationStrigger("Idle");
    }

    protected override void BehaviourWalk()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 0.3f);
            if (behaviour == Behaviour.walkLeft45 || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.walkLeft45);
            SetAnimationStrigger("WalkLeft45");
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            if (behaviour == Behaviour.walk || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.walk);
            SetAnimationStrigger("Walk");
        }
    }

    protected override void BehaviourRun()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 0.3f);
            if (behaviour == Behaviour.runLeft45 || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.runLeft45);
            SetAnimationStrigger("RunLeft45");
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            if (behaviour == Behaviour.run || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.run);
            SetAnimationStrigger("Run");
        }
    }

    protected override void BehaviourWalkRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward.normalized, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 89, 0), 3);
        if (behaviour == Behaviour.walk || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walk);
        SetAnimationStrigger("Walk");
    }

    protected override void BehaviourWalkLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, -transform.forward.normalized, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 89, 0), 3);
        if (behaviour == Behaviour.walkBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walkBack);
        SetAnimationStrigger("WalkBack");
    }

    protected override void BehaviourRunRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward.normalized, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 89, 0), 3);
        if (behaviour == Behaviour.run || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.run);
        SetAnimationStrigger("Run");
    }

    protected override void BehaviourRunLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, -transform.forward.normalized, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 89, 0), 3);
        if (behaviour == Behaviour.runBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.runBack);
        SetAnimationStrigger("RunBack");
    }

    protected override void BehaviourWalkBackWard()
    {
        if (Physics.Raycast(transform.position + Vector3.up, -transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
        if (behaviour == Behaviour.walkBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walkBack);
        SetAnimationStrigger("WalkBack");
    }

    protected override void BehaviourRunBackWard()
    {
        if (Physics.Raycast(transform.position + Vector3.up, -transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
        if (behaviour == Behaviour.runBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.runBack);
        SetAnimationStrigger("RunBack");
    }

    protected override void BehaviourWalkUpRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0,45,0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 3);
        if (behaviour == Behaviour.walk || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walk);
        SetAnimationStrigger("Walk");
    }

    protected override void BehaviourWalkUpLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, -45, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 0.3f);
            if (behaviour == Behaviour.walkLeft45 || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.walkLeft45);
            SetAnimationStrigger("WalkLeft45");
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y - 45, 0), 3);
            if (behaviour == Behaviour.walk || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.walk);
            SetAnimationStrigger("Walk");
        }
    }

    protected override void BehaviourWalkBackRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, 135, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y - 45, 0), 3);
        if (behaviour == Behaviour.walkBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walkBack);
        SetAnimationStrigger("WalkBack");
    }

    protected override void BehaviourWalkBackLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, -135, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 3);
        if (behaviour == Behaviour.walkBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.walkBack);
        SetAnimationStrigger("WalkBack");
    }

    protected override void BehaviourRunUpRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, 45, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 3);
        if (behaviour == Behaviour.run || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.run);
        SetAnimationStrigger("Run");
    }

    protected override void BehaviourRunUpLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, -45, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 0.3f);
            if (behaviour == Behaviour.runLeft45 || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.runLeft45);
            SetAnimationStrigger("RunLeft45");
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y - 45, 0), 3);
            if (behaviour == Behaviour.run || !isBehaviourAbleChange)
                return;
            SetBehaviour(Behaviour.run);
            SetAnimationStrigger("Run");
        }
    }

    protected override void BehaviourRunBackRight()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, 135, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y - 45, 0), 3);
        if (behaviour == Behaviour.runBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.runBack);
        SetAnimationStrigger("RunBack");
    }

    protected override void BehaviourRunBackLeft()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, -135, 0) * transform.forward, out hit, 0.5f, layerHit))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y, 0), 3);
            BehaviourIdle();
            return;
        }
        if (AimGun())
        {
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationLook.eulerAngles.y + 45, 0), 3);
        if (behaviour == Behaviour.runBack || !isBehaviourAbleChange)
            return;
        SetBehaviour(Behaviour.runBack);
        SetAnimationStrigger("RunBack");
    }

    void BehaviourJumpDown()
    {
        if (behaviour == Behaviour.jumpDown)
            return;
        SetBehaviour(Behaviour.jumpDown);
        SetAnimationStrigger("JumpDown");
        ActiveBehaviour(false);
    }

    protected override void BehaviourDead()
    {
        animator.enabled = false;
    }
}

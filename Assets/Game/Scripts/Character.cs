using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System;

public interface ObserverCharacterDead
{
    public void OnCharacterDead(Character character);
}


public abstract class Character : NetworkBehaviour
{
    List<ObserverCharacterDead> observerCharacterDeads = new List<ObserverCharacterDead>();

    public enum Behaviour { idle,walk,walkLeft45,run,runLeft45,walkBack,runBack,left,right,jumpUp,jumpDown,jumpOver};
    public Behaviour behaviour
    {
        get
        {
            return _behaviour;
        }
    }
    Behaviour _behaviour;
    protected bool isBehaviourAbleChange = true;
    [HideInInspector]
    public Quaternion rotationLook;

    public void SetBehaviour(Behaviour behaviour)
    {
        if (isBehaviourAbleChange)
        {
            _behaviour = behaviour;
        }
    }

    bool isIdle,isMoveLeft, isMoveRight, isMoveUp, isMoveDown, isFaster, isLowwer;

    public Gun gunUsing;
    public CameraFollow cameraFollow;

    [HideInInspector]
    public Vector3 targetAim;

    protected RaycastHit hit;
    [SerializeField] protected LayerMask layerHit,layerAim;

    private float _health, _maxHealth;

    public float health
    {
        get
        {
            return _health;
        }
        private set
        {
            if (value < 0)
                _health = 0;
            else if (value > _maxHealth)
                health = _maxHealth;
            else
                health = value;
        }
    }

    protected void SetUp()
    {
    }

    public void ResigterObserverDead(ObserverCharacterDead observer)
    {
        if (!observerCharacterDeads.Contains(observer))
        {
            observerCharacterDeads.Add(observer);
        }
    }

    public void NotifiObserverDead()
    {
        foreach(ObserverCharacterDead observer in observerCharacterDeads)
        {
            observer.OnCharacterDead(this);
        }
    }

    internal void Dame(float hit)
    {
        if (hit <= 0)
            return;
        health -= hit;
    }

    public void MoveLeft()
    {
        isMoveLeft = true;
    }

    public void MoveRight()
    {
        isMoveRight = true;
    }

    public void MoveUp()
    {
        isMoveUp = true;
    }

    public void MoveDown()
    {
        isMoveDown = true;
    }

    public void Faster()
    {
        isFaster = true;
    }

    public void Lowwer()
    {
        isLowwer = true;
    }

    public void Shoot()
    {
        if (gunUsing)
        {
            gunUsing.Shoot(targetAim,gunUsing.hitCollider);
        }
    }

    public void Dead()
    {
        BehaviourDead();
        NotifiObserverDead();
    }



    protected void MoveNormal()
    {
        isIdle = true;

        if (isMoveUp)
        {
            if (isMoveLeft)
            {
                if (isFaster)
                {
                    BehaviourRunUpLeft();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkUpLeft();
                    isIdle = false;
                }
            }
            else if(isMoveRight)
            {
                if (isFaster)
                {
                    BehaviourRunUpRight();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkUpRight();
                    isIdle = false;
                }
            }
            else
            {
                if (isFaster)
                {
                    BehaviourRun();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalk();
                    isIdle = false;
                }
            }
        }
        else if (isMoveDown)
        {
            if (isMoveLeft)
            {
                if (isFaster)
                {
                    BehaviourRunBackLeft();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkBackLeft();
                    isIdle = false;
                }
            }
            else if (isMoveRight)
            {
                if (isFaster)
                {
                    BehaviourRunBackRight();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkBackRight();
                    isIdle = false;
                }
            }
            else
            {
                if (isFaster)
                {
                    BehaviourRunBackWard();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkBackWard();
                    isIdle = false;
                }
            }
        }
        else
        {
            if (isMoveLeft)
            {
                if (isFaster)
                {
                    BehaviourRunLeft();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkLeft();
                    isIdle = false;
                }
            }
            else if (isMoveRight)
            {
                if (isFaster)
                {
                    BehaviourRunRight();
                    isIdle = false;
                }
                else
                {
                    BehaviourWalkRight();
                    isIdle = false;
                }
            }
        }
        if (isIdle)
        {
            BehaviourIdle();
        }
        isMoveLeft = false;
        isMoveRight = false;
        isMoveUp = false;
        isMoveDown = false;
        isFaster = false;
        isLowwer = false;
    }

    protected abstract void BehaviourWalkRight();
    protected abstract void BehaviourWalkLeft();
    protected abstract void BehaviourWalkUpRight();
    protected abstract void BehaviourWalkUpLeft();
    protected abstract void BehaviourWalkBackRight();
    protected abstract void BehaviourWalkBackLeft();
    protected abstract void BehaviourRunRight();
    protected abstract void BehaviourRunLeft();
    protected abstract void BehaviourRunUpRight();
    protected abstract void BehaviourRunUpLeft();
    protected abstract void BehaviourRunBackRight();
    protected abstract void BehaviourRunBackLeft();
    protected abstract void BehaviourWalkBackWard();
    protected abstract void BehaviourRunBackWard();
    protected abstract void BehaviourWalk();
    protected abstract void BehaviourRun();
    protected abstract void BehaviourIdle();

    protected abstract void BehaviourDead();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


public abstract class Gun : NetworkBehaviour
{
    [SerializeField] Transform transformGrip,transformForend;
    public Transform transformGunHolder, transformAim, transformBarrel;
    [HideInInspector]
    public Collider hitCollider;

    public Vector3 positionGrip
    {
        get
        {
            return transformGrip.position;
        }
    }

    public Vector3 positionForend
    {
        get
        {
            return transformForend.position;
        }
    }

    public Quaternion rotationGrip
    {
        get
        {
            return transformGrip.rotation;
        }
    }

    public Quaternion rotationForend
    {
        get
        {
            return transformForend.rotation;
        }
    }

    public Vector3 positionBarrel
    {
        get
        {
            return transformBarrel.position;
        }
    }

    public abstract void Shoot(Vector3 target, Collider hitCollider);
}

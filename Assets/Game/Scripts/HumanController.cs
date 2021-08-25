using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public float speed = 1f;

    [SerializeField] HumanWalk walk;

    [SerializeField] HumanControl control,targetControl;
    [SerializeField] Animator animatorControl;

    private void Awake()
    {
        SetHumanBehaviors();
    }

    private void SetHumanBehaviors()
    {
        walk.Set(animatorControl, control, targetControl);
    }

    private void Update()
    {
        animatorControl.speed = speed;
        walk.OnFixedUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWalk : MoveController
{
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.position = transformToControl.position;
    }
}

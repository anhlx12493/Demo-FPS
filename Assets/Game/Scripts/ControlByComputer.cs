using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlByComputer : Controller
{
    Animator animator;

    float speed = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            character.MoveUp();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            character.Faster();
        }

        character.transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }
}

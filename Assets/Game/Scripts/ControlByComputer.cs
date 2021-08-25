using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlByComputer : Controller
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetFloat("Speed", 2f);
                character.isRunning = true;
            }
            else
            {
                animator.SetFloat("Speed", 1f);
                character.isRunning = false;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            character.isRunning = false;
        }

        character.transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }
}

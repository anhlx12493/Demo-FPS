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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed += 0.1f;
                if (speed > 2)
                    speed = 2;
                animator.SetFloat("Speed", speed);
                character.isRunning = true;
            }
            else
            {
                if (speed > 1)
                {
                    speed -= 0.1f;
                    if (speed < 1)
                        speed = 1;
                }
                else
                {
                    speed = 1;
                }
                animator.SetFloat("Speed", speed);
                character.isRunning = false;
            }
        }
        else
        {
            speed -= 0.1f;
            if (speed < 0)
                speed = 0;
            animator.SetFloat("Speed", speed);
            character.isRunning = false;
        }

        character.transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }
}

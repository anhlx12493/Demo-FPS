using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlByComputer : Controller
{
    Transform transformCopyRotate;

    private void Awake()
    {
        character = GetComponent<Character>();
        transformCopyRotate = new GameObject().transform;
        transformCopyRotate.rotation = character.transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            character.MoveUp();
        }

        if (Input.GetKey(KeyCode.S))
        {
            character.MoveDown();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            character.Faster();
        }

        if (Input.GetKey(KeyCode.A))
        {
            character.MoveLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            character.MoveRight();
        }

        transformCopyRotate.Rotate (-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        character.rotationLook = Quaternion.Euler(transformCopyRotate.rotation.eulerAngles.x, transformCopyRotate.rotation.eulerAngles.y, 0);
        transformCopyRotate.rotation = character.rotationLook;
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (Input.GetMouseButton(0))
        {
            character.Shoot();
        }
    }
}

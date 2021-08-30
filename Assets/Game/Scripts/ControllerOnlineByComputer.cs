using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System;

public class ControllerOnlineByComputer : NetworkBehaviour
{
    NetworkVariableVector3 positionCharacter = new NetworkVariableVector3(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableQuaternion rotationCharacter = new NetworkVariableQuaternion(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableQuaternion rotationCharacterLook = new NetworkVariableQuaternion(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isUp = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isDown = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isLeft = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isRight = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isShoot = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    NetworkVariableBool isFaster = new NetworkVariableBool(new NetworkVariableSettings
    {
        ReadPermission = NetworkVariablePermission.Everyone,
        WritePermission = NetworkVariablePermission.ServerOnly
    });

    bool _isUp, _isDown, _isLeft, _isRight, _isFaster, _isShoot;
    float _lastHealth;
    Transform transformCopyRotate;

    Character character;

    private void Start()
    {
        SetCharacter(GetComponent<Character>());
    }



    public void SetCharacter(Character character)
    {
        if(!transformCopyRotate)
        transformCopyRotate = new GameObject().transform;
        this.character = character;
        _lastHealth = character.health;
        if (IsOwner)
        {
            if (IsServer)
            {
                ServerControl();
            }
            else
            {
                ClientControl();
            }
            character.cameraFollow = FindObjectOfType<CameraFollow>();
            character.cameraFollow.transformToFolow = character.transform;
        }
    }

    private void Update()
    {
        if (!character)
            return;
        if (IsOwner)
        {
            if (IsServer)
            {
                ServerControl();
            }
            else
            {
                ClientControl();
            }
        }
        if (isUp.Value)
        {
            character.MoveUp();
        }
        if (isDown.Value)
        {
            character.MoveDown();
        }
        if (isLeft.Value)
        {
            character.MoveLeft();
        }
        if (isRight.Value)
        {
            character.MoveRight();
        }
        if (isFaster.Value)
        {
            character.Faster();
        }
        if (isShoot.Value)
        {
            character.Shoot();
        }
        if (IsServer)
        {
            if(_lastHealth != character.health)
            {
                _lastHealth = character.health;
                if(_lastHealth <= 0)
                {
                    character.Dead();
                    DeadClientRpc();
                }
            }

            positionCharacter.Value = character.transform.position;
            rotationCharacter.Value = character.transform.rotation;
        }
        else
        {
            if (!IsOwner)
            {
                character.rotationLook = Quaternion.Lerp(character.rotationLook, rotationCharacterLook.Value, 0.3f);
            }
            character.transform.position = Vector3.Lerp(character.transform.position, positionCharacter.Value, 0.3f);
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, rotationCharacter.Value, 0.3f);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (IsClient)
        {
        }
    }

    private void ServerControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!isUp.Value)
                isUp.Value = true;
        }
        else
        {
            if (isUp.Value)
                isUp.Value = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!isDown.Value)
                isDown.Value = true;
        }
        else
        {
            if (isDown.Value)
                isDown.Value = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!isLeft.Value)
                isLeft.Value = true;
        }
        else
        {
            if (isLeft.Value)
                isLeft.Value = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!isRight.Value)
                isRight.Value = true;
        }
        else
        {
            if (isRight.Value)
                isRight.Value = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isFaster.Value)
                isFaster.Value = true;
        }
        else
        {
            if (isFaster.Value)
                isFaster.Value = false;
        }
        if (Input.GetMouseButton(0))
        {
            if (!isShoot.Value)
                isShoot.Value = true;
        }
        else
        {
            if (isShoot.Value)
                isShoot.Value = false;
        }

        transformCopyRotate.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        character.rotationLook = Quaternion.Euler(transformCopyRotate.rotation.eulerAngles.x, transformCopyRotate.rotation.eulerAngles.y, 0);
        transformCopyRotate.rotation = character.rotationLook;
        rotationCharacterLook.Value = character.rotationLook;
    }

    private void ClientControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!_isUp)
            {
                _isUp = true;
                MoveUpServerRpc(_isUp);
            }
        }
        else
        {
            if (_isUp)
            {
                _isUp = false;
                MoveUpServerRpc(_isUp);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!_isDown)
            {
                _isDown = true;
                MoveDownServerRpc(_isDown);
            }
        }
        else
        {
            if (_isDown)
            {
                _isDown = false;
                MoveDownServerRpc(_isDown);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!_isLeft)
            {
                _isLeft = true;
                MoveLeftServerRpc(_isLeft);
            }
        }
        else
        {
            if (_isLeft)
            {
                _isLeft = false;
                MoveLeftServerRpc(_isLeft);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!_isRight)
            {
                _isRight = true;
                MoveRightServerRpc(_isRight);
            }
        }
        else
        {
            if (_isRight)
            {
                _isRight = false;
                MoveRightServerRpc(_isRight);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!_isFaster)
            {
                _isFaster = true;
                MoveFasterServerRpc(_isFaster);
            }
        }
        else
        {
            if (_isFaster)
            {
                _isFaster = false;
                MoveFasterServerRpc(_isFaster);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (!_isShoot)
            {
                _isShoot = true;
                ShootServerRpc(_isShoot);
            }
        }
        else
        {
            if (_isShoot)
            {
                _isShoot = false;
                ShootServerRpc(_isShoot);
            }
        }

        transformCopyRotate.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        character.rotationLook = Quaternion.Euler(transformCopyRotate.rotation.eulerAngles.x, transformCopyRotate.rotation.eulerAngles.y, 0);
        transformCopyRotate.rotation = character.rotationLook;
        LookServerRpc(character.rotationLook);
    }

    [ServerRpc]
    void MoveUpServerRpc(bool value)
    {
        isUp.Value = value;
    }

    [ServerRpc]
    void MoveDownServerRpc(bool value)
    {
        isDown.Value = value;
    }

    [ServerRpc]
    void MoveLeftServerRpc(bool value)
    {
        isLeft.Value = value;
    }

    [ServerRpc]
    void MoveRightServerRpc(bool value)
    {
        isRight.Value = value;
    }

    [ServerRpc]
    void MoveFasterServerRpc(bool value)
    {
        isFaster.Value = value;
    }

    [ServerRpc]
    void ShootServerRpc(bool value)
    {
        isShoot.Value = value;
    }

    [ServerRpc]
    void LookServerRpc(Quaternion value)
    {
        rotationCharacterLook.Value = value;
        if (character)
        {
            character.rotationLook = value;
        }
    }

    [ClientRpc]
    void DeadClientRpc()
    {
        if(character)
            character.Dead();
    }
}

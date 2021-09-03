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

    NetworkVariableVector3 positionAimCharacter = new NetworkVariableVector3(new NetworkVariableSettings
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
            Camera.main.GetComponent<CameraFollow>().transformToFolow = character.transformCamera;
        }
        if (!IsServer)
        {
            GetComponent<Animator>().applyRootMotion = false;
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
            character.MoveAim(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
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
            if (IsOwner)
            {
                if (IsServer)
                {
                    character.Shoot();
                }
                else
                {
                    ShootServerRpc(character.positionStartAim, character.forwardAim);
                }
            }
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
        Synchronized();
    }

    private void Synchronized()
    {
        if (!IsServer)
        {
            character.transform.position = Vector3.Lerp(character.transform.position, positionCharacter.Value, 0.2f);
        }
        if (!IsOwner)
        {
            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, rotationCharacter.Value, 0.2f);
            character.MoveAim(positionAimCharacter.Value);
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
        rotationCharacter.Value = character.transform.rotation;
        positionAimCharacter.Value = character.targetAim;
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
        UpdateRotationCharacterServerRpc(character.transform.rotation,character.targetAim);
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
    void ShootServerRpc(Vector3 positionStart,Vector3 forward)
    {
        character.Shoot(positionStart, forward);
    }

    [ServerRpc]
    void UpdateRotationCharacterServerRpc(Quaternion rotation,Vector3 positionAim)
    {
        rotationCharacter.Value = rotation;
        positionAimCharacter.Value = positionAim;
    }

    [ClientRpc]
    void DeadClientRpc()
    {
        character.Dead();
    }
}

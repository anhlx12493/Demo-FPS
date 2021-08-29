using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player : NetworkBehaviour, ObserverCharacterDead
{
    Character character;

    void Start()
    {
        if (IsOwner)
        {
            CreateCharacterServerRpc();
        }
    }

    public void OnCharacterDead(Character character)
    {
        if (IsOwner)
        {
            CreateCharacterServerRpc();
        }
    }

    [ServerRpc]
    void CreateCharacterServerRpc()
    {
        character = PrefabManager.Instance(PrefabManager.prefabCharacterMan).GetComponent<Character>();
        character.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
        character.transform.position = GamePlayLevelManager.singleton.GetRandomSpawPosition();
        character.ResigterObserverDead(this);
    }
}

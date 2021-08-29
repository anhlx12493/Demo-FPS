using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayLevelManager : SingletonMonoBehaviour <GamePlayLevelManager>
{

    [SerializeField] Transform[] transformsSpawnCharacter;

    private void Awake()
    {
        SetSingleTon(this);
    }

    public Vector3 GetRandomSpawPosition()
    {
        return transformsSpawnCharacter[Random.Range(0, transformsSpawnCharacter.Length)].position;
    }
}

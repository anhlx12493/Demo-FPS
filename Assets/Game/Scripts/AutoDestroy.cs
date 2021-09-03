using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class AutoDestroy : NetworkBehaviour
{
    public float time;


    
    private void OnServerInitialized()
    {
    }
    void Start()
    {
        if (!IsServer)
        {
            Destroy(this);
        }
        StartCoroutine(CoroutineWaitForSecond(time));
    }

    IEnumerator CoroutineWaitForSecond(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}

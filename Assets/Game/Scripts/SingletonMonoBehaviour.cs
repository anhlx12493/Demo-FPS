using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour <T> : MonoBehaviour
{
    private static T _singleton;

    public static T singleton
    {
        get
        {
            return _singleton;
        }
    }

    public void SetSingleTon(T mono)
    {
        if (_singleton != null)
            throw new System.Exception("Singleton has available");
        _singleton = mono;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager
{
    internal static string prefabBullet = "Bullet";
    internal static string prefabBlood = "Blood";
    internal static string prefabCharacterMan = "Man";

    public static GameObject GetPrefab(string prefabName)
    {
        return Resources.Load<GameObject>("Prefabs/" + prefabName);
    }

    public static GameObject Instance(string prefabName, Transform parent = null)
    {
        return GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + prefabName), parent);
    }
}

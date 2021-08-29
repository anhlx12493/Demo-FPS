using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_AR_15 : Gun
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update

    GameObject prefabBullet,prefabBlood;
    
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        prefabBullet = PrefabManager.GetPrefab(PrefabManager.prefabBullet);
        prefabBlood = PrefabManager.GetPrefab(PrefabManager.prefabBlood);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.enabled = false;
    }
    public override void Shoot(Vector3 target, Collider hitCollider)
    {
        if (hitCollider)
        {
            if (hitCollider.tag == "Character")
            {
                Instantiate(prefabBlood, target, Quaternion.Euler(Vector3.zero));
                Character character = hitCollider.GetComponentInParent<Character>();
                if (character)
                {
                    character.Dame(1f);
                }
            }
            else
            {
                Instantiate(prefabBullet, target, Quaternion.Euler(Vector3.zero));
            }
        }
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, positionBarrel);
        lineRenderer.SetPosition(1, target);
    }
}

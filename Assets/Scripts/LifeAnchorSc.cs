using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAnchorSc : MonoBehaviour
{
    public GameObject LifePrefab;
    GameObject prefab;
    int StartNumber;
    GameObject pict;

    
   
    void Start()
    {
        StartNumber = SpaceSheepSc.LifeCount;
        if (LifePrefab != null)
        {
            for(int i=0; i < SpaceSheepSc.LifeCount; i++)
            {
                prefab = Instantiate(LifePrefab);
                prefab.transform.SetParent(gameObject.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(SpaceSheepSc.LifeCount< StartNumber)
        {
            pict = gameObject.transform.GetChild(SpaceSheepSc.LifeCount).gameObject;
            Destroy(pict);
            StartNumber = SpaceSheepSc.LifeCount;
        }
       else if (SpaceSheepSc.LifeCount > StartNumber)
        {
            prefab = Instantiate(LifePrefab);
            prefab.transform.SetParent(gameObject.transform);
            StartNumber = SpaceSheepSc.LifeCount;
        }
    }
}

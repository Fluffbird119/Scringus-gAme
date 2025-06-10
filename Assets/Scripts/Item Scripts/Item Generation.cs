using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
    public GameObject basicSwordPrefab;

    public BasicSword generateBasicSword(Vector3 pos, Quaternion rotation)
    {
        BasicSword newBasicSword = new BasicSword(basicSwordPrefab);
        Instantiate(newBasicSword, 
            pos, 
            Quaternion.Euler(
                rotation.x, 
                rotation.y, 
                rotation.z));
        return newBasicSword;
    }
}

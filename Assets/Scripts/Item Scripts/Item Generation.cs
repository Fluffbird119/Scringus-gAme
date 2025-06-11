using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration : ScriptableObject
{
    public GameObject basicSwordPrefab;


    /*public BasicSword generateBasicSword(GameObject player)
    {
        BasicSword newBasicSword = new BasicSword(basicSwordPrefab, player);
        Instantiate(newBasicSword, player.transform.position, player.transform.rotation);
        return newBasicSword;
    }*/
    public static GameObject spawnInItem<T>(string newObjectName) where T : MonoBehaviour
    {
        GameObject newItem = new GameObject(newObjectName);
        newItem.AddComponent<T>();
        newItem.transform.localScale = new Vector3(100, 100, 100);
        return newItem;
    }
    void Awake()
    {
        spawnInItem<Halberd>("protoHalberd");
    }

}

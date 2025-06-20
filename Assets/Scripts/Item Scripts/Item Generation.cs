using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemGeneration : ScriptableObject
{
    private static GameObject[] weaponPrefabs = Resources.LoadAll<GameObject>("Prefabs/Weapon Prefabs");

    public static void spawnItem(GameObject itemPrefab, Vector3 pos)
    {
        GameObject item = Instantiate(itemPrefab, pos, Quaternion.identity);
        WorldItem worldItem = item.GetComponent<WorldItem>();
    }

    public static void spawnItem(GameObject itemPrefab, Vector3 pos, ItemData itemData) //the version that uses itemData (by inventory)
    {
        GameObject item = Instantiate(itemPrefab, pos, Quaternion.identity);
        BoxCollider2D collider = item.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        WorldItem worldItem = item.AddComponent<WorldItem>();
        worldItem.setItem(itemData);
        Debug.Log(worldItem);
    }

    public static void spawnWorldItem(GameObject itemPrefab, Vector3 pos) //version for hotbar and heldItem
    {
        if(itemPrefab != null) // != null can be used because itemPrefab is a GameObject
        {
            GameObject itemPrefabCopy = Instantiate(itemPrefab, pos, Quaternion.identity);
            itemPrefabCopy.name = nameFunction(itemPrefab, " (HBS)", " (WIV)");

            if (itemPrefabCopy.TryGetComponent<Collider2D>(out Collider2D currentCollider2D))
            {
                currentCollider2D.enabled = true;
                currentCollider2D.isTrigger = true;
            }
            else
            {
                BoxCollider2D newCollider = itemPrefabCopy.AddComponent<BoxCollider2D>();
                newCollider.isTrigger = true;
            }

            if (itemPrefabCopy.TryGetComponent<WorldItem>(out WorldItem currentWorldItem))
            {
                currentWorldItem.enabled = true;
            }
            else
            {
                WorldItem newWorldItem = itemPrefabCopy.AddComponent<WorldItem>();
            }
        }
    }



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
        //newItem.transform.localScale = new Vector3(100, 100, 100);
        return newItem;
    }

    public static GameObject getRandomWeapon()
    {
        int randIndex = Random.Range(0, weaponPrefabs.Length - 1);

        return weaponPrefabs[randIndex];
    }


    public static string nameFunction(GameObject itemPrefab, string endToRemove, string endToAdd)
    {
        string oldName = itemPrefab.name;
        string newName = "";
        if (oldName[^endToRemove.Length..] == endToRemove)
        {
            newName = oldName[..^endToRemove.Length] + endToAdd; //'..' means 'range of index values' and ^ means 'Length - what comes after'
        }
        else
        {
            newName = oldName + endToAdd;
        }
        return newName;
    }

}
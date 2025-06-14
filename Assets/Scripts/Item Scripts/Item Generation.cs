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

    public static void spawnItem(GameObject itemPrefab, Vector3 pos, ItemData itemData, Item item)
    {
        GameObject spawnedItem = Instantiate(itemPrefab, pos, Quaternion.identity);
        BoxCollider2D collider = spawnedItem.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        WorldItem worldItem = spawnedItem.AddComponent<WorldItem>();
        worldItem.setItemData(itemData);
        worldItem.setItem(item);
    }

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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
    public GameObject basicSwordPrefab;

    public static void spawnItem(string filePath, Vector3 pos)
    {
        GameObject itemPrefab = Resources.Load<GameObject>(filePath);

        GameObject item = Instantiate(itemPrefab, pos, Quaternion.identity);

        WorldItem worldItem = item.GetComponent<WorldItem>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public ItemData item;
    public Transform handAnchor;
    private GameObject heldItem;

    public void pickUp(ItemData item)
    {
        this.item = item;

        if (item.visualPrefab != null)
        {
            heldItem = Instantiate(item.visualPrefab, handAnchor.position, handAnchor.rotation, handAnchor);
            heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
    }
}

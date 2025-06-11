using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public Transform handAnchor;
    public ItemData item;
    private GameObject heldItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null) //checks if hand is empty and if player is pressing q
        {
            drop();
        }
    }
    public void pickUp(ItemData item)
    {
        this.item = item;

        if (item.visualPrefab != null)
        {
            heldItem = Instantiate(item.visualPrefab, handAnchor.position, handAnchor.rotation, handAnchor);
            heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
    }

    public void drop()
    {
        ItemGeneration.spawnItem(heldItem, handAnchor.position, this.item);
        Destroy(heldItem);
        heldItem = null;
        item = null;
    }

    public GameObject getHeldItem() { return heldItem; }
}

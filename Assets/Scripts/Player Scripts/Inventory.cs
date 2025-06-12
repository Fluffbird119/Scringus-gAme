using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public Transform handAnchor;
    public ItemData itemData;
    private GameObject heldItem;
    private Item item;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null) //checks if hand is empty and if player is pressing q
        {
            drop();
        }

        if (heldItem != null && Input.GetMouseButtonDown(0))
        {
            item.use(heldItem);
        }
    }
    public void pickUp(ItemData itemData, Item item)
    {
        this.itemData = itemData;
        this.item = item;
        Debug.Log(item);

        if (itemData.visualPrefab != null)
        {
            heldItem = Instantiate(itemData.visualPrefab, handAnchor.position, handAnchor.rotation, handAnchor);
            heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            Collider2D collider = heldItem.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    public void drop()
    {
        ItemGeneration.spawnItem(heldItem, handAnchor.position, this.itemData);
        Destroy(heldItem);
        heldItem = null;
        item = null;
    }

    public GameObject getHeldItem() { return heldItem; }
}

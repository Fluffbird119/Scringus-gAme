using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldItem : MonoBehaviour
{
    public ItemData itemData;
    public Item item;
    private bool playerIsNear = false;
    private Inventory player;

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            player.pickUp(itemData, item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.GetComponent<Inventory>();
        playerIsNear = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        player = null;
        playerIsNear = false;
    }

    public void setItemData(ItemData itemData) { this.itemData = itemData; }
    public void setItem(Item item) { this.item = item; }
}

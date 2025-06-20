using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldItem : MonoBehaviour
{
    public ItemData itemData;           // left here for inventory verison
    private bool playerIsNear = false;  // left here for inventory version
    private Inventory player;           // left here for inventory version

    //[Header("Events")]
    //[SerializeField] private GameEvent onPickUpItemAttempt;

    private void Update() //right now it is a bit weird that all items are constantly calling this
    {
        /* (overwritten inventory verion)
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            player.pickUp(item);
            Destroy(gameObject);
        }*/

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

    public void setItem(ItemData itemData) { this.itemData = itemData; } //left here as inventory version

    
    public GameObject getHotbarSafeCopy() //returns a copy that has a disabled worldItem and collider (for hotbar and held item usage)
    {
        GameObject objectForHotbar = Instantiate(this.gameObject);
        objectForHotbar.name = ItemGeneration.nameFunction(this.gameObject, " (WIV)", " (HBS)"); //HBS means HotbarSafe

        objectForHotbar.TryGetComponent<Collider2D>(out Collider2D collider2D);
        {
            collider2D.enabled = false;

        }
        objectForHotbar.TryGetComponent<WorldItem>(out WorldItem newWorldItem);
        {
            newWorldItem.enabled = false;
        }
        objectForHotbar.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr);
        {
            sr.enabled = false;
        }

        return objectForHotbar;
    }
}

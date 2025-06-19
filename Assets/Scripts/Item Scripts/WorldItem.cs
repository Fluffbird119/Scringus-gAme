using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldItem : MonoBehaviour
{
    private GameObject itemPrefab; //only stored so that when picked up, can throw the itemprefab without the collider (ALSO forcefully has spriterenderer disabled
    public ItemData itemData; // left here for inventory verison
    private bool playerIsNear = false;
    private Inventory player; // left here for inventory version

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

    public void setItemPrefab(GameObject itemPrefab)
    {
        GameObject bucketPrefab = Instantiate(itemPrefab);
        bucketPrefab.GetComponent<SpriteRenderer>().enabled = false;
        this.itemPrefab = bucketPrefab;
    }
    public GameObject getItemPrefab()
    {
        return this.itemPrefab;
    }
}

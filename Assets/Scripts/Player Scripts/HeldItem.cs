using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class HeldItem : MonoBehaviour
{
    // so this effectively is a replacement for Inventory
    /*
     * Effectively, hotbar and the general item class requires the updating of 4 classes: 
     *  Inventory (which this is replacing)
     *  WorldItem (which won't be replaced but instead updated with other methods to incorporate the new changes)
     *  ItemData (which is already redundant to item)
     *  ItemGenerator (which will just have added methods to incorporate the new changes)
     */


    //NOTE: it will later be the case that some 'player class' [for multiplayer purposes] will hold both Hotbar_UI and this. The prefab will actuallly override then

    public Transform handAnchor;
    public Item heldItem; //heldItem needs to actually be an Item (i.e. have components Item, Sprite renderer, etc...)
    //no longer needs hotbar because of hotbar's gameEvent broadcasting

    /*
     
                NOTES TO SELF (TODO LIST<>)
                    Hotbar_UI will handle the dropHotkey update detection
                    Hotbar has its gameEvents, but they need to actually be made in the unity scene and all (listeners too, prolly only for 'drop' so far)
                    Obviously finish integating all of these
                        heldItem will prolly only know about the heldItem and nothing else. I dunno
                    
            


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //checks if hand is empty and if player is pressing q
        {
            //hotKeyDrop();
        }
    }

    public void drop()
    {
        
        
        ItemGeneration.spawnWorldItem(heldItem.gameObject, handAnchor.position);
        Destroy(heldItem);
        item = null;
    }
    */

    /*
     * public class Inventory : MonoBehaviour
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
     * */
}

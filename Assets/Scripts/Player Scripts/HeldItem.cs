using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
//using Object.Equals as UnityEngine.Object.Equals;
using static UnityEditor.Progress;

public class HeldItem : MonoBehaviour
{
    // so this effectively is a replacement for Inventory
    /*
     *  Effectively, hotbar and the general item class requires the updating of 4 classes: 
     *      Inventory (which this is replacing)
     *      WorldItem (which won't be replaced but instead updated with other methods to incorporate the new changes)
     *      ItemData (which is already redundant to item)
     *      ItemGenerator (which will just have added methods to incorporate the new changes)
     *  
     *  Note that HeldItem handles 'using' the weapons and handles visually representing the weapons 
     * 
     */


    //NOTE: it will later be the case that some 'player class' [for multiplayer purposes] will hold both Hotbar_UI and this. The prefab will actuallly override then

    //private readonly bool IS_LEFT_PRIORITY_HAND = true;

    //the Anchors to set transforms to:
    public Transform lHandAnchor;
    public Transform rHandAnchor; //for now, this will be ignored

    //the fields that will hold the purely visual item copy (that will be routinely deleted and instantiated)
    private GameObject lHandVisual; 
    private GameObject rHandVisual;
    
    //the fields that hold the references to the hotbar weapons (whose spriteRenderer should be off)
    private Item lHandItem; //to be called for action
    private Item rHandItem; //to be called for alt action

    public EquipStyle Style  { get; private set; }

    public enum EquipStyle //this will be nice to know especially for abilities and whatnot
    {
        UNARMED,        // holding literally nothing (both hands must be on the same empty slot)
        DUAL_WIELD,  // holding 2 separate 1-handed items
        TWO_HANDED,     // holding a singular 2-handed item
        DUELIST         // holding a singular 1-handed item (technically according to the hotbar both hands are holding the same 1-handed weapon in this case)
    }

    public HeldItem()
    {
        this.Style = EquipStyle.UNARMED;
    }


    public void action()
    {
        throw new NotImplementedException(); //should call the Item's action depending on the current style
    }
    public void altAction()
    {
        throw new NotImplementedException(); //should call the Item's altAction depending on current Style
    }



    public void handleHeldItemOnlyChange(Component sender, object data) //so far for sender hotbar_UI and data (lHandItem, rHandItem)
    {
        if(sender is Hotbar_UI && data is Tuple<Item,Item>)
        {
            //for multiplayer, may need to check exactly which hotbar is sending
            Tuple<Item,Item> castedData = (Tuple<Item,Item>)data;
            if ( !(UnityEngine.Object.Equals(lHandItem, castedData.Item1) && UnityEngine.Object.Equals(rHandItem, castedData.Item2) ) ) //should work even w/nulls
            {
                //this runs only when the 'change' actually changes the handled items (this GameEvent may be called even if there isn't a change to the heldItems)

                removeVisual();//must run before style is overwritten
                
                lHandItem = castedData.Item1;
                rHandItem = castedData.Item2;
                if (UnityEngine.Object.Equals(castedData.Item1, null)) //if the first is null they are both null
                {
                    this.Style = EquipStyle.UNARMED; //emptyEquip
                }
                else if(castedData.Item1.isOneHanded) //empty must be evaluated first or else nullPointerException will occur
                {
                    //asks if both hands sent as data are the same Item or not (if so, Duelist, if not, dual wield)
                    this.Style = UnityEngine.Object.Equals(castedData.Item1, castedData.Item2) ? EquipStyle.DUELIST : EquipStyle.DUAL_WIELD;
                }
                else // the only remaining case is that they are holding a two handed weapon
                {
                    this.Style = EquipStyle.TWO_HANDED;
                }

                createVisual();//must run after Style is overwritten
            }
        }
    }
        
    private void removeVisual()
    {
        removeVisual(this.Style);
    }

    private void createVisual()
    {
        createVisual(this.Style);
    }

    private void removeVisual(EquipStyle eqStyle)
    {
        switch (eqStyle)
        {
            case EquipStyle.DUAL_WIELD:
                Destroy(lHandVisual);
                Destroy(rHandVisual);
                break;
            case EquipStyle.TWO_HANDED:
                Destroy(lHandVisual);
                break;
            case EquipStyle.DUELIST:
                Destroy(lHandVisual);
                break;
            case EquipStyle.UNARMED:
                break;
            default:
                throw new ArgumentOutOfRangeException( "removeVisual() broke, with param: " + nameof(eqStyle));
        }
    }


    private void createVisual(EquipStyle eqStyle)
    {
        switch (eqStyle)
        {
            case EquipStyle.DUAL_WIELD:
                constructHandVisual(true);
                constructHandVisual(false);
                break;
            case EquipStyle.TWO_HANDED:
                constructHandVisual(true);
                break;
            case EquipStyle.DUELIST:
                constructHandVisual(true);
                break;
            case EquipStyle.UNARMED:
                break;
            default:
                throw new ArgumentOutOfRangeException("createVisual() broke, with param: " + nameof(eqStyle));
        }
    }

    private void constructHandVisual(bool isLeftHand)
    {
        if(isLeftHand)
        {
            lHandVisual = Instantiate(lHandItem.gameObject, lHandAnchor.position, lHandAnchor.rotation, lHandAnchor);
            lHandVisual.GetComponent<SpriteRenderer>().enabled = true; //generally in the hotbar Items have enabled = false;
        }
        else
        {
            rHandVisual = Instantiate(rHandItem.gameObject, rHandAnchor.position, rHandAnchor.rotation, rHandAnchor);
            rHandVisual.GetComponent<SpriteRenderer>().enabled = true; //generally in the hotbar Items have enabled = false;
        }
    }
    
    
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

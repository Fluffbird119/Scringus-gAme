using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
public enum ItemType
{
    Consumable, Weapon, misc
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    //[Cu]: I don't want to overwrite this and annihilate the other scene, so my comments explain the alternate version of this stuff for when time comes to
    //          integrate all of this stuff
    public ItemType Type; //Item.getItemType() also reports this albeit technically as Item.ItemType
    private bool isBeingHeld = false; //being kept to not break the other scene, but hotbar should know about this
    public GameObject visualPrefab; //Item.gameObject.GetComponent<SpriteRenderer>().sprite is the sprite if desired
    public string itemName; //Item.gameObject.name
    private Item item;

    [TextArea]
    public string text;

    public void Use()
    {
        Debug.Log("Using " + itemName);
    }

    public bool getIsBeingHeld() { return isBeingHeld; }
    public void setIsBeingHeld(bool isBeingHeld) { this.isBeingHeld = isBeingHeld; }
}

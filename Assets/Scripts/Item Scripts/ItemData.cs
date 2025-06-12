using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;
public enum ItemType
{
    Consumable, Weapon, misc
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType Type;
    private bool isBeingHeld = false;
    public GameObject visualPrefab;
    public string itemName;

    [TextArea]
    public string text;

    public void Use()
    {
        Debug.Log("Using " + itemName);
    }

    public bool getIsBeingHeld() { return isBeingHeld; }
    public void setIsBeingHeld(bool isBeingHeld) { this.isBeingHeld = isBeingHeld; }
}

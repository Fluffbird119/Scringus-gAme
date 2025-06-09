using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Object // maybe should extend entity, ALSO Consumable and Weapon whould inherit from this
{
    //details to be passed into constructors
    private GameObject prefab;
    private GameObject playerObject;
    public Item.ItemType itemType { get; } //non-weapon items are probably going to be called consumable or utility
    private string itemName; //as in name of item if looked at while on ground or in menu


    private Vector2 pos = new Vector2(); //as in position in game, particularly if unequipped and on the ground

    private int hotbarIndex = -1;

    //do prefabs innately have sprites attached? Because if so, displaying an item in the hotbar and on the ground can be virtually the same
    //(except w/regard to location on character screen)

    public Item(GameObject prefab, GameObject playerObject, Item.ItemType itemType, string itemName)
    {
        this.prefab = prefab;
        this.playerObject = playerObject;
        this.itemType = itemType;
        this.itemName = itemName;
    }

    public Item(GameObject prefab, GameObject playerObject)
    {
        this.prefab = prefab;
        this.playerObject = playerObject;
    }
    public void displayItemOnGround()
    {
        //like how the item shows up on the ground (when hovered, it should eventually have a description, but for minimal product that is not needed
        //the gameObject may need to have a sprite atttatched to it to be rendered for this class to handle the display.
    }

    public void displayItemInHotbar() //honestly the hotbar will prolly handle this by taking the prefab/sprite
    {
        
    }

    public void dropItem()
    {
        //Needs to be implemented (will prolly be called by something in player)
    }

    public enum ItemType
    {
        UTILITY, //also called consumable by this code,

        MELEE_WPN,      //
        PROJECTILE_WPN, // all these are wpn subtypes
        SHIELD          // 
    }

    public GameObject getPrefab() { return prefab; }
}

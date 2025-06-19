using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour // maybe should extend entity, ALSO Consumable and Weapon whould inherit from this
{
    //details to be passed into constructors
    //private Transform transform; not needed as it is a monoBehaviour
    //private SpriteRenderer spriteRenderer; ^^^
    public Item.ItemType itemType { get; } //non-weapon items are probably going to be called consumable or utility
    private string pathToSprite; //as in name of item if looked at while on ground or in menu
    public bool isOneHanded { get; }

    
    //public bool isBeingHeld { get; private set; }

    //private player should know who its player is

    //do prefabs innately have sprites attached? Because if so, displaying an item in the hotbar and on the ground can be virtually the same
    //(except w/regard to location on character screen)

    public Item(Item.ItemType itemType, bool isOneHanded, string pathToSprite)
    {
        //isOneHanded is always true for non weapons
        this.pathToSprite = pathToSprite;
        this.isOneHanded = isOneHanded;
        this.itemType = itemType;
    }

    void Awake()
    {
        //kind of accursed, I know
        if (!(this.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer irrelevant)))
        {
            Sprite targetSprite = Resources.Load<Sprite>(this.pathToSprite);
            this.gameObject.AddComponent<SpriteRenderer>();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = targetSprite;
        }
    }


    public void dropItem(GameObject playerGameObject) //whomever is the player dropping
    {
        this.transform.parent = null; //detatches positioning from parent hotbar
        this.transform.SetPositionAndRotation(playerGameObject.transform.position, Quaternion.identity);
    }


    public enum ItemType
    {
        UTILITY, //also called consumable by this code,

        MELEE_WPN,      //
        PROJECTILE_WPN, // all these are wpn subtypes
        SHIELD          // 
    }
}

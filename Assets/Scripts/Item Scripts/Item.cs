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


    private Vector2 pos = new Vector2(); //as in position in game, particularly if unequipped and on the ground
    private int hotbarIndex = -1; //position in hotbar (-1) means it it isn't in the hotbar
    //private player should know who its player is

    //do prefabs innately have sprites attached? Because if so, displaying an item in the hotbar and on the ground can be virtually the same
    //(except w/regard to location on character screen)

    public Item(Item.ItemType itemType, string pathToSprite)
    {
        this.itemType = itemType;
        this.pathToSprite = pathToSprite;
    }

    private void Awake()
    {
        //kind of accursed, I know
        if (!(this.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer irrelevant)))
        {
            Sprite targetSprite = Resources.Load<Sprite>(pathToSprite);
            this.gameObject.AddComponent<SpriteRenderer>();

            SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();

            sr.sprite = targetSprite;
        }
    }



    public void displayItemOnGround(Vector2 itemPos)
    {
        this.transform.SetPositionAndRotation(itemPos, Quaternion.identity);
        //like how the item shows up on the ground (when hovered, it should eventually have a description, but, for minimal product,that is not needed)
        //the gameObject may need to have a sprite atttatched to it to be rendered for this class to handle the display.
    }

    public void displayItemInHotbar(int hotbarIndex, GameObject[] hotbarGameObject) //honestly the hotbar will prolly handle this by taking the prefab/sprite
    {
        this.transform.SetParent(hotbarGameObject[hotbarIndex].transform);
        this.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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

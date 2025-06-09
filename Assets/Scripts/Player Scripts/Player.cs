using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Item> heldItems = new List<Item>(); //all items stored in the hotbar
    private Item equippedItem; //item held in hand
    public GameObject player;

    private void Start()
    {
        ItemGeneration itemGenerator = new ItemGeneration();
        itemGenerator.generateBasicSword(player);
    }
    public void setEquippedItem(Item equippedItem)
    {
        this.equippedItem = equippedItem;
    }

    public void equipItem(Item newItem)
    {
        setEquippedItem(newItem);

        equippedItem.getPrefab().transform.SetParent(transform);
    }


}

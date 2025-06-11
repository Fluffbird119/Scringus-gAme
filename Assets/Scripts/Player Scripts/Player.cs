using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Item> heldItems = new List<Item>(); //all items stored in the hotbar
    private Item equippedItem; //item held in hand

    private void Start()
    {
    }
    public void setEquippedItem(Item equippedItem)
    {
        this.equippedItem = equippedItem;
    }

    public void equipItem()
    {
    }


}

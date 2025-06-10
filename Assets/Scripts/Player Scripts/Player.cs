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
        ItemGeneration itemGeneration = new ItemGeneration();
        Debug.Log(itemGeneration);
        BasicSword item = itemGeneration.generateBasicSword(
            transform.position, 
            new Quaternion(transform.rotation.x, 
            transform.rotation.y, transform.rotation.z, transform.rotation.w));
    }
    public void setEquippedItem(Item equippedItem)
    {
        this.equippedItem = equippedItem;
    }

    public void equipItem()
    {
    }


}

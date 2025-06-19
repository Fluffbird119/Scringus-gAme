using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    //private List<Item> heldItems = new List<Item>(); //all items stored in the hotbar
    //private Item equippedItem; //item held in hand
    private List<WorldItem> nearbyWorldItems = new List<WorldItem>();

    void Start()
    {
    }
    /*public void setEquippedItem(Item equippedItem)
    {
        this.equippedItem = equippedItem;
    }*/

    public WorldItem initiateEquipItem() 
    {
        if (nearbyWorldItems.Count > 0)
        {
            WorldItem worldItemToEquip = nearbyWorldItems[nearbyWorldItems.Count - 1]; //finds last contacted weapon and attempts to equip it
            
            //nearbyWorldItems.RemoveAt(nearbyWorldItems.Count - 1);
            return worldItemToEquip;
        }
        else 
        {  
            return null; 
        }
    }

    public void concludeEquipItem(WorldItem worldItem)
    {
        nearbyWorldItems.Remove(worldItem);
        Destroy(worldItem.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<WorldItem>(out WorldItem worldItem))
        {
            nearbyWorldItems.Add(worldItem);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<WorldItem>(out WorldItem worldItem))
        {
            nearbyWorldItems.Remove(worldItem);
        }
    }


}

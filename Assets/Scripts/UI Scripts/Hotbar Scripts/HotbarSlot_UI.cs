using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HotbarSlot_UI : MonoBehaviour
{
    [SerializeField] public Image outerSlot;
    [SerializeField] public Image itemIcon; //the UI panel looking thing

    //private Sprite itemSprite;
    private Item item;

    private Color defaultOuterSlotColour = Color.grey;

    private int indexInHotbar;

    private bool isEnabled = false;

    public void setItem(Item item)
    {
        //this.itemSprite = item.GetComponent<SpriteRenderer>().sprite;
        this.item = item;
        this.itemIcon.sprite = item.GetComponent<SpriteRenderer>().sprite;
        this.itemIcon.color = new Color(1, 1, 1, 1);
    }

    public void emptySlot()
    {
        this.item = null;
        this.itemIcon.sprite = null;
        this.itemIcon.color = new Color(1, 1, 1, 0); //transparent image
    }

    public void setEquipColors(bool isLeftHand, bool isRightHand) //note that two handed weapons are both hands, and thus call (true, true)
    {
        if(isRightHand && isLeftHand) //i.e. two handed weapon
        {
            outerSlot.color = new Color(1f,0.78f,1f,1f); //purple : L + R
        }
        else if(isRightHand)
        {
            outerSlot.color = new Color(1f, 0.78f, 0.78f, 1f); //red : R
        }
        else if(isLeftHand)
        {
            outerSlot.color = new Color(0.78f, 0.78f, 1f, 1f); //blue : L
        }
        else
        {
            outerSlot.color = defaultOuterSlotColour; //default colour for when it isn't equipped whatsoever
        }
    }


    public void setEnabledState(bool enabledState)
    {
        if(enabledState)
        {
            this.enable();
        }
        else
        {
            this.disable();
        }
    }

    private void enable() 
    {
        if(!isEnabled)
        {
            emptySlot();
            isEnabled = true;
            outerSlot.color = defaultOuterSlotColour;
        }
    }

    private void disable()
    {
        if(isEnabled)
        {
            emptySlot();
            isEnabled = false;
            outerSlot.color = new Color(1, 1, 1, 0);//makes everything transparent
        }
    }

    public Item getItem()
    {
        return this.item;
    }

    public void setIndexForHotbar(int indexInHotbar)
    {
        this.indexInHotbar = indexInHotbar;
    }


    void Awake()
    {
        emptySlot();
        //defaultOuterSlotColour = outerSlot.color;
    }
}

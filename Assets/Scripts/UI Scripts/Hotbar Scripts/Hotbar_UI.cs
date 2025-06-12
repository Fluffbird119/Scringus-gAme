using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar_UI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private GameObject HotbarPanel;

    [SerializeField] private List<GameObject> slots = new List<GameObject>(); //each 'slot' NEEDS the HotbarSlot_UI component

    private int numAvailableSlots = 1;

    private static readonly int totalPossibleSlots = 10;

    private static readonly int[] enableOrder = { 4, 5, 3, 6, 2, 7, 1, 8, 0, 9 };

    private int indexOfL = 4; //equipped LHand
    private int indexOfR = 4; //Equipped RHand


    public void updateSlotNumber() //should also be called when player str changes
    {
        int temp = 1 + player.GetComponent<PlayerStats>().getStr() / 10;
        //this calculation is a prototype
        if(temp != numAvailableSlots)
        {
            for(int i = 0; i < totalPossibleSlots; i++)
            {
                slots[enableOrder[i]].SetActive(i < numAvailableSlots);
            }
        }
    }

    public void changeSlotEquipColors(int indexToChange, bool isLeftHand, bool isRightHand)
    {
        slots[indexToChange].GetComponent<HotbarSlot_UI>().setEquipColors(isLeftHand, isRightHand);
    }
    
    public void equipToSlot(int indexToEquip) //Only 3 possible prev hand positions: L,R on 2handwpn/empty, L,R on same 1handwpn, L,R on diff 1handwpn
    {
        if(indexToEquip == indexOfL || indexToEquip == indexOfR)
        {
            //simply do nothing
        }
        else if (Object.Equals(slots[indexToEquip].GetComponent<HotbarSlot_UI>().getItem(), null) || 
            (!slots[indexToEquip].GetComponent<HotbarSlot_UI>().getItem().isOneHanded)) //asks, is the item 2-handed or simply null?
        {
            changeSlotEquipColors(indexOfL, false, false);
            changeSlotEquipColors(indexOfR, false, false);
            changeSlotEquipColors(indexToEquip, true, true);
            indexOfL = indexToEquip;
            indexOfR = indexToEquip;
        }
        else if(Object.Equals(slots[indexOfL].GetComponent<HotbarSlot_UI>().getItem(), null) ||
                (!slots[indexOfL].GetComponent<HotbarSlot_UI>().getItem().isOneHanded)) //asks, is the previous L item NOT on a onehanded item?
        {
            if(indexOfL == indexOfR) //if true, means previous L,R on same 1-handed wpn
            {
                changeSlotEquipColors(indexOfL, true, false); //bc it was being held by 2 hands
                changeSlotEquipColors(indexToEquip, false, true); //so right hand is being used to equip
            }
            else
            {
                changeSlotEquipColors(indexOfR, false, false); //bc it was being held by only right
                changeSlotEquipColors(indexToEquip, false, true); //so right hand is being used to equip
            }
            indexOfR = indexToEquip;
        }
    }

    public Item dropFromEquip()
    {
        Item droppedItem = slots[indexOfR].GetComponent<HotbarSlot_UI>().getItem(); //this always drops whatever the right index is attatched to
        if (indexOfL != indexOfR) //if L,R are on diff 1-handed wpns
        {
            slots[indexOfR].GetComponent<HotbarSlot_UI>().emptySlot(); //empties R
            changeSlotEquipColors(indexOfR, false, false); //moves R to L
            changeSlotEquipColors(indexOfL, true, true);
            indexOfR = indexOfL;

        }
        else //L,R must be on the same spot
        {
            slots[indexOfR].GetComponent<HotbarSlot_UI>().emptySlot();
            changeSlotEquipColors(indexOfL, true, true); //technically the if statement code above actually works in all cases, I just don't like the extra assigning
        }
            return droppedItem;
    }
    public Item getEquipItem(bool isLeftHand)
    {
        int handIndex = isLeftHand ? indexOfL : indexOfR;
        return slots[handIndex].GetComponent<HotbarSlot_UI>().getItem();
    }
    //[Cu] still needs to make a general index drop method!!!! FIXXXXXXX

    void Start()
    {
        int temp = 1 + player.GetComponent<PlayerStats>().getStr() / 10;
        //this calculation is a prototype
        if (temp != numAvailableSlots)
        {
            for (int i = 0; i < totalPossibleSlots; i++)
            {
                slots[enableOrder[i]].SetActive(i < numAvailableSlots);
                slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().emptySlot();
            }
        }
        changeSlotEquipColors(indexOfR, true, true); //bc indexOfL == indexOfR
    }

    
    
}

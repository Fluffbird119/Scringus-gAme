using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// The hotbar handles which Items are actively equipped and/or stored. It also handles equipping, unequipping and weapon passive requirements.
/// If weapon passives remain exclusively as mere stat increases, it can handle those too.
/// 
/// *Important*: observe that many methods take and return objects of type 'Item' and *not* GameObjects
///     Given that 'Item' extends MonoBehaviour which extends Component (and a Component exists <=> it is attached to a gameObject),
///     any chosen 'Item' is attached to a GameObject. Because of this, the hotbar can easily require gameObjects be Items---
///     while also still effectively storing the GameObjects too.
///         Note that Item.gameObject returns the GameObject it is attached to, and the GetComponent method of GameObject can return its Item Component
/// </summary>


//[Cu]: NEEDS TO ADD: Call functions, finish setItemOrWhatev method, and handling requirements (and their passives too?)


/*
The methods are as follows: (NOT COMPLETE)

updateSlotNumber() 
    updates current available slots based on a players STR (should be called whenever this is modified)

setIndexEquip(int)
    literally just sets an index as equipped
 */


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


    public void updateSlotNumber() //should also be called when player str changes, Hotbar CANNOT regress!
    {
        int newAvailability = numAvailableSlotFormula();

        //this calculation is a prototype
        if (newAvailability > numAvailableSlots)
        {
            for(int i = numAvailableSlots; i < newAvailability; i++)
            {
                slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().setEnabledState(true);
            }
            numAvailableSlots = newAvailability;
        }
    }

    private void changeSlotEquipColors(int indexToChange, bool isLeftHand, bool isRightHand)
    {
        slots[indexToChange].GetComponent<HotbarSlot_UI>().setEquipColors(isLeftHand, isRightHand);
    }
    
    public void setIndexEquip(int indexToEquip) //Only 3 possible prev hand positions: L,R on 2handwpn/empty, L,R on same 1handwpn, L,R on diff 1handwpn
    {
        if(indexToEquip == indexOfL || indexToEquip == indexOfR)
        {
            if(indexOfL != indexOfR) //exclusively when a player has distinct L,R equip, and one hand is selected to equip, swaps hands
            {
                changeSlotEquipColors(indexOfL, false, true); //makes previous L to be R hand
                changeSlotEquipColors(indexOfR, true, false); //makes previous R to be L hand
                int tempIndex = indexOfL; //
                indexOfL = indexOfR;      // swapping the indices
                indexOfR = tempIndex;     //
            }
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

    public Item dropFromEquip() //method intended to be callable by hotkey
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
    public Item getEquipItem(bool isLeftHand) //returns equipped item on left or right hand (depending on bool) currently null for unequipped
    {
        int handIndex = isLeftHand ? indexOfL : indexOfR;
        return slots[handIndex].GetComponent<HotbarSlot_UI>().getItem();
    }
    
    public Item dropFromIndex(int indexToDrop)
    {
        Item droppedItem = slots[indexToDrop].GetComponent<HotbarSlot_UI>().getItem();
        if (indexToDrop == indexOfR)
        {
            dropFromEquip();
        }
        else if (indexToDrop == indexOfL) // bc of the else, if clause is true only when L,R on separate 1-handed items
        {
            slots[indexToDrop].GetComponent<HotbarSlot_UI>().emptySlot(); //empties L
            changeSlotEquipColors(indexOfL, false, false); //moves L to R
            changeSlotEquipColors(indexOfR, true, true);
            indexOfL = indexOfR;
        }
        else //when neither actively equipped hand is selected
        {
            slots[indexToDrop].GetComponent<HotbarSlot_UI>().emptySlot();
        }
        return droppedItem;
    }

    public void storeItem(Item itemToStore)
    {
        if (Object.Equals(slots[indexOfL].GetComponent<HotbarSlot_UI>().getItem(), null))
        {
            slots[indexOfL].GetComponent<HotbarSlot_UI>().setItem(itemToStore);
        }
        //FIXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX (I need to finish this)
    }

    void Start()
    {
        this.numAvailableSlots = numAvailableSlotFormula();
        //this calculation is a prototype
        for (int i = 0; i < totalPossibleSlots; i++)
        {
            slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().setEnabledState(i < this.numAvailableSlots);
        }

        changeSlotEquipColors(indexOfR, true, true); //bc indexOfL == indexOfR
    }


    private int numAvailableSlotFormula()
    {
        return 1 + player.GetComponent<PlayerStats>().getStr() / 10;
    }
    
    
}

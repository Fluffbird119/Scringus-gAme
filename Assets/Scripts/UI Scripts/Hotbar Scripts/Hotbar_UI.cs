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
The public methods are as follows: (yes they are alphabetical here, no I did not change the actual method order to reflect this)

dropFromEquip() : broadcasts HotbarItemDrop
    to be called by hotkey (Q), drops equipped item, but prefers to drop righthand if usnig two onehanded weapons
    

dropFromIndex(int) : broadcasts HotbarItemDrop
    self-explanatory, to be called by mouse interaction on hotbar slot

getItemEquip(bool)
    this returns either the left or right hand equipped item based on the bool (the player will call this to perform weapon actions)

setIndexEquip(int) : broadcasts HeldItemOnlyChange
    literally just sets an index as equipped by index (prefers to equip right hand if using 2 one-handed weapons) 
    (also if either index is already equipped, swaps L,R hands)
    To be called by mouse interaction on hotbar slot

storeItem(Item) : broadcasts HotbarStatusChange
    This attempts to slot an item into the hotbar. If the player is unarmed, the weapon will become equipped actively. Otherwise it fills an empty && enabled slot.
*Important*: the above method returns 'false' if there are not empty slots available (and of course doesn't slot anything). 
    additionally, this method does not know anything about the context of the attempted slotting and thus needs to be called by whatever is equipping (worldItem?)

updateSlotNumber(Component, object) : is a listener for PlayerStrChanged
    updates current available slots based on a players STR (is called whenever this is modified, hence the listener)


 */


public class Hotbar_UI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private GameObject HotbarPanel;

    [SerializeField] private List<GameObject> slots = new List<GameObject>(); //each 'slot' NEEDS the HotbarSlot_UI component

    [Header("Events")]
    [SerializeField] public GameEvent onHotbarItemDropped;  //calls whatever the other two call as well
    [SerializeField] public GameEvent onHotbarStatusChanged;//as in the contents of the hotbar are now different (also calls whatever onHeldItemOnlyChanged calls)
    [SerializeField] public GameEvent onHeldItemOnlyChanged;//as in merely the chosen equipped weapon is different


    private int numAvailableSlots = 1;

    private static readonly int totalPossibleSlots = 10;

    private static readonly int[] enableOrder = { 4, 5, 3, 6, 2, 7, 1, 8, 0, 9 };

    private int indexOfL = enableOrder[0]; //equipped LHand
    private int indexOfR = enableOrder[0]; //Equipped RHand


    public void updateSlotNumber(Component sender, object data) //also called when player str changes, Hotbar CANNOT regress!
    {
        if(data is int) //and data *should* be the player str stat! (the one unhindered by item/temporary effects)
        {
            int playerStr = (int)data;
            int newAvailability = numAvailableSlotFormula(playerStr);

            //this calculation is a prototype
            if (newAvailability > numAvailableSlots)
            {
                for (int i = numAvailableSlots; i < newAvailability; i++)
                {
                    slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().setEnabledState(true);
                }
                numAvailableSlots = newAvailability;
            }
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

        broadcastHeldItemOnlyChange();
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
            changeSlotEquipColors(indexOfL, true, true); //technically the if statement code above actually works in all cases, I just dislike the extra assigning
        }

        if (!Object.Equals(droppedItem, null))
        {
            broadcastHotbarItemDrop(droppedItem);
        }

        return droppedItem; //because of the broadcast, may no longer need to return this
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

        if (indexToDrop != indexOfR && !Object.Equals(droppedItem,null)) //the first condition is because dropFromEquip already handles broadcasing
        {
            broadcastHotbarItemDrop(droppedItem);
        }

        return droppedItem; //because of the broadcast, may no longer need to return this
    }

    

    void Start()
    {
        this.numAvailableSlots = numAvailableSlotFormula(this.player.GetComponent<PlayerStats>().getStr()); //initial call uses connected player GameObject

        for (int i = 0; i < totalPossibleSlots; i++)
        {
            slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().setEnabledState(i < this.numAvailableSlots);
            slots[enableOrder[i]].GetComponent<HotbarSlot_UI>().setParentHotbar(this, enableOrder[i]);
        }

        changeSlotEquipColors(indexOfR, true, true); //bc indexOfL == indexOfR
    }


    private int numAvailableSlotFormula(int playerStr) 
    {
        //this calculation is a prototype
        return 1 + playerStr / 10;
    }

    private void broadcastHotbarItemDrop(Item droppedItem) //currently this broadcast has sender = Hotbar_UI, data = Item
    {
        onHotbarItemDropped.Raise(this, droppedItem);
    }

    private void broadcastHotbarStatusChange() //currently this broadcast has sender = Hotbar_UI, data = List<Item>
    {
        List<Item> hotbarItemList = new List<Item>();
        foreach (GameObject slot in slots)
        {
            HotbarSlot_UI hs_UI = slot.GetComponent<HotbarSlot_UI>();
            if (hs_UI.isEnabled && !Object.Equals(hs_UI.getItem(), null)) //i.e. foreach non-empty enabled slot
                hotbarItemList.Add(hs_UI.getItem());
        }
        onHotbarStatusChanged.Raise(this, hotbarItemList);
    }

    private void broadcastHeldItemOnlyChange() //currently this broadcast has sender = Hotbar_UI, data = (Item, Item) [notably left equip, right equip]
    {
        onHeldItemOnlyChanged.Raise(this, (getEquipItem(true), getEquipItem(false)));
    }


    public bool storeItem(Item itemToStore) //method for when picking up an empty item, returns bool to indicate if it succeeded
    {
        bool hotbarHasSpace = false;
        if (Object.Equals(slots[indexOfL].GetComponent<HotbarSlot_UI>().getItem(), null)) //if unarmed
        {
            slots[indexOfL].GetComponent<HotbarSlot_UI>().setItem(itemToStore);
            hotbarHasSpace = true;
        }
        else
        {
            foreach (GameObject hotbarSlot in slots)
            {
                if (hotbarSlot.GetComponent<HotbarSlot_UI>().isEnabled && Object.Equals(hotbarSlot.GetComponent<HotbarSlot_UI>().getItem(), null))
                {
                    hotbarSlot.GetComponent<HotbarSlot_UI>().setItem(itemToStore);
                    hotbarHasSpace = true;
                    break;
                }
            }
        }

        if(hotbarHasSpace)
        {
            broadcastHotbarStatusChange();
        }

        return hotbarHasSpace;
    }

}

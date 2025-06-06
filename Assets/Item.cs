using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Object
{
    //general base class for items
    //this will be extended by classes like Projectile, Shield, Melee, etc, each of which will (eventually) have sealed extensions for their types

    //the following attributes are part of the items construction, and will rarely update within a game
    private GameObject prefab;

    private Dictionary<Item.PrimaryStats, float> PrimaryStatInnates = new Dictionary<Item.PrimaryStats, float>();
    private Dictionary<Item.SecondaryStats, float> SecondaryStatInnates = new Dictionary<Item.SecondaryStats, float>();

    private Dictionary<Item.PrimaryStats, float> PrimaryStatGrowths = new Dictionary<Item.PrimaryStats, float>();

    private bool isOneHanded;

    private string itemName; //as in name of item if looked at while on ground or in menu


    //the following attributes are general item features that can be handled by this abstract class
    private bool isEquipped = false;
    private Vector2 pos = new Vector2(); //as in position in game, particularly if unequipped and on the ground



    //the individual item types should handle the rendering/sprites

    //as an abstract class, its constructor will only be called by its inheriting classes, which is why it is so long
    private Item(GameObject prefab, Dictionary<Item.PrimaryStats,float> pStatInn, Dictionary<Item.SecondaryStats, float> sStatInn,
                 Dictionary<Item.PrimaryStats, float> pStatGrw, bool isOneHanded, string itemName)
    {
        this.prefab = prefab;
        this.PrimaryStatInnates = pStatInn;
        this.SecondaryStatInnates = sStatInn;
        this.PrimaryStatGrowths = pStatGrw;
        this.isOneHanded = isOneHanded;
        this.itemName = itemName;
    }

    //here are abstract methods all of the inheriting items will implement (it's almost like an interface!)
    public abstract void itemAction(); //basically the attack, but items like shields 'action may just be 'block'
    public abstract void itemActiveAbility(); //the active ability of the weapon
    public abstract void itemPassiveAbility(); // this maybe shouldn't be a function? I don't know what item passives will be like

    public abstract string itemActionDescription();
    public abstract string itemActiveDescription();
    public abstract string itemPassiveDescription();


    //items may have a way of being 'upgraded' in the future? if so, this should be added here

    public void displayItemOnGround()
    {
        //like how the item shows up on the ground (when hovered, it should eventually have a description, but for minimal product that is not needed
        //the gameObject may need to have a sprite atttatched to it to be rendered for this class to handle the display.
    }

    public void displayItemInInventory()
    {
        //I don't fathom how the inventory will work (is there one)?
    }

    public void displayItemOnPlayer()
    {

    }







    //Handling stats is below.
    //#################This is being informed by the 'satiohitnarbegiower' document (Jay & Andrew's document)###############################

    //Primary stats are basically the actual character stats (do weapons innately add to this by being equipped [Item.PrimaryStatInnates]????)
    //in general, CHARACTER Primary Stats are progressively increased by utilising certain weapons. This means weapons have stat 'GrowthRates'
    public enum PrimaryStats //also these don't need to be enums called by dictionaries, I just don't know how many there will be right now, 5???
    {
        //items in general will prolly call PrimaryStats via an enum or something else defined in the abstract player class (so this is temporary prolly)
        KINDNESS,         
        FRIENDSHIP,
        BLOODLUST,          
        SYMPATHY,           //yes right now my placeholders are not actual stats (and they prolly don't need to be all caps) FIXXXX
        UNDERSTANDING
    }

    //Secondary stats will actually be applied and typically just factor inPrimaryStats (and Item.SecondaryStatInnates)
    public enum SecondaryStats //also doesn't need to be an enum accessed by Dicts, but this one will prolly not be a fixed number as the game develops
    {
        //again, SECONDARY STATS WILL PROLLY BE ESTABLISHED IN THE ABSTRACT PLAYER CLASS (so this is prolly a temporary placeholder)
        //remember that adding more of these does not increase the time to call them, there may be many of these as items or characters may have 
            //specialised gimmick stats, which are otherwise not accessed and that is okay!

        //consistently important secondary stats
        MOVE_SPEED, 
        ATTACK_RANGE,
        ATTACK_SPD,
        LUCK,            //i.e. something that may affect drop rate of weapons, or increase gold gain
        MAX_HEALTH,
        MAX_RESOURCE,    //like mana
        RESOURCE_REGEN,

        //RESISTANCES, note that Secondary stats may incorporate other secondary stats! (bc MAGIC and PHYS res will incorporate gen res)
        GEN_RES,
        MAGIC_RES,       //currently I have lazily chosen magic and phys. Could always be based on elemental type or something else altogether 
        PHYS_RES,

        //Every stat below this line is probably not directly affected by stats w/o some kind of passive or item or whatever and are more fringe

        //DMG, note that actual weapon and ability dmg will be calculated in an attack function (since diff weapons will use diff stats for dmg)
        MAGIC_AMP,       //probably just influenced by items/passives
        PHYS_AMP,        //^^^
        GEN_AMP,         //^^^
        HEALING_AMP,

        //more misc that are very much subject to be removed or simply left at zero and unaccessed in most circumstances
        //yes, many of these are based on league because those stats are what Jay & Andrew's document give strong vibes of using, and it's something 
            //that most of us both recognise and have seen incorporated into items and characters (like via item/ability scalings)
        CRIT_CHANCE, //as a percent chance
        CRIT_DMG, //as a percent of actual dmg
        DODGE_CHANCE,
        LIFE_STEAL, //obviously doesn't mean anything rn, but 
        COOLDOWN_REDUC
    }
    //note that ON_HIT_EFFECTS, ON_KILL_EFFECTS, and ON_[NAME TRIGGER HERE]_EFFECTS will prolly be handled in part by scripts
        //and won't be incorporated into scalings (crit is not treated as an on hit effect)


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public abstract class Weapon : Item
{
    //general base class for weapons
    //this will be extended by classes like Projectile, Shield, Melee, etc, each of which will (eventually) have sealed extensions for their types

    //the following attributes are part of the weapon construction, and will rarely update within a game
    private GameObject prefab;

    private Dictionary<Weapon.PrimaryStats, float> PrimaryStatInnates = new Dictionary<Weapon.PrimaryStats, float>();
    private Dictionary<Weapon.SecondaryStats, float> SecondaryStatInnates = new Dictionary<Weapon.SecondaryStats, float>();

    private Dictionary<Weapon.PrimaryStats, float> PrimaryStatGrowths = new Dictionary<Weapon.PrimaryStats, float>();

    private bool isOneHanded;

    


    //the following attributes are general item features that can be handled by this abstract class
    private bool meetsPassiveReq = false; //passive will likely require a primary stat
    private bool isActiveOffCooldown = false;
    private bool isEquipped = false;
    private GameObject equippedWpnPrefab;
    private bool isInLeft = false; //only for one handed weapons

    //the actual item class should handle positioning on the ground and knowing if an item is in hotbar or not
    //private Vector2 pos = new Vector2(); //as in position in game, particularly if unequipped and on the ground



    //the individual wepon types should handle the rendering/sprites

    //as an abstract class, its constructor will only be called by its inheriting classes, which is why it is so long
    public Weapon(GameObject prefab, Dictionary<Weapon.PrimaryStats,float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                   Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, Item.ItemType itemType, 
                   string itemName) : base (prefab, itemType, itemName)
    {
        this.PrimaryStatInnates = pStatInn;
        this.SecondaryStatInnates = sStatInn;
        this.PrimaryStatGrowths = pStatGrw;
        this.isOneHanded = isOneHanded;
        this.prefab = prefab; 
    }

    //here are abstract methods all of the inheriting items will implement (it's almost like an interface!)
    public abstract void wpnAction(); //basically the attack, but items like shields 'action may just be 'block'
    public abstract void wpnActiveAbility(); //the active ability of the weapon
    public abstract void wpnPassiveAbility(); // this maybe shouldn't be a function? I don't know what item passives will be like
    //note that the passive actually functions when a given wepon is in an inventory.

    public abstract string wpnActionDescription();
    public abstract string wpnActiveDescription();
    public abstract string wpnPassiveDescription(); //includes stat requirement


    //items may have a way of being 'upgraded' in the future? if so, this should be added here

    

    public void createEquippedWeapon(GameObject playerGameObject) //intended to be implemented here
    {
        equippedWpnPrefab = Instantiate(this.prefab);
        equippedWpnPrefab.transform.SetParent(playerGameObject.transform); //player class needed to not make this a gameobject param
    }

    public void dropWeapon(GameObject playerGameObject)
    {
        dropItem(playerGameObject);
        if(isEquipped)
        {
            Destroy(equippedWpnPrefab);
        }
    }






    //Handling stats is below.
    //#################This is being informed by the 'satiohitnarbegiower' document (Jay & Andrew's document)###############################

    //Primary stats are basically the actual character stats (do weapons innately add to this by being equipped [Weapon.PrimaryStatInnates]????)
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

    //Secondary stats will actually be applied and typically just factor inPrimaryStats (and Weapon.SecondaryStatInnates)
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

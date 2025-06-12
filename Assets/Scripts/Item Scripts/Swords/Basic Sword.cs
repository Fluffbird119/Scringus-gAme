using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicSwordItem", menuName = "Inventory/Basic_Sword")]
public class BasicSword : MeleeWpn
{
    //note, requirements prolly should be passed, but stat growths are the ratios of stat gain given to the player character, not the requirements
    
    public static readonly string pathToSprite = "Sprite Assets/Item sprites/Weapon sprites/Basic Sword.ase";

    private static readonly Dictionary<Weapon.PrimaryStats, float> requirements = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.STR, 5 },
        { Weapon.PrimaryStats.DEX, 5 }
    };
    private static readonly Dictionary<Weapon.PrimaryStats, float> primaryStatModifiers = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.STR, 5 },
        { Weapon.PrimaryStats.DEX, 5 }
    };
    private static readonly Dictionary<Weapon.SecondaryStats, float> secondaryStatModifiers = new Dictionary<Weapon.SecondaryStats, float> 
    { 
        {Weapon.SecondaryStats.ATTACK_SPD, 5 },
        {Weapon.SecondaryStats.PHYS_AMP, 5 }
    };

    //private static bool isOneHanded = true;
    //private static Item.ItemType itemType = ItemType.MELEE_WPN;
    //private static string itemName = "Basic Sword";
    public BasicSword() : base (requirements, secondaryStatModifiers, primaryStatModifiers, true, pathToSprite)
    {
        //[Cu]: as it stands, I did not alter the order of dicts passed into base since the current system I might change when talking with the player grp
            //the innate stats are pure increases from having the weapon equipped. I intended for primary innates to be replaced with the requirements
            //the third dictionary is primary stat growths, these are the ratio stat increase for the player after progressively
            //killing enemies with a given weapon
    }

    
    override public void use(GameObject obj)
    {
        this.swingAnimation(obj.transform);
    }
    public override void wpnAction()
    {
        throw new System.NotImplementedException();
    }

    public override string wpnActionDescription()
    {
        throw new System.NotImplementedException();
    }

    public override void wpnAltAction() // since this is onehanded, it has no alt action
    {
        wpnAction();
    }
    public override string wpnAltActionDescription()
    {
        return wpnActionDescription();
    }


    public override void wpnPassive()
    {
        throw new System.NotImplementedException();
    }
    public override string wpnPassiveDescription()
    {
        throw new System.NotImplementedException();
    }

}

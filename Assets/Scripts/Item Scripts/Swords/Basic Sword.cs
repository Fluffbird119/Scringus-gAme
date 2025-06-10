using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : Weapon
{
    private static Dictionary<Weapon.PrimaryStats, float> requirements = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.CON, 0 },
        { Weapon.PrimaryStats.STR, 5 },
        { Weapon.PrimaryStats.DEX, 5 },
        { Weapon.PrimaryStats.INT, 0 },
        { Weapon.PrimaryStats.WIS, 0 },
    };
    private static Dictionary<Weapon.PrimaryStats, float> primaryStatModifiers = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.CON, 0 },
        { Weapon.PrimaryStats.STR, 5 },
        { Weapon.PrimaryStats.DEX, 5 },
        { Weapon.PrimaryStats.INT, 0 },
        { Weapon.PrimaryStats.WIS, 0 },
    };
    private static Dictionary<Weapon.SecondaryStats, float> secondaryStatModifiers = new Dictionary<Weapon.SecondaryStats, float> 
    { 
        {Weapon.SecondaryStats.MOVE_SPEED, 0 },
        { Weapon.SecondaryStats.ATTACK_RANGE, 0 },
        {Weapon.SecondaryStats.ATTACK_SPD, 0 },
        {Weapon.SecondaryStats.LUCK, 0 },
        {Weapon.SecondaryStats.MAX_HEALTH, 0 },
        {Weapon.SecondaryStats.MAX_MANA, 0 },
        {Weapon.SecondaryStats.MANA_REGEN, 0 },
        {Weapon.SecondaryStats.GEN_RES, 0 },
        {Weapon.SecondaryStats.MAGIC_RES, 0 },
        {Weapon.SecondaryStats.PHYS_RES, 0 },
        {Weapon.SecondaryStats.MAGIC_AMP, 0 },
        {Weapon.SecondaryStats.PHYS_AMP, 0 },
        {Weapon.SecondaryStats.GEN_AMP, 0 },
        {Weapon.SecondaryStats.HEALING_AMP, 0 },
        {Weapon.SecondaryStats.CRIT_CHANCE, 0 },
        {Weapon.SecondaryStats.CRIT_DMG, 0 },
        {Weapon.SecondaryStats.DODGE_CHANCE, 0 },
        {Weapon.SecondaryStats.LIFE_STEAL, 0 },
        {Weapon.SecondaryStats.COOLDOWN_REDUCE, 0 } 
    };
    private static bool isOneHanded = true;
    private static Item.ItemType itemType = ItemType.MELEE_WPN;
    private static string itemName = "Basic Sword";
    public BasicSword(GameObject prefab) : base(prefab, requirements, secondaryStatModifiers, 
        primaryStatModifiers, isOneHanded, itemType, itemName)
    {

    }

    public override void wpnAction()
    {
        throw new System.NotImplementedException();
    }

    public override string wpnActionDescription()
    {
        throw new System.NotImplementedException();
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

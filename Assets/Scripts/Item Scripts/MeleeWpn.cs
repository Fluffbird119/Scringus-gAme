using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWpn : Weapon
{
    
    public MeleeWpn(GameObject prefab, Dictionary<Weapon.PrimaryStats, float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                   Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, string itemName) : base (prefab, pStatInn, sStatInn,
                   pStatGrw, isOneHanded,Item.ItemType.MELEE_WPN, itemName)
    {
        //note that this goofy constructor passes virtually everything except that it innately can supply itemType
    }

    private void swingAnimation()
    {
        //needs to be coded (to be used by inheriting members)
    }

    private void joustAnimation()
    {
        //needs to be coded (to be used by inheriting members)
    }




}

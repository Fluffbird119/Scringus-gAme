using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWpn : Weapon
{
    
    public MeleeWpn(Dictionary<Weapon.PrimaryStats, float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                    Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, string pathToSprite) : base (pStatInn, 
                    sStatInn, pStatGrw, isOneHanded, Item.ItemType.MELEE_WPN, pathToSprite)
    {
        //note that this goofy constructor passes virtually everything except that it innately can supply itemType
    }

    private void swingAnimation(GameObject spriteToSwing) //the GameObject clone that is actively rendering on the player should go here
    {
        spriteToSwing.transform.Rotate(0, 90, 0);
    }

    private void joustAnimation(GameObject spriteToJoust) //the GameObject clone that is actively rendering on the player should go here)
    {
        throw new System.NotImplementedException();
    }




}

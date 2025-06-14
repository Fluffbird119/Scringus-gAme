using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWpn : Weapon
{
    private int numFrames = 10000;
    public MeleeWpn(Dictionary<Weapon.PrimaryStats, float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                    Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, string pathToSprite) : base (pStatInn, 
                    sStatInn, pStatGrw, isOneHanded, Item.ItemType.MELEE_WPN, pathToSprite)
    {
        //note that this goofy constructor passes virtually everything except that it innately can supply itemType
    }

    protected void swingAnimation(Transform weaponTransform) //the GameObject clone that is actively rendering on the player should go here
    {
        for (int i = 0; i < numFrames; i++)
        {
            weaponTransform.Rotate(0, 0, -90 / numFrames);
        }
    }

    protected void joustAnimation(GameObject weaponPrefab) //the GameObject clone that is actively rendering on the player should go here)
    {
        throw new System.NotImplementedException();
    }




}

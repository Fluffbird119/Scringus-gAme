using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWpn : Weapon
{

    
    public ProjectileWpn(Dictionary<Weapon.PrimaryStats, float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                    Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, string pathToSprite) : base(pStatInn,
                    sStatInn, pStatGrw, isOneHanded, Item.ItemType.PROJECTILE_WPN, pathToSprite)
    {
        //note that this goofy constructor passes virtually everything except that it innately can supply itemType
    }
    
    private void swingAnimation(GameObject projectileSprite)
    {
        throw new System.NotImplementedException();
    }

    private void joustAnimation()
    {
        throw new System.NotImplementedException();
    }




}

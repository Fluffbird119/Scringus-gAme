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
    
    protected void firingAnimation(Transform projectileSource)
    {
        projectileSource.Rotate(0, 0, -22.5f);
        Debug.Log("fire animation");
    }

    protected void fireProjectile(Vector3 start, Vector3 end, GameObject projectile)
    {
        GameObject projectileObj = Instantiate(projectile, start, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody2D>().AddForce((end - start).normalized * 1000);
    }




}

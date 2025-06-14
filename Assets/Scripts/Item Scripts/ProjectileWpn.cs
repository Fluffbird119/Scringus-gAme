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
    }

    protected void fireProjectile(Vector3 start, Vector3 end, GameObject projectile)
    {
        Debug.Log(projectile.transform.eulerAngles.z);
        GameObject projectileObj = Instantiate(projectile, start, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody2D>().AddForce((end - start).normalized * 1000);

        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        Vector3 projectileAngle = new Vector3(0, 0, angle + projectile.transform.eulerAngles.z);
        projectileObj.transform.rotation = Quaternion.Euler(projectileAngle);

        //Debug.DrawLine(start, end, Color.green, 2f);
        //Debug.DrawRay(start, (end - start).normalized, Color.red, 2f);
    }




}

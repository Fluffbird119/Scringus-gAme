using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicStaffItem", menuName = "Inventory/Basic_Staff")]
public class BasicStaff : ProjectileWpn
{
    public static readonly string pathToWeaponSprite = "Sprite Assets/Item sprites/Weapon sprites/Basic Staff.ase";
    public static readonly string pathToProjectileObj = "Prefabs/Weapon Prefabs/Basic Magic Projectile";

    private static readonly Dictionary<Weapon.PrimaryStats, float> requirements = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.INT, 7 }
    };
    private static readonly Dictionary<Weapon.PrimaryStats, float> primaryStatModifiers = new Dictionary<Weapon.PrimaryStats, float>
    {
        { Weapon.PrimaryStats.INT, 5 },
    };
    private static readonly Dictionary<Weapon.SecondaryStats, float> secondaryStatModifiers = new Dictionary<Weapon.SecondaryStats, float>
    {
        {Weapon.SecondaryStats.MAGIC_AMP, 5 }
    };

    //private static bool isOneHanded = true;
    //private static Item.ItemType itemType = ItemType.MELEE_WPN;
    //private static string itemName = "Basic Sword";
    public BasicStaff() : base(requirements, secondaryStatModifiers, primaryStatModifiers, true, pathToWeaponSprite)
    {
        //[Cu]: as it stands, I did not alter the order of dicts passed into base since the current system I might change when talking with the player grp
        //the innate stats are pure increases from having the weapon equipped. I intended for primary innates to be replaced with the requirements
        //the third dictionary is primary stat growths, these are the ratio stat increase for the player after progressively
        //killing enemies with a given weapon
    }


    override public void use(GameObject obj)
    {
        this.firingAnimation(obj.transform);
        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouse);
        mousePos.z = 0;
        Debug.Log("mouseX: " + mousePos.x);
        Debug.Log("mouseY: " + mousePos.y);
        Debug.Log("near: " + Camera.main.nearClipPlane);
        Debug.Log("far: " + Camera.main.farClipPlane);
        this.fireProjectile(obj.transform.position, mousePos, Resources.Load<GameObject>(pathToProjectileObj));
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

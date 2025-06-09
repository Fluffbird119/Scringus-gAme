using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : Weapon
{
    public BasicSword(GameObject prefab, GameObject playerObject, Dictionary<Weapon.PrimaryStats, float> pStatInn, Dictionary<Weapon.SecondaryStats, float> sStatInn,
                   Dictionary<Weapon.PrimaryStats, float> pStatGrw, bool isOneHanded, Item.ItemType itemType,
                   string itemName) : base(prefab, playerObject, pStatInn, sStatInn, pStatGrw, isOneHanded, itemType, itemName)
    {

    }

    public BasicSword(GameObject prefab, GameObject playerObject) : base(prefab, playerObject) { }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Halberd : MeleeWpn
{
    //THIS IS A PROTOTYPE CLASS
    public GameObject halberdPrefab; // to be set
    
    
    
    public Halberd(GameObject halberdPrefab) : base (halberdPrefab,null,null,null,false,"Halberd")
    {
        this.halberdPrefab = halberdPrefab;
    }

    public override void wpnAction() //basically the attack, but items like shields 'action may just be 'block'
    {

    }
    public override void wpnActiveAbility() //the active ability of the weapon
    {

    }
    public override void wpnPassiveAbility()
    {

    }// this maybe shouldn't be a function? I don't know what item passives will be like
    //note that the passive actually functions when a given wepon is in an inventory.

    public override string wpnActionDescription()
    {
        return "";
    }
    public override string wpnActiveDescription()
    {
        return "";
    }
    public override string wpnPassiveDescription()
    {
        return "";
    }//includes stat requirement



}

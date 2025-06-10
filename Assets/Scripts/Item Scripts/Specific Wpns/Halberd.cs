using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class Halberd : MeleeWpn
{
    //THIS IS A PROTOTYPE CLASS


    //with this, just calling badGameObject.AddComponent<Halberd>(); will automatically provide spriteRenderer and the sprite if sprite renderer isnt there
    public static readonly string pathToSprite = "Assets/Resources/Sprite Assets/Item sprites/Weapon sprites/Halberd_Sprite.ase";


    public Halberd() : base (null,null,null,false, Halberd.pathToSprite)
    {

    }
    

    public override void wpnAction() //basically the attack, but items like shields 'action may just be 'block'
    {
        Debug.Log("Halberd performed wpnAction...");
    }
    public override void wpnAltAction() //the active ability of the weapon**** OR IF DUAL WEILDING, THE SECONDARY wpnAction
    {
        Debug.Log("Halberd performed wpnActiveAbility...");
    }
    public override void wpnPassive()
    {
        Debug.Log("Halberd performed wpnPassiveAbility (yeah I know it should be 'performed,' fool)...");
    }// this maybe shouldn't be a function? I don't know what item passives will be like
    //note that the passive actually functions when a given weapon is in an inventory.

    public override string wpnActionDescription()
    {
        Debug.Log("Halberd performed wpnActionDescription????");
        return "";
    }
    public override string wpnAltActionDescription()
    {
        Debug.Log("Halberd performed wpnActiveDescription????");
        return "";
    }
    public override string wpnPassiveDescription()
    {
        Debug.Log("Halberd performed wpnPassiveDescription????");
        return "";
    }//includes stat requirement



}

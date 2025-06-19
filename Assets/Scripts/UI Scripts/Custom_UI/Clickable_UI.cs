using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is a replacement for Unity's UI 'butttons'
/// 
/// This should mostly just extend the OnPointer Click function, but may be made later to have toggleable other functions
/// </summary>

public class Clickable_UI : MonoBehaviour, IPointerClickHandler
{
    public CustomGameEvent leftClickResponse; //note that CustomGameEvent is from file GameEventListener (however this is NOT a broadcast!)
    public CustomGameEvent rightClickResponse;//Also note that CustomGameEvent is just a unityEvent but one that stores <Component sender, object data>

    
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClickResponse.Invoke(this, eventData);
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClickResponse.Invoke(this, eventData);
    }

    //currently neither prevents clicking through the ui (maybe it already handles this)
    //nor handles anything but pointer click
}

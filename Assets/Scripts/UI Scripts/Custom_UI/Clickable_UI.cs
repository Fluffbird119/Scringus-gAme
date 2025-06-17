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
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException(); //I'd prefr to make a general invoke that doesn't require a broadcast
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

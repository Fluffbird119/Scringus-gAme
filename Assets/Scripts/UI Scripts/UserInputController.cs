using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//[Cu]: this is mostly copied from a blog-post to prevent clicks going through the UI (because unity doesn't handle this itself)
public class UserInputController : MonoBehaviour
{
    // The GraphicRaycaster of your Canvas game object
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    // Struct to hold pointer data (mainly its position)
    private PointerEventData _clickData;
    // List containing all the UI elements hit by the raycast
    private List<RaycastResult> _raycastResults;

    private void Start()
    {
        _clickData = new PointerEventData(EventSystem.current);
        _raycastResults = new List<RaycastResult>();
    }

    /*public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        // Check that user did not click over a UI element
        if (HasClickedOverUI())
        {
            return;
        }

        // Game logic continues here...
        // For example, call other C# methods, play a sound, instantiate 
        // a game object, etc...
    }

    private bool HasClickedOverUI()
    {
        // Retrieve current mouse position
        _clickData.position = Mouse.current.position.ReadValue();
        // Clear previous results
        _raycastResults.Clear();
        // Instruct the raycaster to cast a ray from current mouse
        // and stores the results in the given array
        graphicRaycaster.Raycast(_clickData, _raycastResults);

        // Optional: log all the UI elements hit by the ray
        foreach (var raycastResult in _raycastResults)
        {
            Debug.Log($"Clicked in UI element: ${raycastResult}");
        }

        // Return a boolean that will tell us whether at least one
        // UI element has been clicked on 
        return _raycastResults.Count > 0;
    }
    */
}

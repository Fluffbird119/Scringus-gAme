using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabMenu_UI : MonoBehaviour
{
    
    [SerializeField] private GameObject tabMenuPanel;

    private void Start()
    {
        tabMenuPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleMenu();
        }
    }

    public void toggleMenu()
    {
        tabMenuPanel.SetActive(!tabMenuPanel.activeSelf);
    }
}

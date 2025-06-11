using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    private bool isOpen = false;
    private bool playerIsNear = false;
    void Start()
    {
    }

    void Update()
    {
        if (playerIsNear && !isOpen && Input.GetKeyDown(KeyCode.E))
        {
            open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIsNear = false;
    }

    private void open()
    {
        if (isOpen)
        {
            return;
        }
        isOpen = true;

        ItemGeneration.spawnItem("Prefabs/Weapon Prefabs/Basic Sword", transform.position);
    }
}

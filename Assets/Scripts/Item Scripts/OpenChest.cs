using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

        float randDeltaX = Random.Range(-1, 1);
        float randDeltaY = Random.Range(-1, 1);
        Vector3 itemPos = new Vector3(transform.position.x + randDeltaX, transform.position.y + randDeltaY, transform.position.z);
        ItemGeneration.spawnItem(ItemGeneration.getRandomWeapon(), itemPos);
    }
}

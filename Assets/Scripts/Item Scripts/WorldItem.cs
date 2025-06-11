using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldItem : MonoBehaviour
{
    public ItemData item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory player = other.GetComponent<Inventory>();
        if (player != null)
        {
            player.pickUp(item);
            Destroy(gameObject);
        }
    }
}

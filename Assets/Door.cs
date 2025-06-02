using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Boundary
{


    private float doorWidth;
    private float doorHeight;

    private Renderer topRend;
    private Renderer botRend;

    public Door(GameObject prefab, Vector2 pos, Room room1, Room room2) : base(prefab, pos, room1, room2)
    {
        topRend = prefab.transform.GetChild(0).GetComponent<Renderer>();
        botRend = prefab.transform.GetChild(1).GetComponent<Renderer>();

        if (prefab.transform.rotation.z == 0)
        {
            doorWidth = topRend.bounds.size.x / Room.ROOM_UNIT;
            doorHeight = (topRend.bounds.size.y + botRend.bounds.size.y) / Room.ROOM_UNIT;
        }
        else
        {
            doorWidth = (topRend.bounds.size.x + botRend.bounds.size.x) / Room.ROOM_UNIT;
            doorHeight = topRend.bounds.size.y / Room.ROOM_UNIT;

        }
    }
}

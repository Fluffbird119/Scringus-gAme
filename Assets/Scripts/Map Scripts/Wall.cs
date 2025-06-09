using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Boundary
{
    private float roomWidth;
    private float roomHeight;

    private Renderer rend;

    public Wall(GameObject prefab, Vector2 pos, Room room1, Room room2) : base(prefab, pos, room1, room2)
    {
        rend = prefab.GetComponent<Renderer>();

        roomWidth = rend.bounds.size.x / Room.ROOM_UNIT;
        roomHeight = rend.bounds.size.y / Room.ROOM_UNIT;
    }

}

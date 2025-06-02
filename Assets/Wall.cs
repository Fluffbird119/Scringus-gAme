using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Boundary
{
    private float roomWidth;
    private float roomHeight;


    private GameObject prefab;
    private Renderer rend;

    private Vector2 pos;

    private MapGenScript mapGen;

    public Wall(GameObject prefab, Vector2 pos, Room room1, Room room2) : base(room1, room2)
    {
        this.prefab = prefab;

        rend = prefab.GetComponent<Renderer>();

        roomWidth = rend.bounds.size.x / Room.ROOM_UNIT;
        roomHeight = rend.bounds.size.y / Room.ROOM_UNIT;

        this.pos = pos;
    }

}

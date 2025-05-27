using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Room : Object
{
    private float roomWidth;
    private float roomHeight;

    public static readonly float ROOM_UNIT = 10;

    private GameObject prefab;
    private Renderer rend;

    private Vector2 pos;

    public Room(GameObject prefab, Vector2 pos)
    {
        this.prefab = prefab;

        rend = prefab.GetComponent<Renderer>();

        roomWidth = rend.bounds.size.x / ROOM_UNIT;
        roomHeight = rend.bounds.size.y/ ROOM_UNIT;

        this.pos = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Object
{


    private float doorWidth;
    private float doorHeight;


    private GameObject prefab;
    private Renderer topRend;
    private Renderer botRend;

    private Vector2 pos;

    private MapGenScript mapGen;
    public Door(GameObject prefab, Vector2 pos)
    {
        this.prefab = prefab;

        topRend = prefab.transform.GetChild(0).GetComponent<Renderer>();
        botRend = prefab.transform.GetChild(1).GetComponent<Renderer>();

        if (prefab.transform.rotation.z == 0)
        {
            doorWidth = topRend.bounds.size.x / Room.ROOM_UNIT;
            doorHeight = (topRend.bounds.size.y + botRend.bounds.size.y) / Room.ROOM_UNIT;
        } else
        {
            doorWidth = (topRend.bounds.size.x + botRend.bounds.size.x) / Room.ROOM_UNIT;
            doorHeight = topRend.bounds.size.y / Room.ROOM_UNIT;

        }

        this.pos = pos;
    }
}

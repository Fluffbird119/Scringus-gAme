using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : Object
{
    //currently Door and Wall hold their Width and Height
    /*private boundaryWidth;
    private boundaryHeight;*/

    private Vector2 pos;

    private Room room1;
    private Room room2;

    private GameObject prefab;
    private MapGenScript mapGen;

    //Boundaries currently don't innately possess attributes of any room to store it because their rooms may be null (especially for walls)
    public Boundary(GameObject prefab, Vector2 pos, Room room1, Room room2)
    {
        this.prefab = prefab;
        this.pos = pos;
        this.room1 = room1;
        this.room2 = room2;
    }

    public Room getRoom1()
    {
        return this.room1;
    }

    public Room getRoom2()
    {
        return this.room2;
    }

    public Vector2 getPos()
    {
        return this.pos;
    }

    public GameObject getGameObject()
    {
        return prefab;
    }

    //unique function that sees if exactly one room is null, and then attaches the input room to sais null room (for appending a room onto a boundary)
    public void attachRoom(Room attachingRoom) //notably does nothing if the wall has no nulls (nowhere to attach) or has both nulls (how could this even occur???)
    {
        bool isRoom1Null = System.Object.Equals(this.room1, null);
        bool isRoom2Null = System.Object.Equals(this.room2, null);

        if(isRoom1Null && !isRoom2Null) //the case in which room1 is null and room2 specifically isn't null
        {
            this.room1 = attachingRoom;
        }
        else if (!isRoom1Null && isRoom2Null) //the case in which room2 is null and room1 specifically isn't null
        {
            this.room2 = attachingRoom;
        }
    }
}

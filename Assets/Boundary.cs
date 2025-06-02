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

    public Room getOtherRoom(Room inputRoom)
    {
        return room1 == inputRoom ? room2 :
               room2 == inputRoom ? room1 :
                                    null; //null is in case inputRoom is unrelated
    }

    //walls (exclusively) can attach a room to nowhere (i.e. be on border), in which case we'll return a null here
    public Room getOtherRoom(EnclosedArea inputEnclosedArea) //uses an enclosed area to find the room on the other end
    {
        Room bucketRoom = null;
        if (room1 != null && room2 != null) //makes sure both rooms are able to call their enclosedArea
        {
            bucketRoom = (room1.getEnclosedArea() == inputEnclosedArea) ? room2 :
                         (room2.getEnclosedArea() == inputEnclosedArea) ? room1 :
                                                                          null;
        }
        return bucketRoom;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : Object
{
    Room room1;
    Room room2;

    public Boundary(Room room1, Room room2)
    {
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
}

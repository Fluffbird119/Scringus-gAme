using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosedArea : Object
{
    List<Room> rooms;
    List<Boundary> boundaries;
    public EnclosedArea(List<Room> rooms)
    {
        this.rooms = rooms;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosedArea : Object
{
    //currently EnclosedAreas can access their composed rooms and all the traversible boundaries on their edges
    List<Room> rooms;
    List<Boundary> travBounds; //travBounds are traversible boundaries (e.g. doors), and operate like 'edges' in a graph
    public EnclosedArea(List<Room> rooms)
    {
        this.rooms = rooms;
    }

    //rooms don't have access to their boundaries, so boundaries will be scraped from all NON-WALL boundaries and added in
    public void addBoundary(Boundary travBoundary)
    {
        travBounds.Add(travBoundary);
    }
}

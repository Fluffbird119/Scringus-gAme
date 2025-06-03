using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosedArea : Object
{
    //currently EnclosedAreas can access their composed rooms and all the traversible boundaries on their edges
<<<<<<< HEAD
    private List<Room> rooms;
    private List<Boundary> travBounds; //travBounds are traversible boundaries (e.g. doors), and operate like 'edges' in a graph
=======
    List<Room> rooms;
    List<Boundary> travBounds; //travBounds are traversible boundaries (e.g. doors), and operate like 'edges' in a graph
>>>>>>> b71a179dd27a3f7fa0497db87e73102d6daa7abe
    public EnclosedArea(List<Room> rooms)
    {
        this.rooms = rooms;
        foreach(Room room in rooms)
        {
            room.setEnclosedArea(this);
        }
        this.travBounds = new List<Boundary>();
    }

    public EnclosedArea(Room room) //Initialised as disjoint set w/ one room
    {
        this.rooms = new List<Room>();
        rooms.Add(room); //does not set the enclosedArea for the room to be 'this' because this constructor should be set in its call
        this.travBounds = new List<Boundary>();
    }

    public static void union(EnclosedArea alpha, EnclosedArea beta) //merges beta contents into alpha EnclosedArea
    {
        if(alpha == beta) return; //instantly resloves if they are the same

        foreach(Room betaRoom in beta.rooms)
        {
            alpha.rooms.Add(betaRoom);
            betaRoom.setEnclosedArea(alpha);
        }
        foreach(Boundary betaTravBound in beta.travBounds) //this is to make EnclosedArea.union() more general, but ultimately unnecesary
        {
            if (betaTravBound.getRoom1().getEnclosedArea() == betaTravBound.getRoom2().getEnclosedArea()) //if the boundary is within the union
            {
                beta.travBounds.Remove(betaTravBound); //if boundary is between alpha and beta, it is removed from both (there shouldn't be any anyway)
                alpha.travBounds.Remove(betaTravBound);
            }
            else
            {
                alpha.travBounds.Add(betaTravBound);
            }
        }
    }


    //rooms don't have access to their boundaries, so boundaries will be scraped from all NON-WALL boundaries and added in
    public void addBoundary(Boundary travBoundary)
    {
        travBounds.Add(travBoundary);
    }

    //rooms don't have access to their boundaries, so boundaries will be scraped from all NON-WALL boundaries and added in
    public void addBoundary(Boundary travBoundary)
    {
        travBounds.Add(travBoundary);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnclosedArea : Object
{
    //currently EnclosedAreas can access their composed rooms and all the traversible boundaries on their edges
    private List<Room> rooms;
    private List<Boundary> travBounds; //travBounds are traversible boundaries (e.g. doors), and operate like 'edges' in a graph

    //the attributes below are to stay false and null unless actively being used as a graph (fns should clear this info upon each initial access of an EnclosedArea)
    private bool known = false;
    private EnclosedArea prevEncArea = null;

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
        if(haveBeenUnioned(alpha,beta)) return; //instantly resloves if they are the same

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
    public List<Boundary> getTravBoundList()
    {
        return this.travBounds;
    }

    public override string ToString()
    {
        return rooms[0].getGameObject().ToString();
    }

    //out of necessity, EnclosedAreas now are designated by rooms[0].getGameObject (i.e. their first room) Enclosed areas act like disjoint sets, so this is a unique designation
    public static bool haveBeenUnioned(EnclosedArea encArea1, EnclosedArea encArea2) //is basically a quick .equals (do not use == since this isn't a GameObject)
    {
        return encArea1.rooms[0].getGameObject() == encArea2.rooms[0].getGameObject();
    }



    //graph traversal fns for pathing (most will be in mapGen)
    public bool isKnown()
    {
        return this.known;
    }
    public EnclosedArea previousArea()
    {
        return this.prevEncArea;
    }
    public void clearVertexInfo() //upon first finding an enclosedArea in a fn, make sure to call this (it prolly won't be cleared after the previous fn usage)
    {
        this.known = false;
        this.prevEncArea = null;
    }
    public void setPreviousArea(EnclosedArea prevEncArea)
    {
        this.prevEncArea = prevEncArea;
    }
    public void makeKnown()
    {
        this.known = true;
    }

    public List<Room> getRooms()
    {
        return rooms;
    }

}

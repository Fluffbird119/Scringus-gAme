using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
//using System;


public class Room : Object
{
    private float roomWidth;
    private float roomHeight;

    public static readonly float ROOM_UNIT = 10;


    private GameObject prefab;
    private Renderer rend;
    public int roomValue;
    private Vector2 pos;
    private EnclosedArea enclosedArea = null; //refers back to 'parent' enclosed area, not set in constructor

    private MapGenScript mapGenScript;
    public Room(GameObject prefab, Vector2 pos, int roomValue)
    {
        this.prefab = prefab;
        this.roomValue = roomValue;
        this.pos = pos;

        rend = prefab.GetComponent<Renderer>();

        roomWidth = rend.bounds.size.x / ROOM_UNIT;
        roomHeight = rend.bounds.size.y / ROOM_UNIT;

        //this.prefab.name = "Room " + mapGen.roomMap[(int)Math.Round(pos.x/ROOM_UNIT), (int)Math.Round(pos.y/ROOM_UNIT)].ToString();
    }

    public Vector2 getPos()
    {
        return pos;
    }

    //enclosed area is set separately from constructor, should be set in mapgen
    public EnclosedArea getEnclosedArea()
    {
        return this.enclosedArea;
    }

    public void setEnclosedArea(EnclosedArea enclosedArea)
    {
        this.enclosedArea = enclosedArea;
    }

    public void setRoomValue(int roomValue)
    {
        this.roomValue = roomValue;
    }

    public GameObject getGameObject()
    {
        return this.prefab;
    }

    public int getDistanceFromRoom(Room room)
    {
        int x = -1;
        int y = 0;
        for (int i = 0; i < MapGenScript.MAP_WIDTH; i++)
        {
            for (int j = 0; j < MapGenScript.MAP_WIDTH; j++)
            {
                if (MapGenScript.rooms[j, i].Equals(room))
                {
                    x = i; y = j;
                }
            }
        }
        int distance = (int)this.pos.x - x + (int)this.pos.y - y;
        if (distance > 0)
        {
            return distance;
        }
        return distance * -1;
    }

    //these return the array index using room position
    public int col()
    {
        return (int)(this.pos.x / ROOM_UNIT);
    }
    public int row()
    {
        return (int)(MapGenScript.MAP_HEIGHT - this.pos.y / ROOM_UNIT) - 1; 
    }


    public enum Direction { UP = 1, RIGHT = 2, DOWN = 3, LEFT = 4 };
    public Dictionary<Direction, Door> GetDoors()
    {
        Dictionary<Direction, Door> doors = new Dictionary<Direction, Door>();

        Vector2[] keyArray = MapGenScript.doorMap.Keys.ToArray();
        Vector2 up = new Vector2(this.pos.x, this.pos.y + ROOM_UNIT * 0.5f);
        if (keyArray.Contains(up)) 
        {
            doors[Direction.UP] = MapGenScript.doorMap[up];
        }
        Vector2 right = new Vector2(this.pos.x + ROOM_UNIT * 0.5f, this.pos.y);
        if (keyArray.Contains(right))
        {
            doors[Direction.RIGHT] = MapGenScript.doorMap[right];
        }
        Vector2 down = new Vector2(this.pos.x, this.pos.y - ROOM_UNIT * 0.5f);
        if (keyArray.Contains(down))
        {
            doors[Direction.DOWN] = MapGenScript.doorMap[down];
        }
        Vector2 left = new Vector2(this.pos.x - ROOM_UNIT * 0.5f, this.pos.y);
        if (keyArray.Contains(left))
        {
            doors[Direction.LEFT] = MapGenScript.doorMap[left];
        }
        return doors;
    }

    public Dictionary<int, Wall> getWalls()
    {
        Dictionary<int, Wall> walls = new Dictionary<int, Wall>();

        Vector2[] keyArray = MapGenScript.wallMap.Keys.ToArray();
        Vector2 up = new Vector2(this.pos.x, this.pos.y + ROOM_UNIT * 0.5f);
        if (keyArray.Contains(up))
        {
            walls[(int)Direction.UP] = MapGenScript.wallMap[up];
        }
        Vector2 right = new Vector2(this.pos.x + ROOM_UNIT * 0.5f, this.pos.y);
        if (keyArray.Contains(right))
        {
            walls[(int)Direction.RIGHT] = MapGenScript.wallMap[right];
        }
        Vector2 down = new Vector2(this.pos.x, this.pos.y - ROOM_UNIT * 0.5f);
        if (keyArray.Contains(down))
        {
            walls[(int)Direction.DOWN] = MapGenScript.wallMap[down];
        }
        Vector2 left = new Vector2(this.pos.x - ROOM_UNIT * 0.5f, this.pos.y);
        if (keyArray.Contains(left))
        {
            walls[(int)Direction.LEFT] = MapGenScript.wallMap[left];
        }
        return walls;
    }

    override
    public bool Equals(object obj)
    {
        Room room = obj as Room;
        if (this.row() == room.row() && this.col() == room.col())
        {
            return true;
        }
        return false;
    }

    override
    public string ToString()
    {
        return "x: " + this.row() + ", " + this.col();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public List<Boundary> getPathBetweenRooms(Room roomA, Room roomB)
    {
        List<Boundary> path = new List<Boundary>();

        int minDist = MapGenScript.MAP_WIDTH * MapGenScript.MAP_HEIGHT;
        foreach (EnclosedArea area in findKnownAreas(roomA))
        {
            foreach (Room currRoom in area.getRooms())
            {
                int currDist = roomB.getDistanceFromRoom(currRoom);
                if (currDist < minDist)
                {
                    minDist = currDist;
                }
            }
        }
        return path;
    }

    //takes any 2 rooms and sees if they are connected by some path
    public static bool areRoomsConnected(Room roomA, Room roomB)
    {
        spanningTree(roomA); //changes all 'known' and 'previousArea' values to reflect paths from roomA
        return roomB.getEnclosedArea().isKnown();
        //Enclosed areas currently have no toString method or really any designation, but this 'spanningTree can very easily generate the path backwards
    }

    private static void spanningTree(Room initRoom) //all this does is establish the proper 'known' and 'previousArea' values
    {
        foreach (Room room in MapGenScript.rooms)
        {
            room.getEnclosedArea().clearVertexInfo(); //this wipes the vertex info for everything immediately
        }
        EnclosedArea initEncArea = initRoom.getEnclosedArea();
        //List<EnclosedArea> foundEnclosedAreas = new List<EnclosedArea>(); //this list tells me which enclosed areas I have seen
        Queue<EnclosedArea> queue = new Queue<EnclosedArea>();
        queue.Enqueue(initEncArea);
        while (queue.Count > 0)
        {
            EnclosedArea currentEncArea = queue.Dequeue();
            currentEncArea.makeKnown();
            foreach (Boundary edge in currentEncArea.getTravBoundList())
            {
                EnclosedArea debugOption1 = edge.getRoom1().getEnclosedArea(); //the next several lines literally just choose the side of the boundary
                EnclosedArea debugOption2 = edge.getRoom2().getEnclosedArea(); //that isn't the currrentEncArea, but due to a whole lot of debugging, 
                EnclosedArea neighbour = null;                                 //I ultimately deleted the fn that did this and left the debug that works here.
                if (EnclosedArea.haveBeenUnioned(currentEncArea, debugOption1))//I'll prolly optimise it at some point if needed -[Cu]
                {
                    neighbour = debugOption2;
                }
                else if (EnclosedArea.haveBeenUnioned(currentEncArea, debugOption2))
                {
                    neighbour = debugOption1;
                }                                                              //this is the last line of the debug that I should prolly alter.

                if (neighbour.isKnown() == false)
                {
                    if (neighbour.previousArea() == null)
                    {
                        neighbour.setPreviousArea(currentEncArea);
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }
    }

    private List<EnclosedArea> findKnownAreas(Room room)
    {
        spanningTree(room);

        List<EnclosedArea> knownAreas = new List<EnclosedArea>();
        for (int i = 0; i < MapGenScript.MAP_WIDTH; i++)
        {
            for (int j = 0; j < MapGenScript.MAP_HEIGHT; j++)
            {
                EnclosedArea currArea = MapGenScript.rooms[j, i].getEnclosedArea();
                if (currArea.isKnown() && !knownAreas.Contains(currArea))
                {
                    knownAreas.Add(currArea);
                }
            }
        }
        foreach (Room currRoom in MapGenScript.rooms)
        {
            currRoom.getEnclosedArea().clearVertexInfo(); //this wipes the vertex info for everything immediately
        }
        return knownAreas;
    }

    private void collectionOfDebugWhathaveyou()
    {
        int initX = MapGenScript.MAP_WIDTH / 2;
        int initY = MapGenScript.MAP_HEIGHT / 2;
        Debug.Log(areRoomsConnected(MapGenScript.rooms[initY, initX], MapGenScript.rooms[0, 0]) + " what");
        for (int currY = 0; currY < MapGenScript.MAP_HEIGHT; currY++)
        {
            for (int currX = 0; currX < MapGenScript.MAP_WIDTH; currX++)
            {
                //Debug.Log("is room[" + initY + ", " + initX + "] and room[" + currY + ", " + currX + "] conected:" + 
                //                  areRoomsConnected(rooms[initY, initX], rooms[currY, currX]));
                //Debug.Log("room[" + currY + ", " + currX + "] has enclosed Area: " + rooms[currY, currX].getEnclosedArea());
                //EnclosedArea EA = new EnclosedArea(rooms[currY, currX]);

                //rooms[currY, currX].setEnclosedArea(EA);
                //Debug.Log("room[" + currY + ", " + currX + "] has enclosed Area: " + rooms[currY, currX].getEnclosedArea());
                Debug.Log("plus, room at [" + currY + ", " + currX + "]: " + MapGenScript.rooms[currY, currX].getEnclosedArea().isKnown());
            }
        }
    }

    public static List<int> FindPath(Room start, Room end, Room[,] grid)
    {
        Dictionary<Room, Tuple<int, Room>> parents = new();
        bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        Queue<Tuple<int, Room>> queue = new();

        var startPair = new Tuple<int, Room>(0, start);
        queue.Enqueue(startPair);
        parents[start] = null;
        int test = 0;
        while (queue.Count > 0)
        {
            var pair = queue.Dequeue();
            var room = pair.Item2;
            visited[room.row(), room.col()] = true;

            if (room.Equals(end)) //for some reason, start and end are equal
            {
                return GetPath(parents, pair);
            }
            else
            {
                var neighbors = GetUnvisitedNeighbors(room, grid, visited);
                foreach (var neighbor in neighbors)
                {
                    test++;
                    var neighborRoom = neighbor.Item2;
                    if (!parents.ContainsKey(neighborRoom)) 
                    {
                        queue.Enqueue(neighbor);
                        parents[neighborRoom] = pair;
                    }
                }
            }
        }
        Debug.Log(test);
        return null;
    }

    private static List<int> GetPath(Dictionary<Room, Tuple<int, Room>> parents, Tuple<int, Room> destinationPair)
    {
        List<int> path = new() { destinationPair.Item1 };
        Room room = destinationPair.Item2;

        while (parents[room] != null && parents[room].Item1 != 0)
        {
            path.Insert(0, parents[room].Item1);
            room = parents[room].Item2;
        }

        return path;
    }

    private static List<Tuple<int, Room>> GetUnvisitedNeighbors(Room cell, Room[,] grid, bool[,] visited)
    {
        List<Tuple<int, Room>> neighbors = GetNeighbors(cell, grid);
        var unvisited = new List<Tuple<int, Room>>();

        foreach (var neighbor in neighbors)
        {
            var room = neighbor.Item2;
            if (!visited[room.row(), room.col()])
            {
                unvisited.Add(neighbor);
            }
        }
        return unvisited;
    }

    private static List<Tuple<int, Room>> GetNeighbors(Room room, Room[,] grid)
    {
        int row = room.row();
        int col = room.col();
        var neighbors = new List<Tuple<int, Room>>();

        if (row > 0)
            neighbors.Add(new Tuple<int, Room>((int)Room.Direction.UP, grid[row - 1, col]));

        if (row < grid.GetLength(0) - 1)
            neighbors.Add(new Tuple<int, Room>((int)Room.Direction.DOWN, grid[row + 1, col]));

        if (col > 0)
            neighbors.Add(new Tuple<int, Room>((int)Room.Direction.LEFT, grid[row, col - 1]));

        if (col < grid.GetLength(1) - 1)
            neighbors.Add(new Tuple<int, Room>((int)Room.Direction.RIGHT, grid[row, col + 1]));

        return neighbors;
    }

    public static List<Wall> ReplaceWallsAlongPath(List<int> path, Room start, Room[,] rooms)
    {
        List<Wall> walls = new List<Wall>();
        Room currRoom = start;
        if (path != null)
        {
            foreach (int direction in path)
            {
                if (currRoom.getWalls().ContainsKey(direction))
                {
                    walls.Add(currRoom.getWalls()[direction]);
                }
            }
        }
        return walls;
    }

}

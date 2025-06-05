using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFinder : MonoBehaviour
{

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
                return ShufflePath(GetPath(parents, pair));
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



    public static List<int> ShufflePath(List<int> path)
    {
        int n = path.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int value = path[k];
            path[k] = path[n];
            path[n] = value;
        }
        return path;
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
                if (currRoom.getWalls().ContainsKey(direction) && !areRoomsConnected(currRoom, rooms[0, MapGenScript.MAP_WIDTH / 2]))
                {
                    walls.Add(currRoom.getWalls()[direction]);
                }
                switch (direction)
                {
                    case 1:
                        currRoom = rooms[currRoom.row() - 1, currRoom.col()];
                        break;
                    case 2:
                        currRoom = rooms[currRoom.row(), currRoom.col() + 1];
                        break;
                    case 3:
                        currRoom = rooms[currRoom.row() + 1, currRoom.col()];
                        break;
                    case 4:
                        currRoom = rooms[currRoom.row(), currRoom.col() - 1];
                        break;

                }
            }
        }
        return walls;
    }

}

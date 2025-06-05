using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFinder : MonoBehaviour
{
    public static List<Boundary> getPathBetweenRooms(Room roomA, Room roomB)
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
    public static List<EnclosedArea> roomConnectedPath(Room roomA, Room roomB) //returns path from B to A (includes roomB and roomA)
    {
        List<EnclosedArea> pathBtoA = new List<EnclosedArea>();
        spanningTree(roomA);
        if (roomB.getEnclosedArea().isKnown()) //makes sure they actually have a path (otherwise returns an empty list)
        {
            for (EnclosedArea currEncArea = roomB.getEnclosedArea(); System.Object.Equals(currEncArea,null); currEncArea = currEncArea.previousArea())
            {
                pathBtoA.Add(currEncArea);
            }
        }
        return pathBtoA;
    }
    /*private List<EnclosedArea> allNonPathable()
    {

    }*/

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
                    if (System.Object.Equals(neighbour.previousArea(),null))
                    {
                        neighbour.setPreviousArea(currentEncArea);
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }
    }

    public static void allAccessAlgorithm(MapGenScript mapGenInstance) //makes comparatively sparse doors, also currently ignores starter room
    {
        allAccessAlgorithm(MapGenScript.rooms[MapGenScript.MAP_HEIGHT / 2, MapGenScript.MAP_WIDTH / 2], mapGenInstance);
    }

    //initRoom really doesn't matter, the public version will just call this with a preset room (like rooms[0,3] or something)\
    //this is funny though, I'm sorta using a pseudo-kruskal's algorithm here! (we've got both prim and kruskal!)
    private static void allAccessAlgorithm(Room initRoom, MapGenScript mapGenInstance) //mapGenInstance can be removed by making generateDoor static
    {
        //NOTE: for this method, a 'group' refers to a collection of EnclosedAreas that may traverse to one another
        //A group is basically a collection of EnclosedAreas the exact same way an EnclosedArea is a collection of Rooms, there is just no need to make a separate class
        //if two enclosed areas can traverse to eachother (via doors or whatever), they are definitionally part of the same group
        //Queue<(EnclosedArea, bool)> ungroupedAreaQueue = new Queue<(EnclosedArea, bool)>();
        Queue<EnclosedArea> ungroupedAreaQueue = new Queue<EnclosedArea>(); //all areas initially ungrouped and placed in queue
        List<List<EnclosedArea>> listOfGroupedAreas = new List<List<EnclosedArea>>(); //a group will be a list of enclosed areas, hence the list of lists here
        foreach (EnclosedArea enclosedArea in findAllAreas())
        {
            ungroupedAreaQueue.Enqueue(enclosedArea); //this just makes our queue with all enclosed areas
        }

        //the while loop will group all the enclosedAreas as defined above
        while(ungroupedAreaQueue.Count > 0)
        {
            EnclosedArea currUngroupedArea = ungroupedAreaQueue.Dequeue(); //currUngroupedArea is the current area that will be placed into a group
            bool alreadyGrouped = false; //using this bool is not the most optimised process (it would be better If I made a nested group class w/ an accesible bool field)
            foreach (List<EnclosedArea> group in listOfGroupedAreas)
            {
                foreach (EnclosedArea enclosedArea in group)
                {
                    if (EnclosedArea.haveBeenUnioned(enclosedArea, currUngroupedArea))
                    {
                        alreadyGrouped = true;
                        break;
                    }
                }
                if(alreadyGrouped) //for efficiency, so when it finds a match it quits the loops
                {
                    break;
                }
            }
            if(!alreadyGrouped) //if it's already in a group, it gets ignored
            {
                listOfGroupedAreas.Add(findKnownAreas(currUngroupedArea.getIdentityRoom()));
            }
        }
        int DEBUG_COUNTER = 0;
        //now that everything is grouped, it's time to run through individual walls to see if they connect two separate grouped areas!!!
        while (listOfGroupedAreas.Count > 1 && DEBUG_COUNTER <= MapGenScript.MAP_WIDTH * MapGenScript.MAP_HEIGHT) //if there is one group of areas then everything is accessible
        {
            DEBUG_COUNTER++;
            bool fastBreak = false;
            int arbGroupIndex = Random.Range(0, listOfGroupedAreas.Count);
            foreach(EnclosedArea enclosedArea in listOfGroupedAreas[arbGroupIndex])
            {
                foreach (Room room in enclosedArea.getRooms())
                {
                    List<Wall> roomWallList = room.getListOfWalls();
                    foreach(Wall wall in roomWallList)
                    {
                        if(!System.Object.Equals(wall.getRoom1(), null) && !System.Object.Equals(wall.getRoom2(), null))//Object.Equals() NEEDED, will not work w/==null
                        {
                            EnclosedArea encArea1 = wall.getRoom1().getEnclosedArea();
                            EnclosedArea encArea2 = wall.getRoom2().getEnclosedArea();
                            int areaGpIndex1 = -1;
                            int areaGpIndex2 = -1;
                            foreach(List<EnclosedArea> targetGroup in listOfGroupedAreas)
                            {
                                foreach(EnclosedArea targetEnclosedArea in targetGroup)
                                {
                                    if(areaGpIndex1 == -1 && EnclosedArea.haveBeenUnioned(encArea1,targetEnclosedArea))
                                    {
                                        areaGpIndex1 = listOfGroupedAreas.IndexOf(targetGroup); //since ILists are not custom, this IndexOf better work
                                    }
                                    if (areaGpIndex2 == -1 && EnclosedArea.haveBeenUnioned(encArea2, targetEnclosedArea))
                                    {
                                        areaGpIndex2 = listOfGroupedAreas.IndexOf(targetGroup); //since ILists are not custom, this IndexOf better work
                                    }
                                    if(areaGpIndex1 != -1 && areaGpIndex2 != -1) //when they both have found the gp they belong to,m ends loop
                                    {
                                        break;
                                    }
                                }
                                if (areaGpIndex1 != -1 && areaGpIndex2 != -1) //when they both have found the gp they belong to, ends loop
                                {
                                    break;
                                }
                            }

                            if(areaGpIndex1 != areaGpIndex2) //i.e. if they belong different 'groups'
                            {
                                mapGenInstance.generateDoor(wall.getPos(), wall); //create door between 'groups' ALSO THIS IS THE ONLY LINE MAPGENINSTANCE IS USED
                                foreach(EnclosedArea encAreaInSecondaryGroup in listOfGroupedAreas[areaGpIndex2])
                                {
                                    listOfGroupedAreas[areaGpIndex1].Add(encAreaInSecondaryGroup); //the groups are 'merged'
                                }
                                listOfGroupedAreas.RemoveAt(areaGpIndex2); //and once the first group has all the encAreas of the second, the second group is removed
                                fastBreak = true; //because the later for loops won't recognise "areaGpIndex1 != areaGpIndex2" to know to break
                                break;
                            }
                        }
                    }
                    if (fastBreak)
                    {
                        break;
                    }
                }
                if(fastBreak)
                {
                    break;
                }
            }
        }
    }

    private static List<EnclosedArea> findAllAreas() //note that the starter room is not in MapGenScript.rooms[,] so it and it's Enclosed area won't appear here
    {
        List<EnclosedArea> enclosedAreas = new List<EnclosedArea>();
        for (int i = 0; i < MapGenScript.MAP_WIDTH; i++)
        {
            for (int j = 0; j < MapGenScript.MAP_HEIGHT; j++)
            {
                EnclosedArea currArea = MapGenScript.rooms[j, i].getEnclosedArea();
                if (!enclosedAreas.Contains(currArea))
                {
                    enclosedAreas.Add(currArea);
                }
            }
        }
        foreach (Room currRoom in MapGenScript.rooms)
        {
            currRoom.getEnclosedArea().clearVertexInfo(); //this wipes the vertex info for everything immediately
        }
        return enclosedAreas;
    }
    private static List<EnclosedArea> findUnknownAreas(Room room)
    {
        spanningTree(room);

        List<EnclosedArea> unknownAreas = new List<EnclosedArea>();
        for (int i = 0; i < MapGenScript.MAP_WIDTH; i++)
        {
            for (int j = 0; j < MapGenScript.MAP_HEIGHT; j++)
            {
                EnclosedArea currArea = MapGenScript.rooms[j, i].getEnclosedArea();
                if (!currArea.isKnown() && !unknownAreas.Contains(currArea))
                {
                    unknownAreas.Add(currArea);
                }
            }
        }
        foreach (Room currRoom in MapGenScript.rooms)
        {
            currRoom.getEnclosedArea().clearVertexInfo(); //this wipes the vertex info for everything immediately
        }
        return unknownAreas;
    }

    private static List<EnclosedArea> findKnownAreas(Room room)
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

    public static void collectionOfDebugWhathaveyou()
    {
        Debug.Log("did it even finish?");
        List<EnclosedArea> knownAreas = findKnownAreas(MapGenScript.rooms[0, 0]);
        List<EnclosedArea> unknownAreas = findUnknownAreas(MapGenScript.rooms[0, 0]);
        List<List<EnclosedArea>> listOfList = new List<List<EnclosedArea>>();
        listOfList.Add(knownAreas); 
        listOfList.Add(unknownAreas);
        Debug.Log("index that SHOULD BE 0: " + listOfList.IndexOf(knownAreas));
        Debug.Log("index that SHOULD BE 1: " + listOfList.IndexOf(unknownAreas));
        List<Wall> roomWallList = MapGenScript.rooms[0,0].getListOfWalls();
        Debug.Log("room[0,0] number of walls" + roomWallList.Count);
    }

    public static List<int> FindPath(Room start, Room end, Room[,] grid)
    {
        Dictionary<Room, Tuple<int, Room>> parents = new();
        bool[,] visited = new bool[grid.GetLength(0), grid.GetLength(1)];
        Queue<Tuple<int, Room>> queue = new();

        var startPair = new Tuple<int, Room>(0, start);
        queue.Enqueue(startPair);
        parents[start] = null;
        while (queue.Count > 0)
        {
            var pair = queue.Dequeue();
            var room = pair.Item2;
            visited[(int)(room.getPos().y / Room.ROOM_UNIT), (int)(room.getPos().x / Room.ROOM_UNIT)] = true;

            if (room.Equals(end)) //for some reason, start and end are equal
            {
                return GetPath(parents, pair);
            }
            else
            {
                var neighbors = GetUnvisitedNeighbors(room, grid, visited);
                foreach (var neighbor in neighbors)
                {
                    var neighborRoom = neighbor.Item2;
                    if (!parents.ContainsKey(neighborRoom)) 
                    {
                        queue.Enqueue(neighbor);
                        parents[neighborRoom] = pair;
                    }
                }
            }
        }

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

    public static List<Wall> findWallsAlongPath(List<int> path, Room start, Room end, Room[,] rooms)
    {
        List<Wall> walls = new List<Wall>();
        return walls;
    }

}

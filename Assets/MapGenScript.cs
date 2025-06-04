using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using System;

public class MapGenScript : MonoBehaviour
{
    public GameObject doorPrefab;
    public GameObject roomPrefab;
    public GameObject wallPrefab;

    public static readonly int MAP_WIDTH = 7;
    public static readonly int MAP_HEIGHT = 7;

    public static readonly float MERGE_ODDS = 0.25f;

    public int[,] roomMap = new int[MAP_WIDTH,MAP_HEIGHT];

    public static Room[,] rooms = new Room[MAP_WIDTH,MAP_HEIGHT];

    public int seed = 0; //set to 0 to generate random seeds

    // dictionaries in C# are silly
    public static Dictionary<Vector2, Wall> wallMap = new Dictionary<Vector2, Wall>(); 
    public static Dictionary<Vector2, Door> doorMap = new Dictionary<Vector2, Door>();

    void Start()
    {
        genSeed();

        drawRooms();
        Room starterRoom = drawStarterRoom();

        generateWalls();
        generateIntitialDoors();
        generateDoors(starterRoom);

        //collectionOfDebugWhathaveyou();

        //printWallMap();
        List<int> path = PathFinder.FindPath(rooms[0, 0], rooms[2, 3], rooms); 

        foreach (int pathIndex in path)
        {
            Debug.Log(pathIndex);  
        }
    }

    //  Generates a list with the size map width by map height of random colors to make each room
    private Color32[] initializeColorArray()
    {
        Color32[] colorList = new Color32[MAP_WIDTH * MAP_HEIGHT];
        for (int i = 0; i < MAP_WIDTH * MAP_HEIGHT; i++)
        {
            byte r = (byte)Random.Range(0, 255);
            byte g = (byte)Random.Range(0, 255);
            byte b = (byte)Random.Range(0, 255);

            Color32 newColor = new Color32(r, g, b, 255);

            colorList[i] = newColor;
        }

        return colorList;
    }

    
    private void drawRooms()
    {
        // Makes "roomMap" array from 1 to the max size
        int roomNumber = 1;
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                roomMap[x, y] = roomNumber;
                roomNumber++;
            }
        }

        Color32[] colorList = initializeColorArray();

        // goes left to right bottom to top to see if rooms can merge and make connected large rooms
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                float down = Random.Range(0f, 1f);
                float right = Random.Range(0f, 1f);
                if (down < MERGE_ODDS && y != MAP_HEIGHT - 1)
                {
                    roomMap[y + 1, x] = roomMap[y, x];
                }
                if (right < MERGE_ODDS && x != MAP_WIDTH - 1)
                {
                    roomMap[y, x + 1] = roomMap[y, x];
                }

                drawRoom(colorList[roomMap[y, x] - 1], x, y);

            }
        }

        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {

                if (y != MAP_HEIGHT - 1 && roomMap[y + 1, x] == roomMap[y, x])
                {
                    rooms[y + 1, x].setRoomValue(roomMap[y, x]);
                    EnclosedArea.union(rooms[y + 1, x].getEnclosedArea(), rooms[y, x].getEnclosedArea());
                }
                if (x != MAP_WIDTH - 1 && roomMap[y, x + 1] == roomMap[y, x])
                {
                    rooms[y, x + 1].setRoomValue(roomMap[y, x]);
                    EnclosedArea.union(rooms[y, x + 1].getEnclosedArea(), rooms[y, x].getEnclosedArea());
                }
            }
        }
        print2DIntArray(roomMap);
    }

    public void print2DIntArray(int[,] arr)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < roomMap.GetLength(0); i++)
        {
            for (int j = 0; j < roomMap.GetLength(1); j++)
            {
                sb.Append(roomMap[j, i]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    // actually puts all the rooms on screen using the color list for the colors and the numbers from the roomMap to determine what colors they are
    private void drawRoom(Color32 col, int x, int y)
    {
        GameObject newRoom; //This assigns the clone to a new GameObject and then alters that. It makes it so any edits are going to the clone of the prefab not the prefab itself which is a lil more efficent i think
        Vector2 roomPos = new Vector2(x * Room.ROOM_UNIT, (MAP_HEIGHT - (y + 1)) * Room.ROOM_UNIT);
        newRoom = Instantiate(roomPrefab, roomPos, Quaternion.identity);

        rooms[y, x] = new Room(newRoom, roomPos, roomMap[y, x]);
        newRoom.name = roomMap[y, x].ToString();
        EnclosedArea enclosedArea = new EnclosedArea(rooms[y, x]);
        rooms[y, x].setEnclosedArea(enclosedArea); //creates an enclosed area for every room (they will be merged)

        SpriteRenderer sprite = rooms[y, x].getGameObject().GetComponent<SpriteRenderer>();
        sprite.color = col;
            
    }

    public Room drawStarterRoom()
    {
        GameObject newRoom;
        Vector2 roomPos = new Vector2((MAP_WIDTH / 2) * Room.ROOM_UNIT, (MAP_HEIGHT) * Room.ROOM_UNIT);

        newRoom = Instantiate(roomPrefab, roomPos, Quaternion.identity);
        newRoom.name = "Starter Room";

        Room starterRoom = new Room(newRoom, roomPos, 0);

        SpriteRenderer sprite = newRoom.GetComponent<SpriteRenderer>();
        sprite.color = Color.black;

        generateWall((MAP_WIDTH / 2) * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT + 1 - 0.5f), null, starterRoom,
            false, "Top Boundary at x = " + (MAP_WIDTH / 2).ToString(), true);

        generateWall((MAP_WIDTH / 2 - 0.5f) * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT), null, starterRoom,
            true, "Left Boundary at y = " + (MAP_HEIGHT).ToString(), true);

        generateWall((MAP_WIDTH / 2 + 1 - 0.5f) * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT), starterRoom, null,
            true, "Right Boundary at y = " + (MAP_HEIGHT).ToString(), true);

        return starterRoom;
    }

    private void generateWalls()
    {
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                // left boundary wall
                if (x == 0)
                {
                    generateWall(Room.ROOM_UNIT * -0.5f, y * Room.ROOM_UNIT, null, rooms[y, 0], 
                                 true, "Left Boundary at y = " + y.ToString(), false);
                } 

                // right boundary wall
                if (x == MAP_WIDTH - 1)
                {
                    generateWall(Room.ROOM_UNIT * (MAP_WIDTH -0.5f), y * Room.ROOM_UNIT, rooms[y, MAP_WIDTH - 1], null,
                                 true, "Right Boundary at y = " + y.ToString(), false);

                }
                // vertical walls between rooms
                else if (roomMap[y, x] != roomMap[y, x + 1])
                {
                    generateWall((x + 0.5f) * Room.ROOM_UNIT, (MAP_HEIGHT - y - 1) * Room.ROOM_UNIT, rooms[y, x], rooms[y, x + 1], 
                                 true, "Hori Wall at (" + x.ToString() + ", " + y.ToString() + ")", false);
                }

                // top boundary wall
                if (y == MAP_HEIGHT - 1)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT - 0.5f), null, rooms[MAP_HEIGHT - 1, x],
                                 false, "Top Boundary at x = " + x.ToString(), false);
                }
                // bottom boundary wall
                if (y == 0)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * -0.5f, rooms[0, x], null,
                                 false, "Bot Boundary at x = " + x.ToString(), false);
                } 
                // horizontal walls between rooms
                 else if (roomMap[y, x] != roomMap[y - 1, x])
                {
                    generateWall((x) * Room.ROOM_UNIT, ((MAP_HEIGHT - y - 1) + 0.5f) * Room.ROOM_UNIT, rooms[y, x], rooms[y - 1, x],
                                 false, "Vert Wall at (" + x.ToString() + ", " + y.ToString() + ")", false);
                }
            }
        }
    }

    /**
    * orientation is true for vertical, false for horizontal
    **/
    private void generateWall(float x, float y, Room room1, Room room2, bool orientation, string name, bool isStarterRoom)
    {
        Vector2 wallPos = new Vector2(x, y);

        GameObject newWall; // instead of continually editiing one prefab instead just make a clone in this loop and use that

        if (orientation)
        {
            newWall = Instantiate(wallPrefab, wallPos, Quaternion.identity);
        }
        else
        {
            newWall = Instantiate(wallPrefab, wallPos, Quaternion.AngleAxis(90, Vector3.forward));
        }

        newWall.name = name;
        Wall wall = new Wall(newWall, wallPos, room1, room2); //creates wall object

        if (!isStarterRoom)
        {
            wallMap[wallPos] = wall; // saving the newWall data into out dictionary so we can pick some from it later to delete
        }
    }

    // destroys the wall at the Vector2 position given to it
    public void destroyWall(Vector2 wallPos)  // this doesnt actually need the Vector2 wallPos defined in generateWall it just needs any cords in the form of a Vector2 !For example the cords of a soon to be door can be passed into here to remove the wall that would be blocking it!
    {
        // checks the dictionary at the pos passed in and grabs the wall gameobject that was stored there in generateWall to delete
        if (wallMap.TryGetValue(wallPos, out Wall wall))
        {
            Destroy(wall.getGameObject());
            wallMap.Remove(wallPos);
        }
    }

    public void generateIntitialDoors()
    {
        Vector2[] keyArray = wallMap.Keys.ToArray();
        for (int i = 0; i < keyArray.Length; i++)
        {
            Vector2 pos = keyArray[i];
            float makeDoor = Random.Range(0f, 1f);
            if (makeDoor < 0.2 
                && pos.x != -.5f * Room.ROOM_UNIT && pos.x != MAP_WIDTH * Room.ROOM_UNIT - 0.5f * Room.ROOM_UNIT
                && pos.y != -.5f * Room.ROOM_UNIT && pos.y != MAP_HEIGHT * Room.ROOM_UNIT - 0.5f * Room.ROOM_UNIT)
            {
                generateDoor(keyArray[i], wallMap[keyArray[i]]);
            }
        }

    }

    public void generateDoors(Room starterRoom)
    {
        /*
        foreach(Room room in rooms)
        {
            if (!areRoomsConnected(starterRoom, room)) 
            {

            }
        }*/
    }

    public void generateDoor(Vector2 wallPos, Wall wall)
    {
        destroyWall(wallPos);
        bool orientation = wall.getGameObject().transform.rotation.z == 0;
        GameObject newDoor; // instead of continually editiing one prefab instead just make a clone in this loop and use that

        if (orientation)
        {
            newDoor = Instantiate(doorPrefab, wallPos, Quaternion.identity);
        }
        else
        {
            newDoor = Instantiate(doorPrefab, wallPos, Quaternion.AngleAxis(90, Vector3.forward));
        }

        string name = "door";
        newDoor.name = name;

        Vector2 doorPos = new Vector2(wallPos.x, wallPos.y);
        Door door = new Door(newDoor, doorPos, wall.getRoom1(), wall.getRoom2());
        doorMap[doorPos] = door;
        wall.getRoom1().getEnclosedArea().addBoundary(door); 
        wall.getRoom2().getEnclosedArea().addBoundary(door); 
    }

    private void genSeed()
    {
        // !!!! makes sure to change seed back to 0 IN THE INSPECTOR for it to make random seeds again
        // preset seed
        if (seed != 0)
        {
            // set Random so it behaves in a certain way based on what the seed is
            Random.InitState(seed);
        }
        // generates new seed
        else
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
            Random.InitState(seed);

            Debug.Log("Seed Is: " + seed);
        }
    }

    



    private void printWallMap()
    {
        // thanks reddit
        foreach (var i in wallMap)
        {
            Debug.Log($"Key: {i.Key}, Value: {i.Value}"); //Cu is going to alter wallmap to be a disct<pos,Boundary>
        }
    }
}

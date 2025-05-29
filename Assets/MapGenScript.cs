using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public class MapGenScript : MonoBehaviour
{
    public GameObject doorPrefab;
    public GameObject roomPrefab;
    public GameObject wallPrefab;

    public static readonly int MAP_WIDTH = 7;
    public static readonly int MAP_HEIGHT = 7;

    public static readonly float MERGE_ODDS = 0.25f;

    public int[,] roomMap = new int[MAP_WIDTH,MAP_HEIGHT];

    public int seed = 0; //set to 0 to generate random seeds

    // dictionaries in C# are silly
    private Dictionary<Vector2, GameObject> wallMap = new Dictionary<Vector2, GameObject>();

    void Start()
    {
        genSeed();

        initializeRoomMap();

        Color32[] colorList = initializeColorArray();

        drawRoom(colorList);

        generateWalls();
        generateDoors();

        printWallMap();
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

    
    private void initializeRoomMap()
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

        // goes right to left top to bottom to see if rooms can merge and make connected large rooms
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                float right = Random.Range(0f, 1f);
                float down = Random.Range(0f, 1f);
                //Debug.Log(down);
                //Debug.Log(right);
                if (right < MERGE_ODDS && x != MAP_WIDTH - 1)
                {
                    roomMap[x + 1, y] = roomMap[x, y];
                }
                if (down < MERGE_ODDS && y != MAP_HEIGHT - 1)
                {
                    roomMap[x, y + 1] = roomMap[x, y];
                }
            }
        }
        print2DIntArray(roomMap);
    }

    public void print2DIntArray(int[,] arr)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < roomMap.GetLength(1); i++)
        {
            for (int j = 0; j < roomMap.GetLength(0); j++)
            {
                sb.Append(roomMap[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    // actually puts all the rooms on screen using the color list for the colors and the numbers from the roomMap to determine what colors they are
    private void drawRoom(Color32[] colorList)
    {
        for (int x = 0; x < MAP_HEIGHT ; x++)
        {
            for (int y = 0; y < MAP_WIDTH; y++)
            {
                GameObject newRoom; // just learned this is a thing you can do where you assign the clone to a new GameObject and then alter that. It makes it so any edits are going to the clone of the prefab not the prefab itself which is a lil more efficent i think
                Vector2 roomPos = new Vector2(x * Room.ROOM_UNIT, (MAP_HEIGHT - (y + 1)) * Room.ROOM_UNIT);

                newRoom = Instantiate(roomPrefab, roomPos, Quaternion.identity);

                newRoom.name = roomMap[x, y].ToString();

                SpriteRenderer sprite = newRoom.GetComponent<SpriteRenderer>();
                sprite.color = colorList[roomMap[x, y] - 1];

                Room room = new Room(newRoom, roomPos);
            }
        }
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
                    generateWall(Room.ROOM_UNIT * -0.5f, y * Room.ROOM_UNIT, true, "Left Boundary at y = " + y.ToString());
                } 

                // right boundary wall
                if (x == MAP_WIDTH - 1)
                {
                    generateWall(Room.ROOM_UNIT * (MAP_WIDTH -0.5f), y * Room.ROOM_UNIT, true, "Right Boundary at y = " + y.ToString());

                }
                // horizontal walls between rooms
                else if (roomMap[x, y] != roomMap[x + 1, y])
                {
                    generateWall((x + 0.5f) * Room.ROOM_UNIT, (MAP_HEIGHT - y - 1) * Room.ROOM_UNIT, true, "Hori Wall at (" + x.ToString() + ", " + y.ToString() + ")");
                }

                // top boundary wall
                if (y == MAP_HEIGHT - 1)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT - 0.5f), false, "Top Boundary at x = " + x.ToString());
                }
                // bottom boundary wall
                if (y == 0)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * -0.5f, false, "Bot Boundary at x = " + x.ToString());
                } 
                // vertical walls between rooms
                 else if (roomMap[x,y] != roomMap[x,y - 1])
                {
                    generateWall((x) * Room.ROOM_UNIT, ((MAP_HEIGHT - y - 1) + 0.5f) * Room.ROOM_UNIT, false, "Vert Wall at (" + x.ToString() + ", " + y.ToString() + ")");
                }
            }
        }
    }

    /**
    * orientation is true for vertical, false for horizontal
    **/
    private void generateWall(float x, float y, bool orientation, string name)
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

        wallMap[wallPos] = newWall; // saving the newWall data into out dictionary so we can pick some from it later to delete

        Wall wall = new Wall(newWall, wallPos);
    }

    // destroys the wall at the Vector2 position given to it
    public void destroyWall(Vector2 wallPos)  // this doesnt actually need the Vector2 wallPos defined in generateWall it just needs any cords in the form of a Vector2 !For example the cords of a soon to be door can be passed into here to remove the wall that would be blocking it!
    {
        // checks the dictionary at the pos passed in and grabs the wall gameobject that was stored there in generateWall to delete
        if (wallMap.TryGetValue(wallPos, out GameObject wall))
        {
            Destroy(wall);
            wallMap.Remove(wallPos);
        }
    }

    public void generateDoors()
    {
        Vector2[] keyArray = wallMap.Keys.ToArray();
        for (int i = 0; i < keyArray.Length; i++)
        {
            float makeDoor = Random.Range(0f, 1f);
            if (makeDoor < 0.2)
            {
                generateDoor(keyArray[i], wallMap[keyArray[i]]);
            }
        }

    }

    public void generateDoor(Vector2 wallPos, GameObject wall)
    {
        destroyWall(wallPos);
        Debug.Log("door made");
        bool orientation = wall.transform.rotation.z == 0;
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
        Door door = new Door(newDoor, doorPos);
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
            Debug.Log($"Key: {i.Key}, Value: {i.Value}");
        }
    }
}

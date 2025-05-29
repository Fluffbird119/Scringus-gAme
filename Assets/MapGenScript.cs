using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MapGenScript : MonoBehaviour
{
    public GameObject doorPrefab;
    public GameObject roomPrefab;
    public GameObject wallPrefab;

    public static readonly int MAP_WIDTH = 10;
    public static readonly int MAP_HEIGHT = 10;

    public static readonly float MERGE_ODDS = 0.25f;

    public int[,] roomMap = new int[MAP_WIDTH,MAP_HEIGHT];

    public int seed = 0; //set to 0 to generate random seeds

    void Start()
    {
        genSeed();

        initializeRoomMap();

        Color32[] colorList = initializeColorArray();

        drawRoom(colorList);

        generateWalls();
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
                Vector2 roomPos = new Vector2(x * Room.ROOM_UNIT, (MAP_HEIGHT - (y + 1)) * Room.ROOM_UNIT);

                roomPrefab.name = roomMap[x, y].ToString();

                SpriteRenderer sprite = roomPrefab.GetComponent<SpriteRenderer>();
                sprite.color = colorList[roomMap[x, y] - 1];

                Instantiate(roomPrefab, roomPos, Quaternion.identity);


                Room room = new Room(roomPrefab, roomPos);
            }
        }
    }

    private void generateWalls()
    {
        for (int x = 0; x < MAP_WIDTH; x++)
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                if (x == 0)
                {
                    generateWall(Room.ROOM_UNIT * -0.5f, y * Room.ROOM_UNIT, true);
                } 

                if (x == MAP_WIDTH - 1)
                {
                    generateWall(Room.ROOM_UNIT * (MAP_WIDTH -0.5f), y * Room.ROOM_UNIT, true);

                }
                else if (roomMap[x, y] != roomMap[x + 1, y])
                {
                    generateWall((x + 0.5f) * Room.ROOM_UNIT, (MAP_HEIGHT - y - 1) * Room.ROOM_UNIT, true);
                }

                if (y == MAP_HEIGHT - 1)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * (MAP_HEIGHT - 0.5f), false);
                }
                if (y == 0)
                {
                    generateWall(x * Room.ROOM_UNIT, Room.ROOM_UNIT * -0.5f, false);
                } 
                 else if (roomMap[x,y] != roomMap[x,y - 1])
                {
                    generateWall((x) * Room.ROOM_UNIT, ((MAP_HEIGHT - y - 1) + 0.5f) * Room.ROOM_UNIT, false);
                }
            }
        }
    }

    public void generateDoors()
    {

    }

    public void generateDoor()
    {

    }

    /**
     * orientation is true for vertical, false for horizontal
     **/
    private void generateWall(float x, float y, bool orientation)
    {
        Vector2 wallPos = new Vector2(x, y);

        if (orientation)
        {
            Instantiate(wallPrefab, wallPos, Quaternion.identity);
        }
        else
        {
            Instantiate(wallPrefab, wallPos, Quaternion.AngleAxis(90, Vector3.forward));

        }


        Wall wall = new Wall(wallPrefab, wallPos);
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
}

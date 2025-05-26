using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenScript : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject doorPrefab;

    public static readonly int mapWidth = 3;
    public static readonly int mapHeight = 3;

    public int[,] roomMap = new int[mapWidth,mapHeight];

    Color[] colorList = { Color.red, Color.blue, Color.yellow, Color.green, Color.black, Color.cyan, Color.magenta, Color.white, Color.grey };

    //List<RoomScript> room_list;

    void Start()
    {
        int roomNumber = 1;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                roomMap[x,y] = roomNumber;
                roomNumber++;
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float right = Random.Range(0f, 1f);
                float down = Random.Range(0f, 1f);
                Debug.Log(down);
                Debug.Log(right);
                if (right < 0.25 && x != mapWidth - 1)
                {
                    roomMap[x + 1, y] = roomMap[x, y];
                }
                if (down < 0.25 && y != mapHeight - 1)
                {
                    roomMap[x, y + 1] = roomMap[x, y];
                }
            }
        }
        for (int i = 0; i < roomMap.GetLength(0); i++)
        {
            for (int j = 0; j < roomMap.GetLength(1); j++)
            {
                Debug.Log(roomMap[i, j] + "\t");
            }
            Debug.Log("\n");
        }
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                SpriteRenderer sprite = roomPrefab.GetComponent<SpriteRenderer>();

                sprite.color = colorList[roomMap[x,y] - 1];
                
                Vector2 roomPos = new Vector2(x * Room.ROOM_UNIT, y * Room.ROOM_UNIT);

                Instantiate(roomPrefab, roomPos, Quaternion.identity);

                //-Room room = new Room(roomPrefab, roomPos);
            }
        }
        




    }

    void Update()
    {
        
    }
}

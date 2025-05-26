using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGen : MonoBehaviour
{
    public GameObject roomPrefab;

    public Color newcolor;
    private SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        for (int x = 0; x < 2; x++)
        {
            SpriteRenderer sprite = roomPrefab.GetComponent<SpriteRenderer>();

            if (x == 0)
            {
                newcolor = new Color(100, 100, 100, 100);

                sprite.color = newcolor;
            }
            else if (x == 1)
            {
                newcolor = new Color(250, 7, 74, 100);

                sprite.color = newcolor;
            }

            Vector2 roomPos = new Vector2(x * Room.ROOM_UNIT, Room.ROOM_UNIT);

            Instantiate(roomPrefab, roomPos, Quaternion.identity);
        }
    }
}

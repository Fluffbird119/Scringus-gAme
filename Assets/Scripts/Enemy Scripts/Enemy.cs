using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float roomWidth;
    private float roomHeight;

    private Renderer rend;

    public Enemy(GameObject prefab, Vector2 pos) : base()
    {
        // rend = prefab.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy Pathfinding and actions?
        // Detection?
    }
}

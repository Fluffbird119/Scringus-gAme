using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Entity : Object
{
    // Start is called before the first frame update
    private GameObject gameObject;

    //all entities should have game objects with componenets: Transform, Sprite Renderer
    public Entity(GameObject prefab)
    {
        this.gameObject = prefab;
    }



    //public void placeAt



    

}

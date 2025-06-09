using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : Object
{

    private string name;
    private string description;
    public Stat(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public string getName()
    {
        return name;
    }
    public string getDescription()
    {
        return description;
    }
    
}

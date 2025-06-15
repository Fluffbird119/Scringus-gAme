using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{


    

    //[Cu] created this just so hotbar would stop yelling at him
    //[Header("Primary Player Stats")]
    [SerializeField] private int str;
    [SerializeField] private int dex;
    [SerializeField] private int con;
    [SerializeField] private int intel; //cause int is reserved
    [SerializeField] private int wis;

    [Header("Events")]
    [SerializeField] public GameEvent onPlayerStrChanged;

    public PlayerStats(int str, int dex, int con, int intel, int wis)
    {
        this.str = str;
        this.dex = dex;
        this.con = con;
        this.intel = intel;
        this.wis = wis;
    }


    public int getStr() { return str; }
    public int getDex() { return dex; }
    public int getCon() { return con; }
    public int getIntel() { return intel; }
    public int getWis() { return wis; }

    private int increaseStr(int amount)
    {
        this.str += amount;
        onPlayerStrChanged.Raise(this, this.str);
        return this.str;
    }

    //[Cu] hasn't added in secondary stats or anything else because he's a lazy wanker

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //[Cu] created this just so hotbar would stop yelling at him
    private int str = 0;
    private int dex = 0;
    private int con = 0;
    private int intel = 0; //cause int is reserved
    private int wis = 0;

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

    //[Cu] hasn't added in secondary stats or anything else because he's a lazy wanker

}

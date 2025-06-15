using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is technically summary, but since most the class isn't built yet, this will serve as an atttemtped blueprint or something
///     EDIT: this is also a brainstorming thing for how the class neede to work. In doing so I have decided to change the secondary categories
/// 
/// The playerStats class (or something else) ultimately needs to handle various groups of stats that will continuously update.
///     This cannot really be static given the fact that multiple players will be existing and this should only concern a single player
/// 
/// The class will be frequently called for both updating stats and more frequently for accessing them.
/// 
///                 
///     The three categories for Primary are as follows:
///         1. Player: basically permanent. Should never decrease and should be set infrequently but will be accessed very frequently
///         2. Weapon: May need to change whenever a player: changes the equipped weapon, gains/drops an unequipped weapon, or hits the requirements for a wpn
///         3. Status: Exclusively conditional/temporary. Will almost always be handled by GameEvent calls (but many things will ignore them)
///                     Note that Overall Primary (just called Primary) is as follows: Player + Weapon + Status
///     The categories for secondary are as follows:
///         1. Factored Secondary: the secondary stats determined from a formula of ALL cartegoies of certain primary stats
///                             (these formulas may need to be called very often, but they should be really easy to execute [if not, this can be altered]).
///                             Note that factored secondary will never be saved as a variable in player stats because every time it is called it will need to
///                             be recalculated.
///         2. Flat Secondary: flat and added to Factored Secondary (actually stored!)
///         3. Scalar Secondary: These linearly scale the result from the factored secondary (for now, not the flat values)
///                 The secondaries are as follows: Factored * Scalar + Flat = Overall Secondary
/// 
///     
///     This means there will be 6 overall categories as the following (some like Weapon primary and Status primary may never actually be used):
///         1. Player Primary
///         2. Weapon Primary       //(this is honestly present to account for weapon passives allowing for scalings, otherwise weapons could be status)
///         3. Status Primary       //ideally, this does not exist
///         4. Factored Secondary (factors in ALL primary stats)
///         5. Flat Secondary
///         6. Status Secondary
/// 
/// More In-depth are the following explanations of how each one gets accessed and operates
///     Primary stats: {STR, DEX, CON, INT/intel, WIS}
///     
///         Player Primary Stats:
///             What should be setting them:
///                 1. Base player stats & level ups
///                         Both of these are from the player (hence easy communication) and need access infrequently
/// 
///                 2. Primary Stat Growths (from weapons)
///                         These are permanent, even if the weapons that contributed to this are discarded
///                         
///                 3. Miscellaneous exceptions (like a shrine room in the map or something)
///             
///             What should be receiving from them (and hence must be listers for changes):
///                 1. Basically anything that factors in stats.
///                 2. EDIT: This would be secondary stats, if their values are ever stored. 
///                 3. Weapon passive reqs, hotbar slot # at least are intended to exclusively call this
///                 4. Anything that reports the stats in any menus or UI in general (this is the reason this document is being made)
///         
///         Weapon Primary Stats:
///             What should be setting them: literally just weapon passives that happen to influence primary stats
///             What should be a receiving from them (and hence needs to be a listener for each change):
///                 1. Anything that reports weapon primary stats in any menus or UI in general (this is the reason this document is being made)
///                 2. EDIT: Again, secondary being by formula means this is irrelevant
///         
///         
///         Secondary Stats [Cu]: has overwritten these to some degree.
///                 
///             
///                 
/// 
/// 
/// In order for reasonable access and optimization, there are 2 types of stats, but 3 stat handling categories for primary.
///     This may seem convoluted and stupid, so [Cu] has included the reasons for the 3 stat handling categories is as follows (also to prove to himself): 
///         1. Many things need to know about 'Player stats' and have to be informed every time they change.
///                 'Status stats' can (and will) alter very frequently (e.g. any stack based ability or anything which 'decays' (which [1] alr plans to use)) 
///                 Having to call everything that needs to know about stats and tell it to incorporate the new stats potentially multiple times per frame is taxing
///         2. Separating into categories prevents certain goofy behaviour, especially handling weapon passive requirement checks.
///                 Say, if the requirement check was based on status or weapon stats, a player may achieve the conditions via a status but then permanently
///                 receive their benefits because the bonuses from the weapon passives may very well help satisfy its own requirements.
///         3. Guaranteeing that player stats never decrease (except in deliberately introduced exceptions) makes certain features that don't want to decrease
///                 (like the hotbar slot #) much more optimised and less clunky.
///         4. From a debugging/balancing standpoint, it's nice to be able to see these separated, and from the player's standpoint, it means we can report
///                 the separated info (if we choose to report it)
///                     
/// [1] is the satiohitnarbegiower document (otherwise called the player document)
/// 
/// </summary>

public class PlayerStats : MonoBehaviour
{

    //*Important* Unity is getting a little ornery with how much data is present as a SerializedField for PlayerStats
    //  because of this, the fields may 'not show up' in the unity script editor
    //  when this happens, they are still present, but you must do the following:
    //      click the upper right three dots on the Component, and then hit the 'properties...' option (and viola! you'll be able to see and edit them)


    //[Cu] created this just so hotbar would stop yelling at him
    //[Header("Primary Player Stats")]
    [SerializeField] private int str;                               //
    [SerializeField] private int dex;                               // individual characters should handle their base primary stats
    [SerializeField] private int con;                               // this will then incorporate whatever base character is chosen
    [SerializeField] private int intel; //since int is reserved     // but for now, the initial stats are set manually for easy debugging
    [SerializeField] private int wis;                               // 

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Some notes about the Unit: 
 * This is the generic unit that a player will control with player-made scripts. This means that this script will
 * handle all movement, targetting, and overall the AI of the unit being used. Some things will be controlled
 * directly by a player, such as deciding what coordinate to move to, however this script will have the code that
 * decides how it pathfinds there without harming itself so the player won't have to do it themselves.
 * 
 * This concept of hiding each of the functions behind this script means we can eventually make a Block Code system
 * that the player can use to make AI programs with very little hassle, Scratch-style.
 * 
 * TODO:
 * Decide how we want to store tiles. First know that this game should be top down and 2D like Rimworld, where the
 * only digging needed is into walls of mountains instead of underground like Towns. As such each tile will be a
 * square on a very large map grid that the player should be able to hover over to get the exact tile, or later on
 * with the block code be able to select a tile in game while coding a script (like a color picker but for tiles)
 * and automatically write the code targetting it for them (for instance if you want to unload to a chest at a
 * specific tile you could click the Tile requirement in the parameter for the drop off function and it will bring
 * you back to the game world (likely paused) and if you click a tile it will go back to the code blocks with that
 * tile selected as the parameter automatically).
 * 
 * Tiles should be as simple as possible, perhaps make each Tile NOT store where it is located, as each will be exactly
 * 1 unit away from eachother (meaning 0,0 to 0,1 is effectively moving up 1 Unity unit) but instead a Unit will make
 * a call downwards with a raycast or similar to detect information about the Tile it is above to decide things like
 * what is growing on it, what kind of ground it is, etc. The actual Unit does not need to do this raycast, it can
 * also be done by a "magic floating raycast" to check if it is a wall or something first before the Unit attempts to
 * travel to it. As such the player need not store the current Tile it is above except to make it easier to perform tasks
 * to it without needing to rescan/reraycast.
 */

public class Unit : MonoBehaviour
{
    // Name of the current unit (May want to get from editor)
    public string unitName;

    // The group this unit belongs to
    public UnitGroup group;

    // Tile currently stood on
    public Tile currentTile;

    // Inventory of items in Unit
    public List<Item> inventory;
    
    // Boundary points for Unit
    public int boundaryMinX = 1000;
    public int boundaryMaxX = -1000;
    public int boundaryMinY = 1000;
    public int boundaryMaxY = -1000;

    NavMeshAgent agent;


    // Unit functions to be used by the player
    // Moves unit to given Tile
    public void moveTo(Tile newTile) {
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            Debug.Log("Made it");
            agent.destination = newTile.transform.position;
        }
    }

    // Moves unit to given Vector3, TO BE REMOVED
    public void moveTo(Vector3 newTilePos)
    {
        agent.destination = newTilePos;
    }

    // Attempts to gather items on current Tile
    // Returns true if successfully gathered, else false
    public bool gather()
    {
        return false;
    }
    // Overloaded functions that specify amount or the item to pick up
    // Automatically takes from container if standing on one
    public bool gather(int amount)
    {
        return false;
    }
    public bool gather(Item item)
    {
        return false;
    }
    public bool gather(Item item, int amount)
    {
        return false;
    }

    // Attempts to drop the a given Item onto currentTile
    // Returns true if successfully dropped, else false
    // Automatically places items into container if standing on one
    public bool drop(Item item)
    {
        return false;
    }
    public bool drop(Item item, int amount)
    {
        return false;
    }

    // Attempts to find a Tile containing a given item
    // Returns the Tile if one is found, else returns currentTile
    // Try to find the closest of this item in the Unit's boundary
    public Tile find(Item item)
    {
        return currentTile;
    }

    public void setBoundary(int xMin, int xMax, int yMin, int yMax)
    {
        boundaryMinX = xMin;
        boundaryMaxX = xMax;
        boundaryMinY = yMin;
        boundaryMaxY = yMax;
    }

    // Unit functions to be used automatically by the game (Things like pathfinding and helpers)



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

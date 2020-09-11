using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private Grid<PathNode> grid;
    private int x;
    private int y;

    public bool isWalkable;
    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode previousNode;

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}

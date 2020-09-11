using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;


    public Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    

    public Pathfinder(int width, int height, Tilemap tilemap, Vector3Int originPosition)
    {
        grid = new Grid<PathNode>(width, height, 1f, originPosition, (Grid<PathNode> grid,int x, int y) => new PathNode(grid,x,y));
        foreach(PathNode path in grid.gridArray)
        {
            if (tilemap.HasTile(new Vector3Int(path.X, path.Y,0)))
            {
                    path.isWalkable = false;
            }
        }

    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetValue(startX, startY);
        PathNode endNode = grid.GetValue(endX, endY);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                PathNode pathNode = grid.GetValue(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }

            }

        }

        //Out of node in the openList
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode){
        List<PathNode> neighbourList = new List<PathNode>();

        if(currentNode.X - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            //Left Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            //Left Up
            if (currentNode.Y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
        }
        if(currentNode.X + 1 < grid.width)
        {
            //Right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
            //Right Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            //Right Up
            if (currentNode.Y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
        }

        // Down
        if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
        //Up
        if (currentNode.Y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        return neighbourList;

    }

    private PathNode GetNode(int x, int y)
    {
        return grid.GetValue(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}

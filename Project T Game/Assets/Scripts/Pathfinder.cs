using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 5;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinder Instance { get; private set; }
    public Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;



    public Pathfinder(int width, int height, Tilemap tilemap, Vector3 originPosition)
    {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 1f, originPosition, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
        foreach (PathNode pNode in Pathfinder.Instance.grid.gridArray)
        {
            Pathfinder.Instance.grid.GetXY(new Vector3(pNode.X, pNode.Y), out int x, out int y);
            if (tilemap.HasTile(tilemap.WorldToCell(new Vector3(x, y, 0))))
            {
                pNode.isWalkable = false;
            }
        }

    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition,bool useAetoile = true)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY, useAetoile);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.X, pathNode.Y) * grid.cellSize + Vector3.one * grid.cellSize * .5f);
            }
            return vectorPath;
        }

    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY, bool useAetoile)
    {
        if (useAetoile)
        {
            return AEtoilePathFinding(startX, startY, endX, endY);
        }
        else
        {
            return findShortestPath(startX, startY, endX, endY);
        }
    }

    private List<PathNode> AEtoilePathFinding(int startX, int startY, int endX, int endY)
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
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
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

    public PathNode DjikstraPathFinding(int startX, int startY, int endX, int endY)
    {
        // Nodes that are unexplored
        List<PathNode> unexplored = new List<PathNode>();

        // We add all the nodes we found into unexplored.
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                PathNode pathNode = grid.GetValue(x, y);
                pathNode.weight = int.MaxValue;
                pathNode.previousNode = null;
            }
        }

        // Set the starting node weight to 0;
        PathNode startNode = grid.GetValue(new Vector3(startX,startY));
        startNode.weight = 0;

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.weight.CompareTo(y.weight));

            // Get the lowest weight in unexplored.
            PathNode current = unexplored[0];

            // Note: This is used for games, as we just want to reduce compuation, better way will be implementing A*
            /*
            // If we reach the end node, we will stop.
            if(current == end)
            {   
                return end;
            }*/

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            PathNode currentNode = current;
            List<PathNode> neighbours = GetNeighbourList(currentNode);
            foreach (PathNode neighNode in neighbours)
            {
                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && neighNode.isWalkable)
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(new Vector3(neighNode.X, neighNode.Y,0), new Vector3(current.X, current.Y, 0));
                    distance = currentNode.weight + distance;

                    // If the added distance is less than the current weight.
                    if (distance < neighNode.weight)
                    {
                        // We update the new distance as weight and update the new path now.
                        neighNode.weight = distance;
                        neighNode.previousNode = current;
                    }
                }
            }

        }

        return grid.GetValue(endX,endY);
    }

    /// <summary>
    /// Finding the shortest path and return in a List
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>A List of transform for the shortest path</returns>
    public List<PathNode> findShortestPath(int startX, int startY, int endX, int endY)
    {

        List<PathNode> result = new List<PathNode>();
        PathNode node = DjikstraPathFinding(startX, startY, endX, endY);

        // While there's still previous node, we will continue.
        while (node != null)
        {
            result.Add(node);
            PathNode currentNode = node;
            node = currentNode.previousNode;
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.X - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            //Left Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            //Left Up
            if (currentNode.Y + 1 < grid.height) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
        }
        if (currentNode.X + 1 < grid.width)
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
        List<PathNode> path = new List<PathNode>
        {
            endNode
        };
        PathNode currentNode = endNode;
        while (currentNode.previousNode != null)
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
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}

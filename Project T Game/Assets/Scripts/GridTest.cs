using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTest : MonoBehaviour
{
    private Grid<PathNode> grid;
    public int x, y;
    public Transform player;
    Pathfinder pathfinding;
    public Tilemap tilemap;
    public Vector3Int originPosition;
    void Start()
    {
        pathfinding = new Pathfinder(x, y, tilemap, originPosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pathfinding.grid.GetXY(vec, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(Vector3Int.CeilToInt(player.position).x, Vector3Int.CeilToInt(player.position).y, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count-1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].X, path[i].Y), new Vector3(path[i + 1].X, path[i + 1].Y), Color.red, 5000f);
                }
            }
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }

}
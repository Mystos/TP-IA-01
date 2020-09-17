using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTest : MonoBehaviour
{
    public int x, y;
    public Transform player;
    public Tilemap tilemap;
    public Vector3 originPosition;
    void Start()
    {
        new Pathfinder(x, y, tilemap, originPosition);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Pathfinder.Instance.grid.GetXY(vec, out int x, out int y);
            List<Vector3> path = Pathfinder.Instance.FindPath(Vector3Int.FloorToInt(player.position), new Vector3(x,y,0), false);
            if (path != null)
            {
                for (int i = 0; i < path.Count-1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.green, 100f);
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
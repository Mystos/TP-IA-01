using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.WSA;

public class WorldInitialization : MonoBehaviour
{
    public int x, y;
    public Tilemap tilemap;
    public Transform player;
    public Vector3 originPosition;
    public Tile tile;



    // Start is called before the first frame update
    void Start()
    {
        new Pathfinder(x, y, tilemap, originPosition);

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Pathfinder.Instance.grid.GetXY(vec, out int x, out int y);
    //        Pathfinder.Instance.grid.GetXY(player.position, out int xx, out int yy);
    //        List<Vector3> path = Pathfinder.Instance.FindPath(new Vector3(xx, yy, 0), new Vector3(x, y, 0));
    //        if (path != null)
    //        {
    //            for (int i = 0; i < path.Count - 1; i++)
    //            {
    //                Debug.DrawLine(path[i], path[i + 1], Color.green, 100f);
    //            }
    //        }
    //        //Pathfinder.Instance.grid.GetXY(vec, out int xx, out int yy);

    //        //tilemap.SetTile(tilemap.WorldToCell(new Vector3(xx, yy,0)), tile);

    //        //foreach (PathNode pNode in Pathfinder.Instance.grid.gridArray)
    //        //{
    //        //    Pathfinder.Instance.grid.GetXY(new Vector3(pNode.X, pNode.Y), out int xxx, out int yyy);
    //        //    if(tilemap.HasTile(tilemap.WorldToCell(new Vector3(xxx, yyy, 0)))){
    //        //        tilemap.SetTile(tilemap.WorldToCell(new Vector3(xxx, yyy, 0)), tile);
    //        //    }
    //        //}
    //    }
    //}

    //public static Vector3 GetMouseWorldPosition()
    //{
    //    Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    vec.z = 0f;
    //    return vec;
    //}
}

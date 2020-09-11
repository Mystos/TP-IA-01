using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid<PathNode> 
{
    public int width { get; }
    public int height { get; }
    private float cellSize;
    public  Vector3 originPosition { get; }
    public PathNode[,] gridArray;
    private TextMesh[,] debugTextArray;


    public Grid(int width, int height, float cellSize, Vector3Int originPosition, Func<Grid<PathNode>, int, int, PathNode> createPathNode)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new PathNode[width, height];
        debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridArray.GetLength(0); ++x)
        {
            for (int y = 0; y < gridArray.GetLength(1); ++y)
            {
                gridArray[x, y] = createPathNode(this, x, y); ;

                //debugTextArray[x, y] = CreateWorldText(gridArray[x, y].ToString(), GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.black, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return originPosition + new Vector3(x, y); // * cellSize;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
    }

    public void SetValue(int x, int y, PathNode value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, PathNode value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public PathNode GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(PathNode);
        }
    }

  
    public PathNode GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public static TextMesh CreateWorldText(string text, Vector3 localPosition = default(Vector3), int fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 0)
    {
        if (color == Color.clear) color = Color.white;
        return CreateWorldText(null, text, localPosition, fontSize, color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }
}
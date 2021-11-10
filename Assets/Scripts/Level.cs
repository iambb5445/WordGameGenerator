using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeighboringLayer
{
    Dictionary<Cell, List<Cell>> neighbors = new Dictionary<Cell, List<Cell>>();
    public bool containsTransition(Cell startCell, Cell endCell)
    {
        if (neighbors.ContainsKey(startCell))
        {
            return neighbors[startCell].Contains(endCell);
        }
        return false;
    }
    public Cell[] getNeighbors(Cell cell)
    {
        if (neighbors.ContainsKey(cell))
        {
            return neighbors[cell].ToArray();
        }
        return new Cell[]{ };
    }
    public void addRelation(Cell startCell, Cell endCell)
    {
        if (!neighbors.ContainsKey(startCell))
        {
            neighbors[startCell] = new List<Cell>();
        }
        neighbors[startCell].Add(endCell);
    }
}

public abstract class Level
{
    protected List<Cell> cells = new List<Cell>();
    protected List<NeighboringLayer> neighboringLayers = new List<NeighboringLayer>();
    public Cell[] getCells()
    {
        return cells.ToArray();
    }
    public abstract bool isTransitionAllowed(Cell startCell, Cell endCell);
}

public class GridLevel: Level
{
    private static Vector2Int up = new Vector2Int(0, -1);
    private static Vector2Int down = new Vector2Int(0, 1);
    private static Vector2Int left = new Vector2Int(-1, 0);
    private static Vector2Int right = new Vector2Int(1, 0);
    public static List<List<Vector2Int>> eightDirectionsMovement = new List<List<Vector2Int>>()
    {
        new List<Vector2Int>(){up, down, left, right, up+left, up+right, down+left, down+right}
    };
    public static List<List<Vector2Int>> fourDirectionsMovement = new List<List<Vector2Int>>()
    {
        new List<Vector2Int>(){up, down, left, right}
    };
    public static List<List<Vector2Int>> fourDirectionsNoChangeMovement = new List<List<Vector2Int>>()
    {
        new List<Vector2Int>(){up},
        new List<Vector2Int>(){down},
        new List<Vector2Int>(){left},
        new List<Vector2Int>(){right}
    };
    private int xSize, ySize;
    private List<List<Vector2Int>> moveset;
    public GridLevel(int xSize, int ySize, List<List<Vector2Int>> moveset)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        this.moveset = moveset;
        setCells();
        setRelations();
    }
    private void setCells()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                char randChar = (char)('A' + Random.Range(0, 26));
                this.cells.Add(new Cell(randChar.ToString(), new Vector2((i-2)*1.5f, (j-2)*1.5f)));
            }
        }
    }
    private void setRelations()
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                foreach (List<Vector2Int> moveLayer in moveset)
                {
                    NeighboringLayer neighboringLayer = new NeighboringLayer();
                    foreach (Vector2Int move in moveLayer)
                    {
                        Cell startCell = this.getCell(i, j);
                        Cell endCell = this.getCell(i + move.x, j + move.y);
                        if (startCell != null && endCell != null)
                        {
                            neighboringLayer.addRelation(startCell, endCell);
                        }
                    }
                    neighboringLayers.Add(neighboringLayer);
                }
            }
        }
    }
    private Cell getCell(int x, int y)
    {
        if (x < 0 || x >= xSize || y < 0 || y >= ySize)
        {
            return null;
        }
        int index = x * ySize + y;
        return cells[index];
    }
    public override bool isTransitionAllowed(Cell startCell, Cell endCell)
    {
        if (startCell == null)
        {
            return true;
        }
        foreach (NeighboringLayer neighboringLayer in neighboringLayers)
        {
            if (neighboringLayer.containsTransition(startCell, endCell))
            {
                return true;
            }
        }
        return false;
    }
}

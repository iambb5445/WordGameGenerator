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
    protected static float cellSize = 1.5f;
    protected List<Cell> cells = new List<Cell>();
    protected List<NeighboringLayer> neighboringLayers = new List<NeighboringLayer>();
    protected List<string> goals;
    public virtual void initiate(List<string> goals)
    {
        this.goals = new List<string>();
        List<List<string>> goalParts = getParts(goals);
        for(int index = 0; index < goals.Count; index++)
        {
            if (putWithBacktrack(goalParts[index]))
            {
                this.goals.Add(goals[index]);
            }
        }
    }
    protected void fillEmptyCells()
    {
        foreach (Cell cell in cells)
        {
            if (cell.getText() == null)
            {
                char randomChar = (char)('A' + Random.Range(0, 26));
                cell.setText(randomChar.ToString());
            }
        }
    }
    private bool putWithBacktrack(List<string> wordParts, List<Cell> answer=null, NeighboringLayer neighboringLayer=null)
    {
        if (answer == null)
        {
            answer = new List<Cell>();
        }
        if (wordParts.Count == answer.Count)
        {
            return true;
        }
        if (answer.Count == 0)
        {
            List<int> neighboringLayerOrder = getRandomOrder(neighboringLayers.Count);
            foreach(int neighboringLayerIndex in neighboringLayerOrder)
            {
                List<int> cellOrder = getRandomOrder(cells.Count);
                foreach (int cellIndex in cellOrder)
                {
                    if (tryBacktrackPath(wordParts, answer, neighboringLayers[neighboringLayerIndex], cells[cellIndex]))
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            Cell lastCell = answer[answer.Count - 1];
            Cell[] neighbors = neighboringLayer.getNeighbors(lastCell);
            List<int> neighborOrder = getRandomOrder(neighbors.Length);
            foreach (int neighborIndex in neighborOrder)
            {
                if (tryBacktrackPath(wordParts, answer, neighboringLayer, neighbors[neighborIndex]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool tryBacktrackPath(List<string> wordParts, List<Cell> answer, NeighboringLayer neighboringLayer, Cell cell)
    {
        if (answer.Contains(cell)) // no repeat
        {
            return false;
        }
        string wordPart = wordParts[answer.Count];
        if (cell.getText() == null)
        {
            cell.setText(wordPart);
            answer.Add(cell);
            if (putWithBacktrack(wordParts, answer, neighboringLayer))
            {
                return true;
            }
            answer.RemoveAt(answer.Count - 1);
            cell.setText(null);
        }
        else if (cell.getText() == wordPart)
        {
            answer.Add(cell);
            if (putWithBacktrack(wordParts, answer, neighboringLayer))
            {
                return true;
            }
            answer.RemoveAt(answer.Count - 1);
        }
        return false;
    }

    private List<int> getRandomOrder(int size)
    {
        List<int> orderedIndices = new List<int>();
        for(int i = 0; i < size; i++)
        {
            orderedIndices.Add(i);
        }
        List<int> shuffledIndices = new List<int>();
        for(int i = 0; i < size; i++)
        {
            int nextIndex = Random.Range(0, orderedIndices.Count);
            shuffledIndices.Add(orderedIndices[nextIndex]);
            orderedIndices.RemoveAt(nextIndex);
        }
        return shuffledIndices;
    }
    private Cell getRandomCell()
    {
        int index = Random.Range(0, cells.Count);
        return cells[index];
    }
    protected List<List<string>> getParts(List<string> words)
    {
        List<List<string>> parts = new List<List<string>>();
        foreach (string word in words)
        {
            List<string> wordParts = new List<string>();
            foreach (char character in word)
            {
                wordParts.Add(character.ToString());
            }
            parts.Add(wordParts);
        }
        return parts;
    }
    public Cell[] getCells()
    {
        return cells.ToArray();
    }
    public bool isTransitionAllowed(List<Cell> selected, Cell newCell)
    {
        foreach (NeighboringLayer neighboringLayer in neighboringLayers)
        {
            bool isLayerSuitable = true;
            for(int i = 0; i < selected.Count - 1; i++)
            {
                if (!neighboringLayer.containsTransition(selected[i], selected[i + 1]))
                {
                    isLayerSuitable = false;
                }
            }
            if (isLayerSuitable)
            {
                Cell lastCell = selected.Count > 0? selected[selected.Count - 1] : null;
                if(lastCell == null || neighboringLayer.containsTransition(lastCell, newCell))
                {
                    return true;
                }
            }
        }
        return selected.Count == 0;
    }
}

public class GridLevel: Level
{
    private static Vector2Int up = new Vector2Int(0, 1);
    private static Vector2Int down = new Vector2Int(0, -1);
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
                this.cells.Add(new Cell(null, new Vector2((i-2)*cellSize, (j-2)*cellSize)));
            }
        }
    }
    private void setRelations()
    {
        foreach (List<Vector2Int> moveLayer in moveset)
        {
            NeighboringLayer neighboringLayer = new NeighboringLayer();
            foreach (Vector2Int move in moveLayer)
            {
                for (int i = 0; i < xSize; i++)
                {
                    for (int j = 0; j < ySize; j++)
                    {
                        Cell startCell = this.getCell(i, j);
                        Cell endCell = this.getCell(i + move.x, j + move.y);
                        if (startCell != null && endCell != null)
                        {
                            neighboringLayer.addRelation(startCell, endCell);
                        }
                    }
                }
            }
            neighboringLayers.Add(neighboringLayer);
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
}

public class CircleLevel: Level
{
    private int cellCount;
    public CircleLevel(int cellCount)
    {
        this.cellCount = cellCount;
        setCells();
        setRelations();
    }
    private void setCells()
    {
        for (int i = 0; i < cellCount; i++)
        {
            float degreeDiff = 360f / cellCount;
            float degreeDiffRadian = (2 * Mathf.PI) / cellCount;
            float cellDiameter = cellSize * Mathf.Sqrt(2);
            float radius = cellDiameter / Mathf.Cos(degreeDiffRadian / 2);
            Vector2 position = Quaternion.Euler(0, 0, i * degreeDiff) * (Vector2.up * radius);
            this.cells.Add(new Cell(null, position));
        }
    }
    private void setRelations()
    {
        NeighboringLayer neighboringLayer = new NeighboringLayer();
        foreach (Cell startCell in cells)
        {
            foreach (Cell endCell in cells)
            {
                if (startCell != endCell)
                {
                    neighboringLayer.addRelation(startCell, endCell);
                }
            }
        }
        neighboringLayers.Add(neighboringLayer);
    }
}
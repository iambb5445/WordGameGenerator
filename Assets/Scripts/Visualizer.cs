using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour {
    [SerializeField]
    GameObject cellPrefab;
    [SerializeField]
    Text hintText;
    private Level level;
    private List<CellVisualizer> cellVisualizers;
    private List<CellVisualizer> selectionSequence = new List<CellVisualizer>();
    public void setLevel(Level level)
    {
        this.level = level;
        cellVisualizers = new List<CellVisualizer>();
        Cell[] cells = level.getCells();
        foreach (Cell cell in cells)
        {
            GameObject cellObject = Instantiate(cellPrefab);
            CellVisualizer cellVisualizer = cellObject.GetComponentInChildren<CellVisualizer>();
            cellVisualizer.setCell(cell);
            cellVisualizers.Add(cellVisualizer);
        }
        setHint(level.getGoals());
    }
    private void setHint(List<string> goals)
    {
        hintText.text = "";
        for(int goalIndex = 0; goalIndex < goals.Count; goalIndex++)
        {
            hintText.text += goals[goalIndex];
            if (goalIndex != goals.Count - 1)
            {
                hintText.text += "\n";
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            checkSelection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            finalizeWord();
        }
    }
    private void checkSelection()
    {
        CellVisualizer selectedCellVisualizer = getMousePointed();
        if (selectedCellVisualizer == null)
        {
            return;
        }
        int index = selectionSequence.IndexOf(selectedCellVisualizer);
        if (index >= 0) // exists in sequence
        {
            if (index == (selectionSequence.Count - 2))
            {
                selectionSequence[index + 1].hideVisualization();
                selectionSequence.RemoveAt(index + 1);
            }
        }
        else
        {
            List<Cell> selectionSequenceCells = getCells(selectionSequence);
            if (level.isTransitionAllowed(selectionSequenceCells, selectedCellVisualizer.getCell()))
            {
                selectedCellVisualizer.showSelection();
                selectionSequence.Add(selectedCellVisualizer);
            }
        }
    }
    private static List<Cell> getCells(List<CellVisualizer> cellVisualizers)
    {
        List<Cell> cells = new List<Cell>();
        foreach(CellVisualizer cellVisualizer in cellVisualizers)
        {
            cells.Add(cellVisualizer.getCell());
        }
        return cells;
    }
    private CellVisualizer getMousePointed()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(new Vector2(worldPosition.x, worldPosition.y));
        foreach (CellVisualizer cellVisualizer in cellVisualizers)
        {
            if (target == cellVisualizer.getCollider())
            {
                return cellVisualizer;
            }
        }
        return null;
    }
    private void finalizeWord()
    {
        string word = "";
        foreach (CellVisualizer cellVisualizer in selectionSequence)
        {
            word += cellVisualizer.getText();
            cellVisualizer.hideVisualization();
        }
        selectionSequence.Clear();
        Debug.Log(word);
    }
}

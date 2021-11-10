using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {
    [SerializeField]
    GameObject cellPrefab;
    private Level level;
    private List<CellVisualizer> cellVisualizers;
    private List<CellVisualizer> selectionSequence = new List<CellVisualizer>();
    void Start()
    {
        //  TODO remove this
        GridLevel gridLevel = new GridLevel(4, 5, GridLevel.eightDirectionsMovement);
        setLevel(gridLevel);
    }
    void setLevel(Level level)
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
            Cell lastCell = selectionSequence.Count == 0? null
                : selectionSequence[selectionSequence.Count - 1].getCell();
            if (level.isTransitionAllowed(lastCell, selectedCellVisualizer.getCell()))
            {
                selectedCellVisualizer.showSelection();
                selectionSequence.Add(selectedCellVisualizer);
            }
        }
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

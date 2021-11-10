using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    string text;
    Vector2 position;
    CellVisualizer cellVisualizer;
    public Cell(string text, Vector2 position)
    {
        this.text = text;
        this.position = position;
    }
    public Vector2 getPosition()
    {
        return position;
    }
    public string getText()
    {
        return text;
    }
    public void setCellVisualizer(CellVisualizer cellVisualizer)
    {
        this.cellVisualizer = cellVisualizer;
    }
    public CellVisualizer GetCellVisualizer()
    {
        return cellVisualizer;
    }
}
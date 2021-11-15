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
    private void updateVisualizer()
    {
        if (this.cellVisualizer != null)
        {
            cellVisualizer.setCell(this);
        }
    }
    public Vector2 getPosition()
    {
        return position;
    }
    public string getText()
    {
        return text;
    }
    public void setText(string text)
    {
        this.text = text;
        updateVisualizer();
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
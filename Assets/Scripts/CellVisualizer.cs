using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour {
    Collider2D inputCollider;
    SpriteRenderer spriteRenderer;
    Cell cell;
    void Start()
    {
        inputCollider = gameObject.GetComponentInChildren<Collider2D>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    private void setText(string text)
    {
         gameObject.GetComponentInChildren<TextMesh>().text = text;
    }
    public Collider2D getCollider()
    {
        return inputCollider;
    }
    public void showSelection()
    {
        spriteRenderer.color = Color.yellow;
    }
    public void hideVisualization()
    {
        spriteRenderer.color = Color.white;
    }
    public void setCell(Cell cell)
    {
        this.cell = cell;
        cell.setCellVisualizer(this);
        setText(cell.getText());
        transform.position = cell.getPosition();
    }
    public string getText()
    {
        return cell.getText();
    }
    public Cell getCell()
    {
        return cell;
    }
}

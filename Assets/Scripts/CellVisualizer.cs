using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualizer : MonoBehaviour {
    char letter;
    Collider2D inputCollider;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        inputCollider = gameObject.GetComponentInChildren<Collider2D>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
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
}

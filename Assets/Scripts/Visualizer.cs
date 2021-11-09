using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {
    [SerializeField]
    GameObject cell;
    private List<CellVisualizer> cellVisualizers;
    void Start()
    {
        //  TODO remove this
        setLevel();
    }
    void setLevel()
    {
        cellVisualizers = new List<CellVisualizer>();
        // TODO
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject newCell = Instantiate(cell);
                newCell.transform.position = new Vector3((i-2)*1.5f, (j-2)*1.5f);
                cellVisualizers.Add(newCell.GetComponentInChildren<CellVisualizer>());
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D target = Physics2D.OverlapPoint(new Vector2(worldPosition.x, worldPosition.y));
            foreach (CellVisualizer cellVisualizer in cellVisualizers)
            {
                if (target == cellVisualizer.getCollider())
                {
                    cellVisualizer.showSelection();
                }
                else
                {
                    cellVisualizer.hideVisualization();
                }

            }
        }
    }
}

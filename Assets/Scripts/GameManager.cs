using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    Visualizer visualizer;
    public void Start()
    {
        GenerateLevel();
    }
    public void GenerateLevel()
    {
        visualizer = FindObjectOfType<Visualizer>();
        GameDesigner gameDesigner = new GameDesigner(); // TODO
        Level level = new GridLevel(3, 4, GridLevel.eightDirectionsMovement);
        gameDesigner.design("cat", level);
        visualizer.setLevel(level);
    }
}

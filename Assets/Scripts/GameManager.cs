using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    static GameManager instanceReference;
    LevelVisualizer levelVisualizer;
    List<string> goals;
    List<bool> goalGuessed;
    LevelData levelData;
    public static GameManager getInstance()
    {
        return instanceReference;
    }
    public LevelData getLevelData()
    {
        return levelData;
    }
    public void Start()
    {
        if (instanceReference == null)
        {
            instanceReference = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void loadLevelScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void generateLevelInfo()
    {
        levelData = LevelDataInputManager.getInstance().getLevelData();
        levelData.level.initiate(levelData.api.getCandidateWords(levelData.theme, 5));
        setGoals();
    }
    public void setGoals()
    {
        this.goals = levelData.level.getGoals();
        goalGuessed = new List<bool>();
        foreach(string goal in goals)
        {
            goalGuessed.Add(false);
        }
    }
    public void checkWord(string word)
    {
        int index = goals.IndexOf(word);
        if (index >= 0)
        {
            if (goalGuessed[index] == false)
            {
                goalGuessed[index] = true;
                checkVictory();
            }
        }
    }
    public void checkVictory()
    {
        foreach(bool condition in goalGuessed)
        {
            if (!condition)
            {
                return;
            }
        }
        Debug.Log("victory!");
    }
}

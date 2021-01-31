using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData
{
    public int width, depth, levelnum;
    public float timeUntilEnemy;

    public LevelData(int n, int w, int d, float t)
    {
        levelnum = n;
        width = w;
        depth = d;
        timeUntilEnemy = t;
    }
}

public class GameManager : MonoBehaviour
{
    private int level;
    private Maze maze;

    private LevelData[] levels;
    
    void Start()
    {
        levels = new LevelData[]
        {
            new LevelData(0, 3, 20, 3.5f),
            new LevelData(1, 5, 10, 6),
            new LevelData(2, 7, 15, 30),
            new LevelData(3, 15, 15, 60),
            new LevelData(4, 20, 20, 120)
        };

        level = 0;

        maze = GameObject.Find("Maze").GetComponent<Maze>();
        maze.CreateMaze(levels[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        level++;
        if (level < levels.Length)
        {
            Debug.Log("Loading level " + level);
            maze.GetComponent<Maze>().textDisplay.text = "Teleporting to next level...";
            maze.CreateMaze(levels[level]);
        } else
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int width, depth, levelnum;
    public bool hasEnemy;

    public LevelData(int n, int w, int d, bool en)
    {
        levelnum = n;
        width = w;
        depth = d;
        hasEnemy = en;
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
            new LevelData(0, 3, 20, false),
            new LevelData(1, 5, 10, false),
            new LevelData(2, 7, 15, true),
            new LevelData(3, 15, 15, true),
            new LevelData(4, 20, 20, true)
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
            Debug.Log("Loading Level " + level);
            maze.CreateMaze(levels[level]);
        } else
        {
            //YOU WIN!!
        }
    }
}

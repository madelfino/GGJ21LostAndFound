using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int width, depth;
    public bool hasEnemy;

    public LevelData(int w, int d, bool en)
    {
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
            new LevelData(3, 20, false),
            new LevelData(5, 10, false),
            new LevelData(7, 15, true),
            new LevelData(15, 15, true),
            new LevelData(20, 20, true)
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

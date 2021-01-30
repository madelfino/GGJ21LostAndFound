using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int level;
    private Maze maze;

    private Vector2[] levelSizes;

    // Start is called before the first frame update
    void Start()
    {
        levelSizes = new Vector2[] {
            new Vector2(10,10),
            new Vector2(15,10),
            new Vector2(15,20),
            new Vector2(30,30),
        };

        level = 0;

        maze = GameObject.Find("Maze").GetComponent<Maze>();
        maze.CreateMaze(levelSizes[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        level++;
        maze.CreateMaze(levelSizes[level % levelSizes.Length]);
    }
}

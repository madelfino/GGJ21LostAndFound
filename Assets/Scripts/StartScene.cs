using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private Maze maze;

    void Start()
    {
        maze = gameObject.GetComponent<Maze>();
        LevelData start = new LevelData(-1, 20, 20, -1);
        maze.CreateMaze(start);
    }
}

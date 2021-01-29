using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSMaze : Maze
{
    int startX;
    int startZ;

    public override void Generate()
    {
        startX = Random.Range(1, width-1);
        startZ = Random.Range(1, depth-1);
        Generate(startX, startZ);
    }

    void Generate(int x, int z)
    {
        Debug.Log(CountSquareNeighbors(x, z));
        if (CountSquareNeighbors(x, z) >= 2) return;
        map[x, z] = 0;

        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z);
        Generate(x + directions[1].x, z + directions[1].z);
        Generate(x + directions[2].x, z + directions[2].z);
        Generate(x + directions[3].x, z + directions[3].z);
    }

    public override void CreatePlayer()
    {
        Instantiate(player, new Vector3(startX * scale, 0.5f * scale, startZ * scale), Quaternion.identity);
    }
}

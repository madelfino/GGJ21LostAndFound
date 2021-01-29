using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSMaze : Maze
{
    int startX;
    int startZ;
    int longestPathLength;

    MapLocation goalLocation;

    public override void Generate()
    {
        startX = Random.Range(1, width-1);
        startZ = 1; //Random.Range(1, depth-1);
        goalLocation = new MapLocation(startX, startZ);
        longestPathLength = 0;
        Generate(startX, startZ, 0);
    }

    void Generate(int x, int z, int pathLength)
    {
        if (CountSquareNeighbors(x, z) >= 2) return;
        map[x, z] = 0;

        if (pathLength > longestPathLength)
        {
            longestPathLength = pathLength;
            goalLocation.x = x;
            goalLocation.z = z;
        }

        directions.Shuffle();

        Generate(x + directions[0].x, z + directions[0].z, pathLength + 1);
        Generate(x + directions[1].x, z + directions[1].z, pathLength + 1);
        Generate(x + directions[2].x, z + directions[2].z, pathLength + 1);
        Generate(x + directions[3].x, z + directions[3].z, pathLength + 1);
    }

    public override void CreatePlayer()
    {
        Instantiate(player, new Vector3(startX * scale, 0.5f * scale, startZ * scale), Quaternion.identity);
    }

    public override void CreateGoal()
    {
        Instantiate(goal, new Vector3(goalLocation.x * scale, 0, goalLocation.z * scale), Quaternion.identity);
    }
}

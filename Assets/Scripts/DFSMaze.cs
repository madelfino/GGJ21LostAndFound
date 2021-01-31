using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSMaze : Maze
{
    int longestPathLength;

    MapLocation startLocation;
    MapLocation goalLocation;
    MapLocation enemyLocation;

    public override void Generate()
    {
        startLocation = new MapLocation(Random.Range(1, width - 1), 1);
        goalLocation = new MapLocation(startLocation.x, startLocation.z);
        enemyLocation = new MapLocation(startLocation.x, startLocation.z);
        longestPathLength = 0;
        Generate(startLocation.x, startLocation.z, 0);
    }

    void Generate(int x, int z, int pathLength)
    {
        if (CountSquareNeighbors(x, z) >= 2) return;
        map[x, z] = 0;

        if (pathLength > longestPathLength)
        {
            enemyLocation.x = goalLocation.x;
            enemyLocation.z = goalLocation.z;
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
        Instantiate(player, new Vector3(startLocation.x * scale, height + 0.5f * scale, startLocation.z * scale), Quaternion.identity);
        Instantiate(startPad, new Vector3(startLocation.x * scale, height, startLocation.z * scale), Quaternion.identity);
    }

    public override void CreateEnemy()
    {
        GameObject enemyInst = Instantiate(enemy, new Vector3(enemyLocation.x * scale, height + 0.5f, enemyLocation.z * scale), Quaternion.identity);
        enemyInst.GetComponent<Xenotaur>().MoveToLocation(new Vector3(startLocation.x * scale, height + 0.5f * scale, startLocation.z * scale));
    }

    public override void CreateGoal()
    {
        Instantiate(goal, new Vector3(goalLocation.x * scale, height, goalLocation.z * scale), Quaternion.identity);
    }
}

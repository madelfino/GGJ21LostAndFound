using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
}

public class Maze : MonoBehaviour
{

    public List<MapLocation> directions = new List<MapLocation>()
    {
        new MapLocation(1, 0),
        new MapLocation(0, 1),
        new MapLocation(-1, 0),
        new MapLocation(0, -1)
    };

    public int width = 30;
    public int depth = 30;
    public int scale = 3;
    public byte[,] map;

    public GameObject straight;
    public GameObject corner;
    public GameObject tjunction;
    public GameObject crossroads;
    public GameObject deadend;

    public GameObject player;
    public GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        map = new byte[width, depth];
        InitializeMap();
        Generate();
        DrawMap();
        CreatePlayer();
        CreateGoal();
    }

    public virtual void CreatePlayer()
    {
        int x = Random.Range(1, width - 1);
        int z = Random.Range(1, depth - 1);
        while (map[x, z] != 0)
        {
            x = Random.Range(1, width - 1);
            z = Random.Range(1, depth - 1);
        }
        Instantiate(player, new Vector3(x * scale, 0.5f * scale, z * scale), Quaternion.identity);
    }

    public virtual void CreateGoal()
    {
        int x = Random.Range(1, width - 1);
        int z = Random.Range(1, depth - 1);
        while (map[x, z] != 0)
        {
            x = Random.Range(1, width - 1);
            z = Random.Range(1, depth - 1);
        }
        Instantiate(goal, new Vector3(x * scale, 0, z * scale), Quaternion.identity);
    }

    void InitializeMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
            }
        }
    }

    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0f, 1f) > 0.5)
                {
                    map[x, z] = 0;
                }

            }
        }
    }

    void DrawMap()
    {
        for (int z = 1; z < depth-1; z++)
        {
            for (int x = 1; x < width-1; x++)
            {
                Vector3 pos = new Vector3(x * scale, 0, z * scale);
                if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 1, -1, 0, -1}))
                {
                    Instantiate(straight, pos, Quaternion.identity);
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 0, -1, 1, -1 }))
                {
                    Instantiate(straight, pos, Quaternion.Euler(0, 90, 0));
                }
                //CROSSROADS
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 0, -1, 0, -1 }))
                {
                    Instantiate(crossroads, pos, Quaternion.identity);
                }
                //CORNERS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 0, -1, 0, -1 }))
                {
                    Instantiate(corner, pos, Quaternion.Euler(0, 90, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 1, -1, 0, -1 }))
                {
                    Instantiate(corner, pos, Quaternion.Euler(0, 180, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 1, -1, 1, -1 }))
                {
                    Instantiate(corner, pos, Quaternion.Euler(0, -90, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 0, -1, 1, -1 }))
                {
                    Instantiate(corner, pos, Quaternion.identity);
                }
                //T-JUNCTIONS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 0, -1, 0, -1 }))
                {
                    Instantiate(tjunction, pos, Quaternion.identity);
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 0, -1, 0, -1 }))
                {
                    Instantiate(tjunction, pos, Quaternion.Euler(0, -90, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 1, -1, 0, -1 }))
                {
                    Instantiate(tjunction, pos, Quaternion.Euler(0, 90, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 0, -1, 1, -1 }))
                {
                    Instantiate(tjunction, pos, Quaternion.Euler(0, 180, 0));
                }
                //DEAD-ENDS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 1, -1, 0, -1 }))
                {
                    Instantiate(deadend, pos, Quaternion.Euler(0, -90, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 0, -1, 1, -1 }))
                {
                    Instantiate(deadend, pos, Quaternion.Euler(0, 180, 0));
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 1, -1, 1, -1 }))
                {
                    Instantiate(deadend, pos, Quaternion.identity);
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 1, -1, 1, -1 }))
                {
                    Instantiate(deadend, pos, Quaternion.Euler(0, 90, 0));
                }
            }
        }
    }

    bool Search2D(int c, int r, int[] pattern)
    {
        int pos = 0;
        for (int z = 1; z >= -1; z--)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (pattern[pos] != -1 && pattern[pos] != map[c + x, r + z]) return false;
                pos++;
            }
        }
        return true;
    }

    public int CountSquareNeighbors(int x, int z)
    {
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        return 4 - (map[x - 1, z] + map[x + 1, z] + map[x, z - 1] + map[x, z + 1]);
    }

    public int CountDiagonalNeighbors(int x, int z)
    {
        int count = 0;

        return count;
    }
}

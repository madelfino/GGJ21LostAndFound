using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

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
    public int height = 0;
    public int scale = 3;
    public byte[,] map;
    public float enemyTimer = 600;
    private bool xeno = true;

    public GameObject straight;
    public GameObject corner;
    public GameObject tjunction;
    public GameObject crossroads;
    public GameObject deadend;

    public GameObject player;
    public GameObject goal;
    public GameObject enemy;
    public GameObject startPad;

    private NavMeshSurface[,] navMeshSurfaces;

    public TextMeshProUGUI textDisplay;
    
    public void DestroyMaze()
    {
        GameObject[] mazePieces = GameObject.FindGameObjectsWithTag("MazePiece");
        foreach( GameObject piece in mazePieces )
        {
            piece.GetComponent<NavMeshSurface>().RemoveData();
            GameObject.Destroy(piece);
        }
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Player"));
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Goal"));
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Respawn"));
    }

    public void CreateMaze(LevelData level)
    {
        textDisplay.text = "Teleporting to next level...";
        DestroyMaze();
        width = level.width;
        depth = level.depth;
        height = level.levelnum * 10;
        map = new byte[width, depth];
        enemyTimer = level.timeUntilEnemy;
        xeno = false;
        navMeshSurfaces = new NavMeshSurface[width, depth];
        InitializeMap();
        Generate();
        DrawMap();
        GenerateNavMesh();
        CreatePlayer();
        CreateGoal();
        textDisplay.text = "";
    }

    public void Update()
    {
        enemyTimer -= Time.deltaTime;
        if (!xeno)
        {
            textDisplay.text = "" + (int)(enemyTimer + 1);
            if (enemyTimer < 0)
            {
                xeno = true;
                CreateEnemy();
                textDisplay.text = "The Xenotaur is coming";
            }
        }
        
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
        Instantiate(player, new Vector3(x * scale, height + 0.5f * scale, z * scale), Quaternion.identity);
        Instantiate(startPad, new Vector3(x * scale, height + 0, z * scale), Quaternion.identity);
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
        Instantiate(goal, new Vector3(x * scale, height + 0, z * scale), Quaternion.identity);
    }

    public virtual void CreateEnemy()
    {
        int x = Random.Range(1, width - 1);
        int z = Random.Range(1, depth - 1);
        while (map[x, z] != 0)
        {
            x = Random.Range(1, width - 1);
            z = Random.Range(1, depth - 1);
        }
        Instantiate(enemy, new Vector3(x * scale, height + 0.5f * scale, z * scale), Quaternion.identity);
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

    public void GenerateNavMesh()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 0)
                {
                    navMeshSurfaces[x, z].BuildNavMesh();
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
                Vector3 pos = new Vector3(x * scale, height, z * scale);
                if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 1, -1, 0, -1}))
                {
                    GameObject piece = Instantiate(straight, pos, Quaternion.identity);
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 0, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(straight, pos, Quaternion.Euler(0, 90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                //CROSSROADS
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 0, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(crossroads, pos, Quaternion.identity);
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                //CORNERS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 0, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(corner, pos, Quaternion.Euler(0, 90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 1, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(corner, pos, Quaternion.Euler(0, 180, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 1, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(corner, pos, Quaternion.Euler(0, -90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 0, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(corner, pos, Quaternion.identity);
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                //T-JUNCTIONS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 0, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(tjunction, pos, Quaternion.identity);
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 0, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(tjunction, pos, Quaternion.Euler(0, -90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 1, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(tjunction, pos, Quaternion.Euler(0, 90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 0, 0, 0, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(tjunction, pos, Quaternion.Euler(0, 180, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                //DEAD-ENDS
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 1, -1, 0, -1 }))
                {
                    GameObject piece = Instantiate(deadend, pos, Quaternion.Euler(0, -90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 1, 0, 0, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(deadend, pos, Quaternion.Euler(0, 180, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 1, -1, 0, 0, 1, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(deadend, pos, Quaternion.identity);
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
                }
                else if (Search2D(x, z, new int[] { -1, 0, -1, 1, 0, 1, -1, 1, -1 }))
                {
                    GameObject piece = Instantiate(deadend, pos, Quaternion.Euler(0, 90, 0));
                    navMeshSurfaces[x, z] = piece.GetComponent<NavMeshSurface>();
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

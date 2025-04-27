using UnityEngine;
using System.Linq;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    
    public bool positionPlayerAtStart = true;

    [Range(0, 100)]
    public float randomWallRemovalPercentage = 0;

    public float tileSize = 1.0f;

    private int[,] maze;

    private void Start()
    {
        GenerateMaze();
        DrawMaze();
        AdjustCamera();

        if (positionPlayerAtStart)
        {
            PositionPlayerAtStart();
        }
    }

    private void GenerateMaze()
    {
        maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }

        CarvePassages(1, 1);
        maze[0, 1] = 0;
        RemoveRandomWalls();
    }

    private void CarvePassages(int x, int y)
    {
        maze[x, y] = 0;

        int[] directions = { 0, 1, 2, 3 };
        System.Random rng = new();
        directions = directions.OrderBy(d => rng.Next()).ToArray();

        foreach (int direction in directions)
        {
            int nx = x, ny = y;

            switch (direction)
            {
                case 0: ny -= 2; break;
                case 1: nx += 2; break;
                case 2: ny += 2; break;
                case 3: nx -= 2; break;
            }

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                maze[(x + nx) / 2, (y + ny) / 2] = 0;
                CarvePassages(nx, ny);
            }
        }
    }

    private void RemoveRandomWalls()
    {
        System.Random rng = new();
        var wallPositions = new System.Collections.Generic.List<(int x, int y)>();
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 1 && HasTwoOppositeNeighbors(x, y))
                {
                    wallPositions.Add((x, y));
                }
            }
        }

        wallPositions = wallPositions.OrderBy(_ => rng.Next()).ToList();
        int wallsToRemove = Mathf.RoundToInt(wallPositions.Count * (randomWallRemovalPercentage / 100f));

        for (int i = 0; i < wallsToRemove; i++)
        {
            var (x, y) = wallPositions[i];
            maze[x, y] = 0;
        }
    }

    private bool HasTwoOppositeNeighbors(int x, int y)
    {
        bool horizontal = maze[x - 1, y] == 0 && maze[x + 1, y] == 0;
        bool vertical = maze[x, y - 1] == 0 && maze[x, y + 1] == 0;
        return horizontal || vertical;
    }

    private void DrawMaze()
    {
        Vector3 offset = new(-width * tileSize / 2f + tileSize / 2f, 0, -height * tileSize / 2f + tileSize / 2f);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(x * tileSize, tileSize / 2f, y * tileSize) + offset;
                    wall.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
                    wall.transform.parent = transform;
                }
            }
        }
    }

    private void PositionPlayerAtStart()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            Vector3 offset = new(-width * tileSize / 2f + tileSize / 2f, 0, -height * tileSize / 2f + tileSize / 2f);
            player.transform.position = new Vector3((width - 2) * tileSize, player.transform.position.y, (height - 2) * tileSize) + offset;
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' component not found.");
        }
    }

    public void RegenerateLabyrinth()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GenerateMaze();
        DrawMaze();
        AdjustCamera();

        if (positionPlayerAtStart)
        {
            PositionPlayerAtStart();
        }
    }

    private void AdjustCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float mazeWidth = (width + 2) * tileSize;
            float mazeHeight = (height + 2) * tileSize;

            float aspectRatio = (float)Screen.width / Screen.height;
            if (aspectRatio >= 1)
            {
                mainCamera.orthographicSize = mazeHeight / 2f;
            }
            else
            {
                mainCamera.orthographicSize = mazeWidth / (2f * aspectRatio);
            }

            mainCamera.transform.position = new Vector3(0, Mathf.Max(mazeWidth, mazeHeight) / 2f, 0);
            mainCamera.orthographic = true;
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Cannot adjust camera to fit the labyrinth.");
        }
    }
}
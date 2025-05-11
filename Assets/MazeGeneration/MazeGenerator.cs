using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public int width = 11;
    public int height = 11;
    public GameObject wallPrefab;
    public float cellSize = 1.0f;
    public float removeWallPercentage = 0.1f;

    private int[,] maze;
    private GameObject lastGeneratedMaze;

    public void GenerateMaze()
    {
        maze = new int[width, height];
        InitializeMaze();

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(1, 1);
        maze[currentCell.x, currentCell.y] = 0;
        stack.Push(currentCell);

        while (stack.Count > 0)
        {
            currentCell = stack.Peek();
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell);

            if (neighbors.Count > 0)
            {
                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWall(currentCell, chosenNeighbor);
                maze[chosenNeighbor.x, chosenNeighbor.y] = 0;
                stack.Push(chosenNeighbor);
            }
            else
            {
                stack.Pop();
            }
        }

        RemoveRandomWalls();
        InstantiateMaze();
    }

    private void InitializeMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1;
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(0, 2),
            new Vector2Int(0, -2),
            new Vector2Int(2, 0),
            new Vector2Int(-2, 0)
        };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = cell + dir;
            if (neighbor.x > 0 && neighbor.x < width - 1 && neighbor.y > 0 && neighbor.y < height - 1 && maze[neighbor.x, neighbor.y] == 1)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wall = current + (neighbor - current) / 2;
        maze[wall.x, wall.y] = 0;
    }

    private void RemoveRandomWalls()
    {
        List<Vector2Int> removableWalls = new List<Vector2Int>();

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 1 && CountWallNeighbors(x, y) == 2 && !IsCornerWall(x, y))
                {
                    removableWalls.Add(new Vector2Int(x, y));
                }
            }
        }

        int wallsToRemove = Mathf.FloorToInt(removableWalls.Count * removeWallPercentage);
        for (int i = 0; i < wallsToRemove; i++)
        {
            int randomIndex = Random.Range(0, removableWalls.Count);
            Vector2Int wall = removableWalls[randomIndex];
            maze[wall.x, wall.y] = 0;
            removableWalls.RemoveAt(randomIndex);
        }
    }

    private bool IsCornerWall(int x, int y)
    {
        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        List<Vector2Int> wallNeighbors = new List<Vector2Int>();

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;
            if (nx >= 0 && nx < width && ny >= 0 && ny < height && maze[nx, ny] == 1)
            {
                wallNeighbors.Add(dir);
            }
        }

        foreach (var neighbor1 in wallNeighbors)
        {
            foreach (var neighbor2 in wallNeighbors)
            {
                if (neighbor1 + neighbor2 == Vector2Int.zero)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private int CountWallNeighbors(int x, int y)
    {
        int count = 0;
        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;
            if (nx >= 0 && nx < width && ny >= 0 && ny < height && maze[nx, ny] == 1)
            {
                count++;
            }
        }

        return count;
    }

    private void InstantiateMaze()
    {
        GameObject mazeParent = new GameObject("Maze");
        mazeParent.transform.position = Vector3.zero;
        lastGeneratedMaze = mazeParent;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    var wall = Instantiate(wallPrefab, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity, mazeParent.transform);
                    wall.name = $"Wall_{x}_{y}";
                }
            }
        }
    }

    public void ClearLastGeneratedMaze()
    {
        if (lastGeneratedMaze != null)
        {
            DestroyImmediate(lastGeneratedMaze);
            lastGeneratedMaze = null;
        }
    }
}

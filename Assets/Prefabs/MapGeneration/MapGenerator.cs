using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    
    public bool positionPlayerAtEnd = true;

    private int[,] maze;

    private void Start()
    {
        GenerateMaze();
        DrawMaze();

        if (positionPlayerAtEnd)
        {
            PositionPlayerAtEnd(); // Rename to reflect the new behavior
        }
    }

    private void GenerateMaze()
    {
        maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 1; // Initialize all cells as walls
            }
        }

        // Start recursive backtracking from a random cell
        CarvePassages(1, 1);

        // Add exit
        maze[0, 1] = 0;
    }

    private void CarvePassages(int x, int y)
    {
        maze[x, y] = 0; // Mark the current cell as a passage

        // Randomize the directions to explore
        int[] directions = { 0, 1, 2, 3 }; // 0 = up, 1 = right, 2 = down, 3 = left
        System.Random rng = new();
        directions = directions.OrderBy(d => rng.Next()).ToArray();

        foreach (int direction in directions)
        {
            int nx = x, ny = y;

            // Determine the next cell based on the direction
            switch (direction)
            {
                case 0: ny -= 2; break; // Up
                case 1: nx += 2; break; // Right
                case 2: ny += 2; break; // Down
                case 3: nx -= 2; break; // Left
            }

            // Check if the next cell is within bounds and is a wall
            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 1)
            {
                // Carve a passage between the current cell and the next cell
                maze[(x + nx) / 2, (y + ny) / 2] = 0;
                CarvePassages(nx, ny);
            }
        }
    }

    private void DrawMaze()
    {
        Vector3 offset = new(-width / 2f + 0.5f, 0, -height / 2f + 0.5f); // Adjust offset by half a tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    // Instantiate a wall prefab at this position
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.position = new Vector3(x, 0, y) + offset; // Apply the corrected offset
                    wall.transform.parent = transform;
                }
            }
        }
    }

    private void PositionPlayerAtEnd()
    {
        Player player = FindFirstObjectByType<Player>(); // Use FindFirstObjectByType to locate the player with the Player component
        if (player != null)
        {
            Vector3 offset = new Vector3(-width / 2f + 0.5f, 0, -height / 2f + 0.5f); // Adjust offset by half a tile
            player.transform.position = new Vector3(width - 2, 0, height - 2) + offset; // Position player at the end (width-2, height-2)
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' component not found.");
        }
    }
}

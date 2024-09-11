using UnityEngine;
using System.Collections.Generic;

public class Graph
{
    public Node[,] nodes;  // A 2D array to hold nodes in a grid layout

    // Constructor to create a grid of nodes
    public Graph(int width, int height)
    {
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new Node(new Vector3(x, y, 0));
            }
        }
    }

    // Method to get neighboring nodes (e.g., up, down, left, right)
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (var direction in directions)
        {
            int checkX = (int)node.position.x + (int)direction.x;
            int checkY = (int)node.position.y + (int)direction.y;

            if (checkX >= 0 && checkX < nodes.GetLength(0) && checkY >= 0 && checkY < nodes.GetLength(1))
            {
                neighbors.Add(nodes[checkX, checkY]);
            }
        }

        return neighbors;
    }
}

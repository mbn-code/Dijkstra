using UnityEngine;

public class Node
{
    public Vector3 position;  // Position in the game world
    public float distance = Mathf.Infinity;  // Start with an infinite distance
    public Node previousNode = null;  // Used to trace back the path
    public bool isVisited = false;  // To check if the node is visited
    public bool isWalkable = true;  // To check if the node is walkable (not an obstacle)
    
    // Constructor to initialize the node with a position
    public Node(Vector3 pos)
    {
        position = pos;
    }
}
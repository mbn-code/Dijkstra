using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Graph graph;
    private GameObject[,] nodeObjects;  // Store instantiated nodes
    public GameObject nodePrefab;
    public int width = 100;
    public int height = 100;
    public float delay = 0.1f;

    void Start()
    {
        graph = new Graph(width, height);
        nodeObjects = new GameObject[width, height];

        // Instantiate grid visual elements (e.g., node prefabs)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                nodeObjects[x, y] = Instantiate(nodePrefab, pos, Quaternion.identity);
            }
        }

        // Place obstacles in the grid
        //PlaceObstacles();

        // Start the coroutine to visualize A* algorithm
        //StartCoroutine(RunAstarVisualization());
    }

    void PlaceObstacles()
    {
        int obstacleCount = (width * height) / 2; // 10% of the grid will be obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // Skip the start and target nodes 
            if (x == 0 && y == 0 || x == width - 1 && y == height - 1)
            {
                continue;
            }

            // Mark the node as an obstacle
            graph.nodes[x, y].isWalkable = false;

            // Change the color of the node object to black to indicate an obstacle
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void AddObstacle(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isWalkable = false;
    }

    public bool IsObstacle(float x, float y)
    {
        return !graph.nodes[(int)x, (int)y].isWalkable;
    }

    public void RemoveObstacle(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isWalkable = true;
    }

    IEnumerator RunAstarVisualization()
    {
        Astar astar = new Astar();
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        Node startNode = graph.nodes[0, 0];
        Node targetNode = graph.nodes[width - 1, height - 1];
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Visualize the current node being processed
            int cx = (int)currentNode.position.x;
            int cy = (int)currentNode.position.y;
            nodeObjects[cx, cy].GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(delay);

            if (currentNode == targetNode)
            {
                VisualizePath(startNode, targetNode);
                yield break;
            }

            foreach (Node neighbor in graph.GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float newCostToNeighbor = currentNode.gCost + Vector3.Distance(currentNode.position, neighbor.position);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = Vector3.Distance(neighbor.position, targetNode.position);
                    neighbor.previousNode = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void VisualizePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();

        foreach (Node node in path)
        {
            int x = (int)node.position.x;
            int y = (int)node.position.y;
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private bool hasRun = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!hasRun)
            {
                hasRun = !hasRun;
                StartCoroutine(RunAstarVisualization());
            }
            else
            {
                hasRun = !hasRun;
                // Reset the grid
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (!graph.nodes[x, y].isWalkable)
                            continue;

                        nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                        graph.nodes[x, y].Reset();
                    }
                }
            }
        }
    }
}

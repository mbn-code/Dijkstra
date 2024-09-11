using System.Collections;
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

        // Start the coroutine to visualize Dijkstra's algorithm
        //StartCoroutine(RunDijkstraVisualization());
    }

    void PlaceObstacles()
    {
        int obstacleCount = (width * height) / 2; // 10% of the grid will be obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            // Skip the start and target nodes 
            if(x == 0 && y == 0 || x == width - 1 && y == height - 1)
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

    IEnumerator RunDijkstraVisualization()
    {
        Dijkstra dijkstra = new Dijkstra();
        yield return StartCoroutine(dijkstra.FindShortestPathWithVisualization(graph.nodes[0, 0], graph.nodes[width - 1, height - 1], graph, nodeObjects, delay));

        // After the algorithm finishes, visualize the final path
        VisualizePath(graph.nodes[width - 1, height - 1]);
    }

    void VisualizePath(Node targetNode)
    {
        Node currentNode = targetNode;

        // Traverse back from the target node to the start node using the previousNode property
        while (currentNode != null)
        {
            int x = (int)currentNode.position.x;
            int y = (int)currentNode.position.y;

            // Change the color of the node object at the current position to red
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.red;

            // Move to the previous node in the path
            currentNode = currentNode.previousNode;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RunDijkstraVisualization());
        }
    }
}
using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject nodePrefab;
    public float delay = 0.1f; // Time delay between steps in seconds

    private Graph graph;
    private GameObject[,] nodeObjects;  // Store instantiated nodes

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

        // Start the coroutine to visualize Dijkstra's algorithm
        StartCoroutine(RunDijkstraVisualization());
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
}

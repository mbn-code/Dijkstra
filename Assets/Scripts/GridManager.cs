using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Graph graph;
    private GameObject[,] nodeObjects;  // Store instantiated nodes
    public GameObject nodePrefab;
    public int width = 100;
    public int height = 100;
    public float delay = 0.1f;

    private int CurrentAlgorithm = 0; // 0 = Dijkstra, 1 = A*

    public TMPro.TextMeshProUGUI AlgorithmText;


    private Dictionary<string, Node> StartAndEnd = new Dictionary<string, Node>();


    void Start()
    {
        CurrentAlgorithm = PlayerPrefs.GetInt("CurrentAlgorithm", 0);

        if (CurrentAlgorithm == 0)
            AlgorithmText.text = "Current Algorithm:\nDijkstra";
        else
            AlgorithmText.text = "Current Algorithm:\nA*";

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
    
    public bool IsStartOrEnd(float x, float y)
    {
        return (graph.nodes[(int)x, (int)y].isEnd || graph.nodes[(int)x, (int)y].isStart);
    }

    public bool PlaceStartOrEnd(float x, float y)
    {
        if (StartAndEnd.Count == 2)
        {
            Debug.Log("Start and End nodes are already set");
            return false;
        }

        if (StartAndEnd.Count == 0)
        {
            SetStartNode(x, y);
        }
        else
        {
            SetEndNode(x, y);
        }

        return true;
    }

    public void RemoveStartOrEnd(float x, float y)
    {
        if(StartAndEnd.Count == 0)
        {
            Debug.Log("Start amd End nodes are not set");
        }

        Node node = graph.nodes[(int)x, (int)y];

        if (node.isStart)
            RemoveStartNode(x, y);
        else if (node.isEnd)
            RemoveEndNode(x, y);
    }

    public void SetStartNode(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isStart = true;

        if (StartAndEnd.ContainsKey("Start"))
            StartAndEnd.Remove("Start");

        StartAndEnd.Add("Start", graph.nodes[(int)x, (int)y]);
    }

    public void SetEndNode(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isEnd = true;

        if (StartAndEnd.ContainsKey("End"))
            StartAndEnd.Remove("End");

        StartAndEnd.Add("End", graph.nodes[(int)x, (int)y]);
    }

    public void RemoveEndNode(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isEnd = false;

        if (StartAndEnd.ContainsKey("End"))
            StartAndEnd.Remove("End");
    }

    public void RemoveStartNode(float x, float y)
    {
        graph.nodes[(int)x, (int)y].isStart = false;

        if (StartAndEnd.ContainsKey("Start"))
            StartAndEnd.Remove("Start");
    }

    private void ResetPreviousNodes()
    {
        foreach (Node node in graph.nodes)
        {
            node.Reset();
        }
    }

    IEnumerator RunDijkstraVisualization()
    {
        ResetPreviousNodes();

        if (StartAndEnd.Count < 2)
        {
            Debug.Log("Please set the start and end nodes");
            yield break;
        }

        Dijkstra dijkstra = new Dijkstra();
        yield return StartCoroutine(dijkstra.FindShortestPathWithVisualization(StartAndEnd["Start"], StartAndEnd["End"], graph, nodeObjects, delay));

        // After the algorithm finishes, visualize the final path
        VisualizePath(StartAndEnd["Start"], StartAndEnd["End"]);
    }

    IEnumerator RunAstarVisualization()
    {
        ResetPreviousNodes();

        if (StartAndEnd.Count < 2)
        {
            Debug.Log("Please set the start and end nodes");
            yield break;
        }

        Astar astar = new Astar();
        yield return StartCoroutine(astar.FindPath(StartAndEnd["Start"], StartAndEnd["End"], graph, nodeObjects, delay));

        // After the algorithm finishes, visualize the final path
        VisualizePath(StartAndEnd["Start"], StartAndEnd["End"]);
    }

    void VisualizePath(Node startNode, Node targetNode)
    {
        nodeObjects[(int)startNode.position.x, (int)startNode.position.y].GetComponent<SpriteRenderer>().color = Color.red;
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

    public void SwitchDijkstra()
    {
        AlgorithmText.text = "Current Algorithm:\nDijkstra";
        CurrentAlgorithm = 0;
        PlayerPrefs.SetInt("CurrentAlgorithm", 0);
        PlayerPrefs.Save();
    }

    public void SwitchAstar()
    {
        AlgorithmText.text = "Current Algorithm:\nA*";
        CurrentAlgorithm = 1;
        PlayerPrefs.SetInt("CurrentAlgorithm", 1);
        PlayerPrefs.Save();
    }

    private bool hasRun = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!hasRun)
            {
                hasRun = !hasRun;
                if (CurrentAlgorithm == 0)
                    StartCoroutine(RunDijkstraVisualization());
                else
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

                        if (graph.nodes[x, y].isStart || graph.nodes[x, y].isEnd)
                            continue;

                        nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.white;
                        graph.nodes[x, y].Reset();
                    }
                }
            }
        }
    }
}

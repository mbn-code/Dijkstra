using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private Graph graph;
    private GameObject[,] nodeObjects;  // Store instantiated nodes
    public GameObject nodePrefab;
    public int width = 10;
    public int height = 10;
    public float delay = 0f;

    private int CurrentAlgorithm = 0; // 0 = Dijkstra, 1 = A*

    public TextMeshProUGUI AlgorithmText;
    public TextMeshProUGUI TimeText;

    public Slider GridSizeSlider;

    internal event System.Action OnGridSizeChanged;


    private Dictionary<string, Node> StartAndEnd = new Dictionary<string, Node>();


    private bool Initialized = false;
    void Start()
    {
        GridSizeSlider.value = PlayerPrefs.GetInt("GridsSize", 10);

        width = (int)GridSizeSlider.value;
        height = (int)GridSizeSlider.value;

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

        OnGridSizeChanged?.Invoke(); // Fix cam pos

        Initialized = true;
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
            UnityEngine.Debug.Log("Start and End nodes are already set");
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
            UnityEngine.Debug.Log("Start amd End nodes are not set");
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

    IEnumerator RunPathFinding()
    {
        ResetPreviousNodes();

        if (StartAndEnd.Count < 2)
        {
            UnityEngine.Debug.Log("Please set the start and end nodes");
            yield break;
        }

        Stopwatch stopwatch = new Stopwatch();

        if (CurrentAlgorithm == 0)
        {
            Dijkstra dijkstra = new Dijkstra();
            stopwatch.Start();
            yield return StartCoroutine(dijkstra.FindShortestPathWithVisualization(StartAndEnd["Start"], StartAndEnd["End"], graph, nodeObjects, delay));
            stopwatch.Stop();
        }
        else
        {
            Astar astar = new Astar();
            stopwatch.Start();
            yield return StartCoroutine(astar.FindPath(StartAndEnd["Start"], StartAndEnd["End"], graph, nodeObjects, delay));
            stopwatch.Stop();
        }

        TimeText.text = stopwatch.ElapsedMilliseconds + "ms";

        VisualizePath(StartAndEnd["Start"], StartAndEnd["End"]);

        isRunning = false;
    }

    void VisualizePath(Node startNode, Node targetNode)
    {
        nodeObjects[(int)startNode.position.x, (int)startNode.position.y].GetComponent<SpriteRenderer>().color = new Color(0.62f, 0.12f, 0.94f);
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
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = new Color(0.62f, 0.12f, 0.94f);
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

    public void GridSizeChanged()
    {
        if(!Initialized)
            return;

        width = (int)GridSizeSlider.value;
        height = (int)GridSizeSlider.value;

        // Clean up the grid
        foreach (GameObject gameObject in nodeObjects)
        {
            Destroy(gameObject);
        }

        // Make new stuff for algorithms
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

        StartAndEnd.Clear();

        // Update CameraManager
        OnGridSizeChanged?.Invoke();

        PlayerPrefs.SetInt("GridsSize", (int)GridSizeSlider.value);
        PlayerPrefs.Save();
    }

    private bool hasRun = false;
    private bool isRunning = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRunning)
        {
            if (!hasRun)
            {
                hasRun = !hasRun;
                isRunning = true;
                StartCoroutine(RunPathFinding());
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

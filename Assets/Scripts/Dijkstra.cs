using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    public IEnumerator FindShortestPathWithVisualization(Node startNode, Node targetNode, Graph graph, GameObject[,] nodeObjects, float delay)
    {
        //Highlight target node
        GameObject finalTargetNode = nodeObjects[(int)targetNode.position.x, (int)targetNode.position.y];
        finalTargetNode.GetComponent<SpriteRenderer>().color = Color.green;

        // Priority Queue to process nodes by distance (closest first)
        PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>();
        startNode.distance = 0;
        priorityQueue.Enqueue(startNode);

        while (priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();
            currentNode.isVisited = true;

            // Visualize the current node being processed (e.g., turn it yellow)
            int x = (int)currentNode.position.x;
            int y = (int)currentNode.position.y;
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;

            // Wait for the delay to show the process step-by-step
            yield return new WaitForSeconds(delay);

            // If we reached the target node, stop the process
            if (currentNode == targetNode)
                break;

            foreach (var neighbor in graph.GetNeighbors(currentNode))
            {
                if (neighbor.isVisited)
                    continue;

                float tentativeDistance = currentNode.distance + Vector3.Distance(currentNode.position, neighbor.position);

                if (tentativeDistance < neighbor.distance)
                {
                    neighbor.distance = tentativeDistance;
                    neighbor.previousNode = currentNode;

                    // Visualize the neighbor being added to the queue (e.g., turn it blue)
                    int nx = (int)neighbor.position.x;
                    int ny = (int)neighbor.position.y;
                    nodeObjects[nx, ny].GetComponent<SpriteRenderer>().color = Color.blue;

                    priorityQueue.Enqueue(neighbor);
                }
            }
        }
    }
}

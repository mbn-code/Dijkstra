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

        
        // Flatten the 2D array into a single list
        List<Node> unvisitedNodes = new List<Node>();
        for (int x = 0; x < graph.nodes.GetLength(0); x++)
        {
            for (int y = 0; y < graph.nodes.GetLength(1); y++)
            {
                unvisitedNodes.Add(graph.nodes[x, y]);
            }
        }

        startNode.distance = 0;

        while (unvisitedNodes.Count > 0)
        {
            // Sort the unvisited nodes by distance
            unvisitedNodes.Sort((node1, node2) => node1.distance.CompareTo(node2.distance));
            Node currentNode = unvisitedNodes[0];
            unvisitedNodes.Remove(currentNode);

            // If the current node is not walkable, skip it
            if (!currentNode.isWalkable)
            {
                continue;
            }

            // If the current node is the target node, break out of the loop
            if (currentNode == targetNode)
            {
                break;
            }

            // Update the distances to the neighboring nodes
            foreach (Node neighbor in graph.GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable)
                {
                    continue;
                }

                float tentativeDistance = currentNode.distance + Vector3.Distance(currentNode.position, neighbor.position);
                if (tentativeDistance < neighbor.distance)
                {
                    neighbor.distance = tentativeDistance;
                    neighbor.previousNode = currentNode;
                }
            }

            // Visualize the current node as visited
            int x = (int)currentNode.position.x;
            int y = (int)currentNode.position.y;
            nodeObjects[x, y].GetComponent<SpriteRenderer>().color = Color.blue;

            yield return new WaitForSeconds(delay);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    public IEnumerator FindPath(Node startNode, Node targetNode, Graph graph, GameObject[,] nodeObjects, float delay)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
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
                break; // Har fundet frem til en rigtige
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
}

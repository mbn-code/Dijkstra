using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    private GameObject gridManagerObject;
    private GridManager gridManager;  // Reference to the GridManager script

    void Start()
    {
        // Get GridManager from gridManagerObject
        gridManagerObject = GameObject.Find("GridManager");
        gridManager = gridManagerObject.GetComponent<GridManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) // Blocking Nodes
        {
            if (gridManager.IsStartOrEnd(transform.position.x, transform.position.y))
                return;

            if (gridManager.IsObstacle(transform.position.x, transform.position.y))
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                gridManager.RemoveObstacle(transform.position.x, transform.position.y);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.black;
                gridManager.AddObstacle(transform.position.x, transform.position.y);
            }
        }

        if (Input.GetMouseButtonDown(1)) // Start and Target Nodes
        {
            if (gridManager.IsObstacle(transform.position.x, transform.position.y))
                return;

            if (gridManager.IsStartOrEnd(transform.position.x, transform.position.y))
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                gridManager.RemoveStartOrEnd(transform.position.x, transform.position.y);
            }
            else
            {
                if (gridManager.PlaceStartOrEnd(transform.position.x, transform.position.y))
                    GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}

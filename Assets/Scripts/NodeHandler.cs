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
    
    void OnMouseDown()
    {
        if(transform.position.x == 0 && transform.position.y == 0 || transform.position.x == gridManager.width - 1 && transform.position.y == gridManager.height - 1)
        {
            return;
        }
        
        GetComponent<SpriteRenderer>().color = Color.black;
        gridManager.AddObstacle(transform.position.x, transform.position.y);
    }
}

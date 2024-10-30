using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject gridManagerObject;
    private GridManager gridManager;  // Reference to the GridManager script
    private Camera MyCamera;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = gridManagerObject.GetComponent<GridManager>();
        MyCamera = GetComponent<Camera>();

        gridManager.OnGridSizeChanged += UpdateCameraPosition;

        // Set the camera position to the center of the grid
        Vector3 cameraPosition = new Vector3(gridManager.width / 2, gridManager.height / 2, -10);
        transform.position = cameraPosition;
    }

    private void UpdateCameraPosition()
    {
        Vector3 cameraPosition = new Vector3(gridManager.width / 2, gridManager.height / 2, -10);
        MyCamera.orthographicSize = gridManager.width * 1.5f;
        transform.position = cameraPosition;
    }


}

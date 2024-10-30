# Pathfinding Visualization Project

This project is a Unity-based application for visualizing pathfinding algorithms. It includes implementations of Dijkstra's algorithm and A* algorithm, allowing users to see how these algorithms work in real-time.

## Features

- **Dijkstra's Algorithm**: Visualize the shortest path using Dijkstra's algorithm.
- **A* Algorithm**: Visualize the shortest path using the A* algorithm.
- **Obstacle Placement**: Add and remove obstacles to see how they affect the pathfinding.
- **Start and End Nodes**: Set start and end nodes to define the pathfinding area.
- **Algorithm Switching**: Easily switch between Dijkstra's and A* algorithms.

## Getting Started

### Prerequisites

- Unity 2020.3 or later
- TextMesh Pro package

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/mbn-code/Unity-Path-finding-demo.git
    ```
2. Open the project in Unity.

### Usage

1. Open the `Scenes` folder and load the main scene.
2. Use the UI to place obstacles, set start and end nodes, and switch between algorithms.
3. Press the play button to start the visualization.

## Project Structure

- **Assets/Scripts**: Contains all the scripts for the project.
  - [`GridManager`](Assets/Scripts/GridManager.cs): Manages the grid and handles user interactions.
  - [`Dijkstra`](Assets/Scripts/Dijkstra.cs): Implements Dijkstra's algorithm.
- **Assets/Scenes**: Contains the Unity scenes.
- **Assets/Prefab**: Contains prefabs used in the project.

## Contributing

Contributions are welcome! Please read the [contributing guidelines](Library/PackageCache/com.unity.2d.tilemap.extras@3.1.2/CONTRIBUTING.md) before making any changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Unity Technologies for providing the Unity engine.
- Contributors to the open-source libraries used in this project.

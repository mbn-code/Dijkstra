# Unity Pathfinding Visualization Project

This Unity project visualizes and compares two pathfinding algorithms, Dijkstra’s and A*, demonstrating how they calculate the shortest paths on a grid in real-time. Users can set start and end points, place obstacles, and observe each algorithm's path calculation and efficiency differences.

![image](https://github.com/user-attachments/assets/8445a2df-15b8-4b91-a76b-044ccee59039)

## Table of Contents
1. [Features](#features)
2. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
   - [Usage](#usage)
3. [Algorithms Overview](#algorithms-overview)
   - [Dijkstra's Algorithm](#dijkstras-algorithm)
   - [A* Algorithm](#a-algorithm)
   - [Comparison of Dijkstra's and A*](#comparison-of-dijkstra-and-a)
4. [Project Structure](#project-structure)
5. [Performance Analysis](#performance-analysis)
6. [Contributing](#contributing)
7. [License](#license)

---

## Features
- **Dijkstra's Algorithm**: Visualizes pathfinding using Dijkstra’s algorithm.
- **A* Algorithm**: Visualizes pathfinding with A* algorithm, known for faster pathfinding in most cases.
- **Obstacle Placement**: Add and remove obstacles to explore how they influence the path.
- **Real-time Algorithm Switching**: Compare algorithm performance directly by switching between Dijkstra’s and A* during runtime.

## Getting Started

### Prerequisites
- Unity 2020.3 or later
- TextMesh Pro package

### Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/mbn-code/Unity-Path-finding-demo.git
   ```
2. Open the project in Unity.
3. Ensure TextMesh Pro is installed from the Package Manager if not included by default.

### Usage
1. Open the `Scenes` folder and load the main scene.
2. Use the UI to set start and end nodes, place obstacles, and switch algorithms.
3. Press the play button in Unity to start the visualization.
4. Interact:
   - **Left Click**: Place or remove obstacles.
   - **Right Click**: Set start and end nodes (two are needed: one for each).
   - **Space**: Execute the selected pathfinding algorithm on the grid.

## Algorithms Overview

### Dijkstra's Algorithm
Dijkstra’s algorithm finds the shortest path from a start node to a target node across a grid by:
- Exploring all nodes equally, without bias towards the target.
- Assigning costs to each node based on cumulative travel distance from the start node.
- Updating the shortest path to the target if a shorter route is discovered.

### A* Algorithm
A* is an optimized pathfinding algorithm that combines elements of Dijkstra and Greedy Best-First Search:
- Evaluates both the travel cost from the start and an estimated distance (heuristic) to the target.
- Uses the heuristic to prioritize nodes closer to the target, usually resulting in faster pathfinding.
- Suited for large grids and scenarios where finding the shortest path efficiently is critical.

### Comparison of Dijkstra and A*
| Feature       | Dijkstra's Algorithm                  | A* Algorithm                           |
|---------------|--------------------------------------|----------------------------------------|
| Cost Tracking | Tracks cumulative cost from the start node only | Tracks cumulative cost plus estimated distance to the target |
| Heuristics    | None (explores all directions equally) | Uses heuristics for faster results |
| Efficiency    | Slower, especially on large grids | Faster due to heuristic prioritization |

## Project Structure
- **Assets/Scripts**: Contains core scripts:
   - `GridManager`: Manages grid creation and updates based on user interaction.
   - `Dijkstra.cs`: Implements Dijkstra’s algorithm.
   - `Astar.cs`: Implements A* algorithm.
- **Assets/Scenes**: Contains the main Unity scenes.
- **Assets/Prefab**: Includes prefabs used within the project for nodes and other UI elements.

## Performance Analysis
The performance of the algorithms was tested using different grid sizes, and metrics such as algorithm speed and efficiency were compared.

### Big O Complexity
- **Dijkstra’s Algorithm**: O(n^2 log(n)) due to its priority queue and repeated evaluations.
- **A* Algorithm**: O(n^2) in the worst case, as it uses heuristics to reduce the number of nodes explored.

### Empirical Testing
Tests were conducted on 10x10 and 50x50 grids, measuring average execution times across five trials:
| Grid Size | Algorithm | Average Time |
|-----------|-----------|--------------|
| 10x10     | Dijkstra  | 98.4ms       |
| 10x10     | A*        | 14ms         |
| 50x50     | Dijkstra  | 5284.6ms     |
| 50x50     | A*        | 103ms        |

Results indicate that A* performs significantly faster, particularly with larger grid sizes, due to its heuristic-based prioritization.

## Contributing
Contributions are welcome! Please read the contributing guidelines before submitting any pull requests.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class Maze : MonoBehaviour
    {
        [SerializeField] private Vector3 _mazeWorldSize;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _wallThickness;
        [SerializeField] private Transform _wallPrefab;
        [SerializeField] private Transform _floorPrefab;

        private Grid _grid;

        public void Init()
        {
            _grid = GenerateMaze(_mazeWorldSize.x, _mazeWorldSize.y, _mazeWorldSize.z, _cellSize);

            DrawMaze(_grid);

            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        private Grid GenerateMaze(float width, float height, float length, float cellSize)
        {
            var grid = new Grid(width, height, length, cellSize);
            CreateMazeFromGrid(grid);

            return grid;
        }

        private void CreateMazeFromGrid(Grid grid)
        {
            var stack = new Stack<Cell>();
            var random = new System.Random(/*Seed*/);
            var randomGridX = random.Next(0, grid.GridSize.x);
            var randomGridY = random.Next(0, grid.GridSize.y);

            var randomCell = grid[randomGridX, randomGridY];
            randomCell.Visited = true;
            stack.Push(randomCell);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var neighbours = grid.GetNeighbours(current).Where(c => c.Visited == false);

                if (neighbours.Count() > 0)
                {
                    stack.Push(current);
                    var randomNeighbourIndex = random.Next(0, neighbours.Count());
                    var randomNeighbour = neighbours.ElementAt(randomNeighbourIndex);

                    current.RemoveSharedWall(randomNeighbour);
                    randomNeighbour.Visited = true;
                    stack.Push(randomNeighbour);
                }
            }
        }

        private void DrawMaze(Grid grid)
        {
            var floor = Instantiate(_floorPrefab, transform);
            floor.eulerAngles = new Vector3(90, 0, 0);
            floor.localScale = new Vector3(grid.GridWorldSize.x, grid.GridWorldSize.z, 1);

            for (int i = 0; i < grid.GridSize.x; i++)
            {
                for (int j = 0; j < grid.GridSize.y; j++)
                {
                    var cell = grid[i, j];
                    var cellPos = transform.position + cell.WorldPosition;

                    if (cell.Walls.HasFlag(Walls.LEFT))
                    {
                        var leftWall = Instantiate(_wallPrefab, transform);
                        leftWall.position = cellPos + new Vector3(-_cellSize / 2, 0, 0);
                        leftWall.localScale = new Vector3(_wallThickness, grid.GridWorldSize.y, _cellSize + _wallThickness);
                    }

                    if (cell.Walls.HasFlag(Walls.BOTTOM))
                    {
                        var bottomWall = Instantiate(_wallPrefab, transform);
                        bottomWall.position = cellPos + new Vector3(0, 0, -_cellSize / 2);
                        bottomWall.localScale = new Vector3(_cellSize + _wallThickness, grid.GridWorldSize.y, _wallThickness);
                    }

                    if (i == grid.GridSize.x - 1)
                    {
                        if (cell.Walls.HasFlag(Walls.TOP))
                        {
                            var topWall = Instantiate(_wallPrefab, transform);
                            topWall.position = cellPos + new Vector3(0, 0, _cellSize / 2);
                            topWall.localScale = new Vector3(_cellSize + _wallThickness, grid.GridWorldSize.y, _wallThickness);
                        }
                    }

                    if (j == grid.GridSize.y - 1)
                    {
                        if (cell.Walls.HasFlag(Walls.RIGHT))
                        {
                            var rightWall = Instantiate(_wallPrefab, transform);
                            rightWall.position = cellPos + new Vector3(_cellSize / 2, 0, 0);
                            rightWall.localScale = new Vector3(_wallThickness, grid.GridWorldSize.y, _cellSize + _wallThickness);
                        }
                    }
                }
            }
        }

        #region Debug
        public bool ShowGrid = false;

        private void OnDrawGizmos()
        {
            if (ShowGrid && _grid != null)
            {
                for (int i = 0; i < _grid.GridSize.x; i++)
                {
                    for (int j = 0; j < _grid.GridSize.y; j++)
                    {
                        var cell = _grid[i, j];
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(transform.localPosition + cell.WorldPosition, Vector3.one * (_cellSize - 0.1f));
                    }
                }
            }
        }
        #endregion
    }
}

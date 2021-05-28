using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class Grid
    {
        private Cell[,] _grid;

        public Vector3 GridWorldSize { get; private set; }
        public Vector2Int GridSize { get; private set; }
        public float CellSize { get; private set; }

        public Cell this[int x, int y]
        {
            get => _grid[x, y];
            private set => _grid[x, y] = value;
        }

        public Grid(float width, float height, float length, float cellSize)
        {
            GridWorldSize = new Vector3(width, height, length);
            CellSize = cellSize;
            GridSize = new Vector2Int(Mathf.FloorToInt(width / cellSize), Mathf.FloorToInt(length / cellSize));

            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Cell[GridSize.x, GridSize.y];

            Vector3 gridBottomLeft = -Vector3.right * GridWorldSize.x / 2 + Vector3.up * GridWorldSize.y / 2 - Vector3.forward * GridWorldSize.z / 2;

            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    Vector3 cellWorldPoint = gridBottomLeft +
                        Vector3.forward * (i * CellSize + CellSize / 2) +
                        Vector3.right * (j * CellSize + CellSize / 2);

                    Walls initial = Walls.TOP | Walls.RIGHT | Walls.BOTTOM | Walls.LEFT;

                    _grid[i, j] = new Cell(cellWorldPoint, new Vector2Int(i, j), initial);
                }
            }
        }

        public IEnumerable<Cell> GetNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();
            var cellX = cell.GridPosition.x;
            var cellY = cell.GridPosition.y;

            if (cellX > 0)
            {
                neighbours.Add(_grid[cellX - 1, cellY]);
            }
            if (cellX < GridSize.x - 1)
            {
                neighbours.Add(_grid[cellX + 1, cellY]);
            }
            if (cellY > 0)
            {
                neighbours.Add(_grid[cellX, cellY - 1]);
            }
            if (cellY < GridSize.y - 1)
            {
                neighbours.Add(_grid[cellX, cellY + 1]);
            }

            return neighbours;
        }
    }
}
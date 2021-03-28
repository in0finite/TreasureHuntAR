using System;
using UnityEngine;

namespace Maze
{
    [Flags]
    public enum Walls
    {
        TOP = 1,
        RIGHT = 2,
        BOTTOM = 4,
        LEFT = 8,
    }

    public class Cell
    {
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public Walls Walls { get; private set; }
        public bool Visited { get; set; }

        public Cell(Vector3 worldPos, Vector2Int gridPos, Walls walls)
        {
            WorldPosition = worldPos;
            GridPosition = gridPos;
            Walls = walls;
        }

        public void RemoveSharedWall(Cell cell)
        {
            var dir = cell.GridPosition - GridPosition;

            if (dir == Vector2.left)
            {
                Walls &= ~Walls.BOTTOM;
                cell.Walls &= ~Walls.TOP;
            }
            else if (dir == Vector2.right)
            {
                Walls &= ~Walls.TOP;
                cell.Walls &= ~Walls.BOTTOM;

            }
            else if (dir == Vector2.up)
            {
                Walls &= ~Walls.RIGHT;
                cell.Walls &= ~Walls.LEFT;

            }
            else if (dir == Vector2.down)
            {
                Walls &= ~Walls.LEFT;
                cell.Walls &= ~Walls.RIGHT;
            }
        }
    }
}

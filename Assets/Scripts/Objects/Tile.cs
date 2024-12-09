using Assets.Scripts.TileModifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrain
{
    public class Tile
    {
        public GroundTypeEnum GroundType { get; set; }

        public TerrainLayerEnum Layer { get; set; }

        public TileModifierBase Modifier { get; set; }

        public Vector3 Center { get; }

        public int X { get; }
        
        public int Y { get; }

        public List<Tile> Neighbours { get; private set; }

        public List<Tile> OrthogonalNeighbours { get; private set; }

        public List<Tile> VerticalNeighbours { get; private set; }

        public List<Tile> HorizontalNeighbours { get; private set; }

        public bool IsWalkable()
        {
            return Layer == TerrainLayerEnum.Ground;
        }

        public Tile(int x, int y, Vector2 center)
        {
            X = x;
            Y = y;
            Center = center;
        }

        public void InitNeighbours(TerrainData terrainData)
        {
            Neighbours = new();
            int[] rowOffsets = { -1, 1, 0, 0, -1, -1, 1, 1 };
            int[] colOffsets = { 0, 0, -1, 1, -1, 1, -1, 1 };
            for (int i = 0; i < rowOffsets.Length; i++)
            {
                int newRow = X + rowOffsets[i];
                int newCol = Y + colOffsets[i];
                var neighbour = terrainData.GetTile(newRow, newCol);
                if (neighbour != null)
                {
                    Neighbours.Add(neighbour);
                }
            }

            OrthogonalNeighbours = new();
            int[] rowOffsetsOrt = { -1, 1, 0, 0 };
            int[] colOffsetsOrt = { 0, 0, -1, 1 };
            for (int i = 0; i < rowOffsetsOrt.Length; i++)
            {
                int newRow = X + rowOffsetsOrt[i];
                int newCol = Y + colOffsetsOrt[i];
                var neighbour = terrainData.GetTile(newRow, newCol);
                if (neighbour != null)
                {
                    OrthogonalNeighbours.Add(neighbour);
                }
            }

            VerticalNeighbours = new();
            int[] verticalOffsets = { -1, 1 };

            for (int i = 0; i < verticalOffsets.Length; i++)
            {
                int newRow = X + verticalOffsets[i];
                var neighbour = terrainData.GetTile(newRow, Y);
                if (neighbour != null)
                {
                    VerticalNeighbours.Add(neighbour);
                }
            }

            HorizontalNeighbours = new();
            int[] horizontalOffsets = { -1, 1 };

            for (int i = 0; i < horizontalOffsets.Length; i++)
            {
                int newCol = Y + horizontalOffsets[i];
                var neighbour = terrainData.GetTile(X, newCol);
                if (neighbour != null)
                {
                    HorizontalNeighbours.Add(neighbour);
                }
            }
        }
    }
}

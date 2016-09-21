using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using UnityEngine;

namespace Assets
{
    public class MapGenerator1 : MonoBehaviour
    {
        public int Width;
        public int Height;

        public string Seed;
        public bool UseRandomSeed;

        [Range(0, 100)]
        public int RandomFillPercent;

        [Range(0,100)]
        public int HeightUpperLimit ;

        [Range(0, 100)]
        public int HeightLowerLimit;
        
        private List<List<Vector3>> _pointsInPlane;
        private float _largestY;
        private float _smallestY;
        private float _lowerBoundaryY;
        private float _upperBoundaryY;

        public GameObject[] Trees;
        
        int[,] map;

        void Start()
        {
            var vertices = GetComponent<MeshFilter>().mesh.vertices;
            vertices = vertices.OrderBy(s => s.x).ThenBy(s => s.z).ToArray();
            var v1 = vertices.OrderBy(s => s.y);

            _largestY = v1.LastOrDefault().y;
            _smallestY = v1.FirstOrDefault().y;
            _lowerBoundaryY = _smallestY + (_largestY - _smallestY) * (0.01f*HeightLowerLimit);
            _upperBoundaryY = _smallestY + (_largestY - _smallestY) * (0.01f*HeightUpperLimit);

            _pointsInPlane = new List<List<Vector3>>();
            for (int i = 0; i < Width; i++)
            {
                var t = new List<Vector3>();
                
                for (int j = 0; j < Height; j++)
                {
                    t.Add(vertices[i* Height + j]);
                }
                _pointsInPlane.Add(t);
            }
            
            GenerateMap();
            Draw();
        }

        void Update()
        {
        }

        void GenerateMap()
        {
            map = new int[Width, Height];
            RandomFillMap();

            for (int i = 0; i < 1; i++)
            {
                SmoothMap();
            }
        }


        void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = Random.value.ToString(CultureInfo.InvariantCulture);
            }

            var pseudoRandom = new System.Random(Seed.GetHashCode());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < RandomFillPercent) ? 1 : 0;
                }
            }
        }

        void SmoothMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        map[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        map[x, y] = 0;

                }
            }
        }

        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
                        }
                    }
                }
            }

            return wallCount;
        }


        void OnDrawGizmos()
        {
            if (map == null) return;

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    var pos = new Vector3(-Width / 2 + x + .5f, 0, -Height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
        }

        private void Draw()
        {
            var thisMatrix = transform.localToWorldMatrix;
            if (map == null) return;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    // There should not be a tree here
                    if (map[x,y] == 0)
                        continue;

                    // Are we outsite the boundary?
                    var pointInPlain = _pointsInPlane[x][y];
                    if (pointInPlain.y < _lowerBoundaryY || pointInPlain.y > _upperBoundaryY)
                        continue;

                    // Spawn the tree at that position
                    Instantiate(Trees[new System.Random().Next(0, Trees.Length)], thisMatrix.MultiplyPoint3x4(pointInPlain), Quaternion.identity);
                }
        }

    }
}
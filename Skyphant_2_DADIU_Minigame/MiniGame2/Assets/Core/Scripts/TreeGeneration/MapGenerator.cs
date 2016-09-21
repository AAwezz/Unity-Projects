using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Assets.Core.Scripts.TreeGeneration
{
    public class MapGenerator : MonoBehaviour
    {
        public int Width;
        public int Height;
        public int NumberOfClosters;
        public int NumberOfStones;

        public string Seed;
        public bool UseRandomSeed;

        [Range(0,100)]
        public int HeightUpperLimit ;

        [Range(0, 100)]
        public int HeightLowerLimit;

        [Range(0,100)]
        public float SizeVariation;

        public GameObject[] Trees;
        public GameObject[] Stones;

        private List<List<Vector3>> _pointsInPlane;
        private float _largestY;
        private float _smallestY;
        private float _lowerBoundaryY;
        private float _upperBoundaryY;
        private int[,] _map;

        void Start()
        {
            // Seed vs 'true' random
            if (UseRandomSeed)
            {
                Seed = Random.value.ToString(CultureInfo.InvariantCulture);
            }
            Random.seed = Seed.GetHashCode();

            // Get and order vertices
            var vertices = GetComponent<MeshFilter>().mesh.vertices;
            vertices = vertices.OrderBy(s => s.x).ThenBy(s => s.z).ToArray();
            var v1 = vertices.OrderBy(s => s.y);

            // Lower and upper boundary
            _largestY = v1.LastOrDefault().y;
            _smallestY = v1.FirstOrDefault().y;
            _lowerBoundaryY = _smallestY + (_largestY - _smallestY) * (0.01f*HeightLowerLimit);
            _upperBoundaryY = _smallestY + (_largestY - _smallestY) * (0.01f*HeightUpperLimit);

            // Create matrix with vertives
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

            // Generate seeds for clusters
            GenerateClusters();

            // Actually adds trees to the cluster, otherwise only lonely trees
            for (var i = 0; i < 2; i++)
            {
                SmoothClusters();
            }

            // Add stones
            GeneateStones();

            DrawTrees();
            DrawStones();

        }

        void GenerateClusters()
        {
            _map = new int[Width, Height];
            var numCLusters = 0;
            var maxTries = 0;

            //Find some starting points, within the defines bounds (maxTries there to make sure no infinite loop.
            while (numCLusters < NumberOfClosters && maxTries++ < 500 && _lowerBoundaryY < _upperBoundaryY)
            {
                var x = Random.Range(0, _pointsInPlane.Count);
                var y = Random.Range(0, _pointsInPlane[x].Count);

                // Not a valid candidate
                if (!(_pointsInPlane[x][y].y < _upperBoundaryY) || !(_pointsInPlane[x][y].y > _lowerBoundaryY))
                    continue;

                _map[x, y] = 1;
                numCLusters++;
            }
        }

        private void SmoothClusters()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (_map[x, y] == 1)
                        continue;
                    if (neighbourWallTiles > 0 && Random.Range(0,100) < 15)
                        _map[x, y] = 1;
                    else
                        _map[x, y] = 0;
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
                            wallCount += _map[neighbourX, neighbourY];
                        }
                    }
                }
            }

            return wallCount;
        }

        void GeneateStones()
        {
            var numStones = 0;
            var maxTries = 0;
            //Find some starting points, within the defines bounds (maxTries there to make sure no infinite loop.
            while (numStones < NumberOfClosters && maxTries++ < 500 && _lowerBoundaryY < _upperBoundaryY)
            {
                var x = Random.Range(0, _pointsInPlane.Count);
                var y = Random.Range(0, _pointsInPlane[x].Count);

                // Not a valid candidate
                if (!(_pointsInPlane[x][y].y < _upperBoundaryY) || !(_pointsInPlane[x][y].y > _lowerBoundaryY) || _map[x, y] != 0)
                    continue;

                _map[x, y] = 2;
                numStones++;
            }
        }
        
/*        void OnDrawGizmos()
        {
            if (_map == null) return;

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Height; y++)
                {
                    Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
                    var pos = new Vector3(-Width / 2 + x + .5f, 0, -Height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
        }*/

        private void DrawTrees()
        {
            if(Trees.Length < 1)
                return;
            
            var thisMatrix = transform.localToWorldMatrix;
            if (_map == null) return;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    // There should not be a tree here
                    if (_map[x,y] != 1)
                        continue;

                    // Are we outsite the boundary?
                    var pointInPlain = _pointsInPlane[x][y];
                    if (pointInPlain.y < _lowerBoundaryY || pointInPlain.y > _upperBoundaryY)
                        continue;

                    // Spawn the tree at that position
                    var tree = (GameObject)Instantiate(Trees[Random.Range(0, Trees.Length)], thisMatrix.MultiplyPoint3x4(pointInPlain), Quaternion.identity);

                    // Random rotation
                    var rot = tree.transform.localRotation;
                    rot.y = Random.Range(0, 360);
                    tree.transform.rotation = rot;

                    // Random scale
                    var scale = tree.transform.localScale;
                    scale = scale*(1+Random.Range(0, SizeVariation) /100f);
                    tree.transform.localScale = scale;
                }
        }

        void DrawStones()
        {
            if(Stones.Length < 1)
                return;

            var thisMatrix = transform.localToWorldMatrix;
            if (_map == null) return;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    // There should not be a stone here
                    if (_map[x, y] != 2)
                        continue;

                    // Are we outsite the boundary?
                    var pointInPlain = _pointsInPlane[x][y];
                    if (pointInPlain.y < _lowerBoundaryY || pointInPlain.y > _upperBoundaryY)
                        continue;

                    // Spawn the tree at that position
                    var stone = (GameObject)Instantiate(Stones[Random.Range(0, Stones.Length)], thisMatrix.MultiplyPoint3x4(pointInPlain), Quaternion.identity);

                    // Random rotation
                    var rot = stone.transform.localRotation;
                    rot.y = Random.Range(0, 360);
                    stone.transform.rotation = rot;

                    // Random scale
                    var scale = stone.transform.localScale;
                    scale = scale * (1 + Random.Range(0, SizeVariation) / 100f);
                    stone.transform.localScale = scale;
                }
        }


    }
}
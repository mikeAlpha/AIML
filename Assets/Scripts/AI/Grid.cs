//Written By Arka Deb

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mikealpha.AI.Pathfinding
{
    public enum MapType
    {
        Plane,
        Terrain
    }

    [ExecuteInEditMode]
    public class Grid : MonoBehaviour
    {
        public bool VisualizePath;

        public int gridTotalRows;
        public int gridTotalCols;

        public float MaxSlopeForUnit = 45.0f;
        public LayerMask unwalkable;

        int gridUnitSizeX;
        int gridUnitSizeY;

        float nodeDiameter;
        public float nodeRadius;

        public Color OpenNodeColor;
        public Color LowAltNodeColor;
        public Color MidAltNodeColor;
        public Color HighAltNodeColor;

        public MapType mMapType;
        Terrain mTerrain;

        Node[,] nodes;
        float MaxHeight = 0.0f;
        public List<Node> pathFound = new List<Node>();

        void Awake()
        {
            nodeDiameter = nodeRadius * 2;

            if (mMapType == MapType.Terrain)
                mTerrain = GetComponent<Terrain>();

            CreateGrid();
        }

        void CreateGrid()
        {
            nodes = new Node[gridTotalCols, gridTotalRows];
            gridUnitSizeX = Mathf.RoundToInt(gridTotalCols / nodeDiameter);
            gridUnitSizeY = Mathf.RoundToInt(gridTotalRows / nodeDiameter);

            Vector3 worldBottomLeft = Vector3.zero;

            if (mMapType == MapType.Plane)
                worldBottomLeft = transform.position - Vector3.right * gridTotalCols / 2 - Vector3.forward * gridTotalRows / 2;
            else
            {
                Vector3 terrainPos = new Vector3(transform.position.x + gridTotalCols / 2, transform.position.y, transform.position.z + gridTotalRows / 2);
                worldBottomLeft = terrainPos - Vector3.right * gridTotalCols / 2 - Vector3.forward * gridTotalRows / 2;
            }


            for (int x = 0; x < gridTotalCols; x++)
            {
                for (int y = 0; y < gridTotalRows; y++)
                {
                    Vector3 worldPosition = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    NodeType nodetype = NodeType.OPEN; ;

                    if (mMapType == MapType.Terrain)
                    {
                        worldPosition.y = Terrain.activeTerrain.SampleHeight(worldPosition);
                    }

                    nodes[x, y] = new Node(worldPosition, !Physics.CheckSphere(worldPosition, nodeRadius, unwalkable), x, y, nodetype);
                }
            }
            GenerateNodeType(nodes);
            CheckSlopeAngle(nodes);
        }

        void GenerateNodeType(Node[,] nodes)
        {
            float[] heights = new float[gridTotalRows * gridTotalCols];
            int i = 0;
            float sum = 0.0f;
            foreach (Node n in nodes)
            {
                heights[i] = n.mWorldPosition.y;
                if (heights[i] > MaxHeight)
                    MaxHeight = heights[i];

                sum += heights[i];
            }


            foreach (Node n in nodes)
            {
                if (n.mWorldPosition.y > MaxHeight - (sum / heights.Length) && n.mWorldPosition.y <= MaxHeight)
                {
                    n.mNodeType = NodeType.HIGHALTITUDE;
                }

                else if (n.mWorldPosition.y > MaxHeight - 2 * (sum / heights.Length) && n.mWorldPosition.y <= MaxHeight)
                {
                    n.mNodeType = NodeType.MIDALTITUDE;
                }

                else if (n.mWorldPosition.y > MaxHeight - 3.0f * (sum / heights.Length) && n.mWorldPosition.y <= MaxHeight - (sum / heights.Length))
                {
                    n.mNodeType = NodeType.MIDALTITUDE;
                }

                else if (n.mWorldPosition.y > 0 && n.mWorldPosition.y <= MaxHeight - 3.0f * (sum / heights.Length))
                {
                    n.mNodeType = NodeType.LOWALTITUDE;
                }

                else
                    n.mNodeType = NodeType.OPEN;
            }

        }

        void CheckSlopeAngle(Node[,] nodes)
        {
            foreach (Node n in nodes)
            {
                if (!n.mIsWalkable)
                    continue;

                foreach (Node neighbour in GenerateNeighBors(n))
                {
                    if (!neighbour.mIsWalkable)
                        continue;

                    float pDist = neighbour.mWorldPosition.y;
                    float hDist = (n.mWorldPosition - neighbour.mWorldPosition).magnitude;

                    float sine = Mathf.Asin(pDist / hDist);
                    float angle = sine * 180.0f / Mathf.PI;

                    if (angle > MaxSlopeForUnit)
                        neighbour.mIsWalkable = false;
                }
            }
        }

        public Node NodeFromWoldPos(Vector3 nodePos)
        {
            int X = 0, Y = 0;

            if (mMapType == MapType.Plane)
            {
                X = Mathf.RoundToInt(nodePos.x + gridTotalCols / 2);
                Y = Mathf.RoundToInt(nodePos.z + gridTotalRows / 2);
            }

            else
            {
                X = Mathf.RoundToInt(nodePos.x);
                Y = Mathf.RoundToInt(nodePos.z);
            }

            if (X < 0 || X > gridTotalCols - 1 || Y < 0 || Y > gridTotalRows - 1)
                return null;

            return nodes[X, Y];
        }

        public List<Node> GenerateNeighBors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int cGridX = node.gridX + x;
                    int cGridY = node.gridY + y;

                    if (cGridX >= 0 && cGridX < gridTotalCols && cGridY >= 0 && cGridY < gridTotalRows)
                        neighbors.Add(nodes[cGridX, cGridY]);
                }
            }

            return neighbors;
        }

        void OnDrawGizmos()
        {
            if (VisualizePath)
            {
                if (mMapType == MapType.Plane)
                    Gizmos.DrawWireCube(transform.position, new Vector3(gridTotalCols, 1, gridTotalRows));
                else
                    Gizmos.DrawWireCube(new Vector3(transform.position.x + gridTotalCols / 2, transform.position.y, transform.position.z + gridTotalRows / 2), new Vector3(gridTotalCols, 1, gridTotalRows));

                if (nodes != null)
                {
                    for (int x = 0; x < gridTotalCols; x++)
                    {
                        for (int y = 0; y < gridTotalRows; y++)
                        {
                            Gizmos.color = (nodes[x, y].mIsWalkable) ? Color.white : Color.red;

                            if (nodes[x, y].mNodeType == NodeType.LOWALTITUDE)
                                Gizmos.color = LowAltNodeColor;
                            if (nodes[x, y].mNodeType == NodeType.MIDALTITUDE)
                                Gizmos.color = MidAltNodeColor;
                            if (nodes[x, y].mNodeType == NodeType.HIGHALTITUDE)
                                Gizmos.color = HighAltNodeColor;

                            if (pathFound != null)
                            {
                                if (pathFound.Contains(nodes[x, y]))
                                {
                                    Gizmos.color = Color.black;
                                }
                            }
                            Gizmos.DrawCube(nodes[x, y].mWorldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
                }
            }
        }
    }
}

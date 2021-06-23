using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Mikealpha.AI.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        List<Node> openSet;
        HashSet<Node> closedSet;

        Grid grid;
        Node currentNode;
        Node StartNode, EndNode;

        PathRequestManager requestManager;

        void Awake()
        {
            grid = GetComponent<Grid>();
            requestManager = GetComponent<PathRequestManager>();
        }


        public void StartFindPath(Vector3 StartPos, Vector3 EndPos)
        {
            StartCoroutine(FindPath(StartPos, EndPos));
        }

        IEnumerator FindPath(Vector3 srcPos, Vector3 dstPos)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool success = false;

            StartNode = grid.NodeFromWoldPos(srcPos);
            EndNode = grid.NodeFromWoldPos(dstPos);

            if (StartNode == null || EndNode == null)
                yield return null;

            openSet = new List<Node>();
            closedSet = new HashSet<Node>();

            openSet.Add(StartNode);
            openSet[0].gCost = 0;
            openSet[0].hCost = movementCost(StartNode, EndNode);


            while (openSet.Count > 0)
            {
                currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost <= currentNode.fCost)
                        currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == EndNode)
                {
                    sw.Stop();
                    success = true;
                    break;
                }

                foreach (Node n in grid.GenerateNeighBors(currentNode))
                {
                    if (!n.mIsWalkable)
                        continue;

                    int cost = currentNode.gCost + movementCost(currentNode, n);
                    if (openSet.Contains(n) && cost < n.gCost)
                        openSet.Remove(n);
                    if (closedSet.Contains(n) && cost < n.gCost)
                        closedSet.Remove(n);
                    if (!closedSet.Contains(n) && !openSet.Contains(n))
                    {
                        n.gCost = cost;
                        n.hCost = movementCost(n, EndNode);
                        n.Parent = currentNode;
                        openSet.Add(n);
                    }
                }
            }
            yield return null;
            if (success)
            {
                waypoints = RetracePath();
            }
            requestManager.FinishedProcessingPath(waypoints, success);

        }

        public Vector3[] RetracePath()
        {
            List<Node> path = new List<Node>();
            currentNode = EndNode;

            while (currentNode != StartNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            Vector3[] waypoints = FilterPath(path);
            Array.Reverse(waypoints);
            return waypoints;

        }

        Vector3[] FilterPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 dirOld;

            dirOld = Vector3.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 dirNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (dirNew != dirOld)
                    waypoints.Add(path[i].mWorldPosition);

                dirOld = dirNew;
            }

            return waypoints.ToArray();
        }

        int movementCost(Node startNode, Node endNode)
        {
            int dstX = Mathf.Abs(startNode.gridX - endNode.gridX);
            int dstY = Mathf.Abs(startNode.gridY - endNode.gridY);


            //Check with the penalty calculation later
            if (dstX < dstY)
                return 14 * dstX + 10 * (dstY - dstX) + endNode.tCost + endNode.movementPenalty;
            return 14 * dstY + 10 * (dstX - dstY) + endNode.tCost + endNode.movementPenalty;
        }
    }
}
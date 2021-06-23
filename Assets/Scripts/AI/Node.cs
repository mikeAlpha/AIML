using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikealpha.AI.Pathfinding
{
    public enum NodeType
    {
        OPEN = 0,
        LOWALTITUDE = 5,
        MIDALTITUDE = 10,
        HIGHALTITUDE = 15
    }

    public class Node
    {
        public Vector3 mWorldPosition;
        public bool mIsWalkable;

        public int gridX;
        public int gridY;

        public NodeType mNodeType;
        public Node Parent;

        public int movementPenalty = 10;

        public int gCost;
        public int hCost;
        public int tCost
        {
            get { return ((int)mNodeType + Mathf.RoundToInt(mWorldPosition.y)); }
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public Node(Vector3 worldPosition, bool IsWalkable, int gridX, int gridY, NodeType nodeType)
        {
            this.mWorldPosition = worldPosition;
            this.mIsWalkable = IsWalkable;
            this.gridX = gridX;
            this.gridY = gridY;
            this.mNodeType = nodeType;
        }
    }
}

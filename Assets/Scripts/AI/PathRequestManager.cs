using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikealpha.AI.Pathfinding
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> Requests = new Queue<PathRequest>();
        PathRequest currentPathRequest;
        public static PathRequestManager pInstance;
        bool IsProcessingPath = false;
        Pathfinding mPathfinding;

        void Awake()
        {
            pInstance = this;
            mPathfinding = GetComponent<Pathfinding>();
        }

        class PathRequest
        {
            public Vector3 mStartPos;
            public Vector3 mEndPos;
            public Action<Vector3[], bool> mCallback;

            public PathRequest(Vector3 StartPos, Vector3 EndPos, Action<Vector3[], bool> Callback)
            {
                mStartPos = StartPos;
                mEndPos = EndPos;
                mCallback = Callback;
            }
        }

        public static void RequestPath(Vector3 StartPos, Vector3 EndPos, Action<Vector3[], bool> Callback)
        {
            PathRequest newPathRequest = new PathRequest(StartPos, EndPos, Callback);
            pInstance.Requests.Enqueue(newPathRequest);
            pInstance.ProcessNext();
        }

        void ProcessNext()
        {
            if (!IsProcessingPath && Requests.Count > 0)
            {
                currentPathRequest = Requests.Dequeue();
                IsProcessingPath = true;
                mPathfinding.StartFindPath(currentPathRequest.mStartPos, currentPathRequest.mEndPos);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            pInstance.currentPathRequest.mCallback(path, success);
            IsProcessingPath = false;
            pInstance.ProcessNext();
        }
    }
}

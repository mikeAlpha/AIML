using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mikealpha.AI.Editor {
    public class Connection
    {
        public ConnectorPoint InPoint;
        public ConnectorPoint OutPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectorPoint InPoint, ConnectorPoint OutPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.InPoint = InPoint;
            this.OutPoint = OutPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(InPoint.rect.center, OutPoint.rect.center, InPoint.rect.center + Vector2.left * 50f, OutPoint.rect.center - Vector2.left * 50f, Color.white, null, 2f);
            if (Handles.Button((InPoint.rect.center + OutPoint.rect.center), Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}

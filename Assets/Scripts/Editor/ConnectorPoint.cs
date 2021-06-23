using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mikealpha.AI.Editor { 
public enum ConnectorType { IN,OUT }

    public class ConnectorPoint
    {
        public Node node;

        public ConnectorType type;

        public Rect rect;

        public GUIStyle style;

        public Action<ConnectorPoint> OnConnectorClick;

        public ConnectorPoint(Node node, ConnectorType type, GUIStyle style, Action<ConnectorPoint> OnConnectorClick)
        {
            this.node = node;
            this.type = type;
            this.style = style;
            this.OnConnectorClick = OnConnectorClick;
            rect = new Rect(0, 0, 10, 10);
        }

        public void Draw()
        {
            rect.x = node.rect.x + (node.rect.width * 0.5f) - rect.height * 0.5f;
            switch (type)
            {
                case ConnectorType.IN:
                    rect.y = node.rect.y - node.rect.height + 45f;
                    break;

                case ConnectorType.OUT:
                    rect.y = node.rect.y + node.rect.height - 8f;
                    break;
            }

            if (GUI.Button(rect, "", style))
            {
                if (OnConnectorClick != null)
                {
                    OnConnectorClick(this);
                }
            }

        }
    }
}

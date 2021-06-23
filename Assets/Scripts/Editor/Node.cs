using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mikealpha.AI.Editor {
    public class Node
    {
        public Rect rect;
        public string title;

        public GUIStyle style;

        private bool IsDragged = false;

        public ConnectorPoint InPoint;
        public ConnectorPoint OutPoint;

        bool IsSelected = false;

        Action<Node> RemoveNode;

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, Action<ConnectorPoint> OnClickInPoint, Action<ConnectorPoint> OnClickOutPoint, Action<Node> RemoveNode)
        {
            rect = new Rect(position.x, position.y, width, height);

            style = nodeStyle;
            InPoint = new ConnectorPoint(this, ConnectorType.IN, style, OnClickInPoint);
            OutPoint = new ConnectorPoint(this, ConnectorType.OUT, style, OnClickOutPoint);
            this.RemoveNode = RemoveNode;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            InPoint.Draw();
            OutPoint.Draw();
            GUI.Box(rect, title, style);
            string tex = EditorGUI.TextField(new Rect(rect.center.x - 5, rect.center.y - 5, 30, 20), "Here");
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            IsDragged = true;
                            IsSelected = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                            IsSelected = false;
                        }
                    }

                    if (e.button == 1 && IsSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }

                    break;

                case EventType.MouseUp:
                    IsDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && IsDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        void ProcessContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove Node"), false, OnRemoveNode);
            menu.ShowAsContext();
        }

        void OnRemoveNode()
        {
            if (RemoveNode != null)
            {
                RemoveNode(this);
            }
        }
    }
}

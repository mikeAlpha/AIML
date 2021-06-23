using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Mikealpha.AI.Editor {
    public class FSM : EditorWindow
    {

        private List<Node> nodes;

        private GUIStyle nodeStyle;
        private GUIStyle InPointStyle;
        private GUIStyle OutPointStyle;

        ConnectorPoint InPointSelected;
        ConnectorPoint OutPointSelected;

        List<Connection> connections;

        private Vector2 drag;

        [MenuItem("Arte De MikeAlpha/Game Tools/FSM")]
        public static void ShowWindow()
        {
            FSM window = GetWindow<FSM>();
            window.titleContent = new GUIContent("FSM");
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnGUI()
        {
            DrawNodes();
            DrawConnections();

            DrawNodeConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool gChanged = nodes[i].ProcessEvents(e);
                    if (gChanged)
                        GUI.changed = true;
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnMouseDrag(e.delta);
                    }
                    break;

            }
        }

        private void OnMouseDrag(Vector2 delta)
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        void DrawNodeConnectionLine(Event e)
        {
            if (InPointSelected != null && OutPointSelected == null)
            {
                Handles.DrawBezier(InPointSelected.rect.center, e.mousePosition, InPointSelected.rect.center + Vector2.left * 50f, e.mousePosition - Vector2.left * 50f, Color.white, null, 2f);
                GUI.changed = true;
            }

            if (OutPointSelected != null && InPointSelected == null)
            {
                Handles.DrawBezier(OutPointSelected.rect.center, e.mousePosition, OutPointSelected.rect.center + Vector2.left * 50f, e.mousePosition - Vector2.left * 50f, Color.white, null, 2f);
                GUI.changed = true;
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(mousePosition));
            menu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
                nodes = new List<Node>();
            nodes.Add(new Node(mousePosition, 200, 50, nodeStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
        }

        void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> ConnToBeRemoved = new List<Connection>();
                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i].InPoint == node.InPoint || connections[i].OutPoint == node.OutPoint)
                    {
                        ConnToBeRemoved.Add(connections[i]);
                    }
                }

                for (int i = 0; i < ConnToBeRemoved.Count; i++)
                {
                    connections.Remove(ConnToBeRemoved[i]);
                }

                ConnToBeRemoved = null;
            }
            nodes.Remove(node);
        }

        void OnClickInPoint(ConnectorPoint point)
        {
            InPointSelected = point;
            if (OutPointSelected != null)
            {
                if (OutPointSelected.node != InPointSelected.node)
                {
                    CreateConnection();
                    ClearConnection();
                }
                else
                    ClearConnection();
            }
        }

        void OnClickOutPoint(ConnectorPoint point)
        {
            OutPointSelected = point;
            if (InPointSelected != null)
            {
                if (InPointSelected.node != OutPointSelected.node)
                {
                    CreateConnection();
                    ClearConnection();
                }
                else
                    ClearConnection();
            }
        }

        void CreateConnection()
        {
            if (connections == null)
                connections = new List<Connection>();
            connections.Add(new Connection(InPointSelected, OutPointSelected, RemoveConnection));
        }

        void ClearConnection()
        {
            InPointSelected = null;
            OutPointSelected = null;
        }

        void RemoveConnection(Connection conn)
        {
            connections.Remove(conn);
        }
    }
}

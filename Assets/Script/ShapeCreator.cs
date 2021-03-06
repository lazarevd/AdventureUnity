﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor;

public class ShapeCreator : MonoBehaviour

{
    [HideInInspector]
    public List<Vector2> points = new List<Vector2>();
    private Dictionary<string, GraphNode> nodes;
    private Dictionary<string, GraphEdge> edges;
    private Dictionary<string, GraphPolygon4> polygons;
    private Dictionary<string, GraphPointOnEdge> poes;
    [HideInInspector]
    public bool NODE = true;
    [HideInInspector]
    public bool POLY = true;

    [HideInInspector]
    public bool needsRepaint;
    [HideInInspector]
    public Tool curTool;



    public enum ToolDisplayStatus { HOVERNODEPOLYGON, HOVERPOLYGON, HOVERPOE, HOVERNODE, HOVEREDGE, NORMAL };

    ToolDisplayStatus toolDisplayStatus = ToolDisplayStatus.NORMAL;

    public ShapeCreator()
    {
        Debug.Log("Construct ShapeCreator");
        nodes = new Dictionary<string, GraphNode>();
        edges = new Dictionary<string, GraphEdge>();
        polygons = new Dictionary<string, GraphPolygon4>();
        poes = new Dictionary<string, GraphPointOnEdge>();

    }


    public void render()
    {
        if (ToolBox.processTool(curTool))
        {//Если ToolBox вернул true, значит инструмент завершил работу и его можно удалить
            curTool = null;
        }

        Event guiEvent = Event.current;
        /*
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Vector2 pos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
            GraphNode grnod = new GraphNode(mouseRay.origin.x, mouseRay.origin.y, 5);
            shapeCreator.addNode(grnod);
            Debug.Log("add: " + pos);
            needsRepaint = true;
        }

        if (shapeCreator.points.Count > 4)
        {
            shapeCreator.points.Clear();
        }
        */
        renderNodes();
        renderEdges();
        renderPolygons();

        if (needsRepaint)
        {
            HandleUtility.Repaint();
            needsRepaint = false;
        }

        if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

    public void OnGUI()
    {
       
    }

    private void renderNodes()
    {
        foreach (KeyValuePair<string, GraphNode> gn in getNodes())
        {
            Handles.DrawSolidDisc(new Vector2(gn.Value.getX(), gn.Value.getY()), Vector3.forward, .3f);
        }
    }


    private void renderEdges()
    {
        Handles.color = Color.white;
        foreach (KeyValuePair<string, GraphEdge> ge in getEdges())
        {
            GraphNode[] gns = new GraphNode[2];
            gns[0] = getNodeByName(ge.Value.getNodes()[0]);
            gns[1] = getNodeByName(ge.Value.getNodes()[1]);
            float x1 = gns[0].getX();
            float y1 = gns[0].getY();
            float x2 = gns[1].getX();
            float y2 = gns[1].getY();
            Handles.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2));
        }
    }


    private void renderPolygons()
    {

        foreach (KeyValuePair<string, GraphPolygon4> gp in getPolygons())
        {


            for (int i = gp.Value.getVertices().Length - 4; i >= 0; i = i - 2)
            {
                Handles.color = Color.magenta;
                Handles.DrawLine(new Vector2(gp.Value.getVertices()[i], gp.Value.getVertices()[i + 1]), new Vector2(gp.Value.getVertices()[i + 2], gp.Value.getVertices()[i + 3]));
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(new Vector2(gp.Value.getVertices()[i], gp.Value.getVertices()[i + 1]), Vector3.forward, .1f);
            }
            Handles.color = Color.magenta;
            Handles.DrawLine(new Vector2(gp.Value.getVertices()[gp.Value.getVertices().Length - 2], gp.Value.getVertices()[gp.Value.getVertices().Length - 1]), new Vector2(gp.Value.getVertices()[0], gp.Value.getVertices()[1]));
            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(new Vector2(gp.Value.getVertices()[gp.Value.getVertices().Length - 2], gp.Value.getVertices()[gp.Value.getVertices().Length - 1]), Vector3.forward, .1f);
            Handles.color = Color.red;
            Handles.DrawSolidDisc(gp.Value.getCentreOf4Poly(), Vector3.forward, .1f);

            Vector2[] midPoints = gp.Value.getMiddlePoints();
            for (int i = 0; i < midPoints.Length; i ++)
            {
                Handles.color = Color.white;
                Handles.DrawSolidDisc(midPoints[i], Vector3.forward, .05f);
            }

            /*
           [0 1 2 3 4 5 6 7]
            1 2 3 4 5 6 7 8
            */
        }
    }



    public Vector2 getMouseRay()
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector2 pos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
        //Debug.Log("getMouseRay: " + pos);
        return pos;
    }

    public Vector2 getMouseScreen()
    {
        Vector2 pos = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);
        //Debug.Log("getMouseScreen: " + pos);
        return pos;
    }

    public ToolDisplayStatus getToolDisplayStatus()
    {
        return this.toolDisplayStatus;
    }

    public void setToolDisplayStatus(ToolDisplayStatus displayMode)
    {
        this.toolDisplayStatus = displayMode;
    }

    public void OnPostRender()
    {
        
    }


    public void SaveGraph()
    {
        return;
    }

    public void LoadGraph()
    {
        return;
    }



    /*
    for (NodePOESource nps : graphSource.getNodesPOESource()) {
        addNode();
        pointsOnEdges.add(selectedPolygons.get(0).addPointOnClosestEdge(selectedCoords.get(0).x, selectedCoords.get(0).y));//На основании ранее отселекченных координат добавляем
        pointsOnEdges.add(selectedPolygons.get(1).addPointOnClosestEdge(selectedCoords.get(1).x, selectedCoords.get(1).y));//точки на ребра (poe) на отселекченные полигоны. сразу добавлем их в список poe
        this.addNode(new NodeConnector(pointsOnEdges, true));

    }

    */



    public string getId<T>()
    {
        return null;
    }


    public GraphNode getNodeByName(string nodName) 
    {
        return nodes[nodName];
    }


    public GraphPointOnEdge getPOEById(string name)
    {
        return this.poes[name];
    }

    public Dictionary<string, GraphPointOnEdge> getPOEs()
    {
        return this.poes;
    }


    public Dictionary<string, GraphNode> getNodes()
    {
        return this.nodes;
    }


    public string getNodeKey(GraphNode node)
    {

        foreach (KeyValuePair<string, GraphNode> entry in nodes)
        {
            if (node.Equals(entry.Value))
            {
                return entry.Key;
            }
        }
        return null;
    }

    public Dictionary<string, GraphPolygon4> getPolygons()
    {

        return this.polygons;
    }


    public string getEdgeKey(GraphEdge edge)
    {

        foreach (KeyValuePair<string, GraphEdge> entry in edges)
        {

            if (entry.Equals(edge))
            {
                return entry.Key;
            }
        }


        return null;
    }


    public Dictionary<string, GraphEdge> getEdges()
    {
        return this.edges;
    }

    public void addNode(GraphNode node)
    {
        string name = getNewNodeName();
        this.nodes[name] = node;
        Debug.Log("addNode:" + node);
    }

    public void addNode(string name, GraphNode node)
    {
        this.nodes[name] = node;
    }





    public void addEdge(string name, GraphEdge edge)
    {
        if (edge != null)
        {
            foreach (KeyValuePair<string, GraphEdge> entry in this.edges)
            {
                try
                {
                    string sn1 = edge.getNodes()[0];
                    string sn2 = edge.getNodes()[1];
                    string dn1 = entry.Value.getNodes()[0];
                    string dn2 = entry.Value.getNodes()[1];
                    if ((sn1.Equals(dn1) && sn2.Equals(dn2)) || (sn2.Equals(dn1) && sn1.Equals(dn2)))
                    {
                        return;//Check each edge. Is there edge with same nodes. If is - then return;
                    }

                }
                catch (NullReferenceException)
                {
                    Debug.Log("error adding edge");
                }
            }
            this.edges[name] = edge;
        }

    }

    public void addPolygon(string name, GraphPolygon4 polygon)
    {
        if (polygon != null)
        {
            Debug.Log("Add poly:" + polygon.getVertices()[0]+":"+ polygon.getVertices()[1]);
            this.polygons[name] = polygon;
        }
    }


    public void deleteNode(string name)
    {
        GraphNode procNode = nodes[name];
        procNode.clearLinked();
        nodes.Remove(name);
        refreshNeighbours();
    }


    public void deleteEdge(string name)
    {
        edges.Remove(name);
        refreshNeighbours();
    }




    public void refreshNeighbours()
    {

        fillNeighbours();

    }





    public void fillNeighbours()
    {

        foreach (KeyValuePair<string, GraphNode> entry in nodes)
        {


            foreach (GraphEdge edg in edges.Values)
            {
                if (entry.Key.Equals(edg.getNodes()[0]))
                {//if first node exist in edge, then second is neigbout
                    entry.Value.addNeighbour(edg.getNodes()[1]);
                }
                else if (entry.Key.Equals(edg.getNodes()[1]))
                {//  and vice versa 
                    entry.Value.addNeighbour(edg.getNodes()[0]);
                }
            }

        }


    }


    public string getNewNodeName()
    {
        string newName = null;

        newName = "node" + (getLastNodeNameNumber() + 1);


        return newName;
    }



        public static int getLastId<T>(Dictionary<string, T> dict)
        {

            int curInt = 0;

            if (dict.GetType().Equals(typeof(Dictionary<string, T>)))
            {
                string regExp = "[0-9]+$";

                foreach (string name in dict.Keys)
                {
                    Regex regex = new Regex(regExp);

                    string[] matches = Regex.Matches(name, regExp)
                    .OfType<Match>()
                    .Select(m => m.Value)
                    .ToArray();
                        foreach (string mat in matches)
                        {
                            try
                            {
                                curInt = Int32.Parse(mat);
                            }
                            catch (FormatException e)
                            {
                                Debug.Log(e.Message);
                            }
                        }
                     
                }
            }
            return curInt;

        }





    public int getLastNodeNameNumber()
    {
        return ShapeCreator.getLastId<GraphNode>(nodes);
    }

    public int getLastPOENameNumber()
    {
        return ShapeCreator.getLastId<GraphPointOnEdge>(poes);
    }

    public int getLastEdgeNameNumber()
    {
        return ShapeCreator.getLastId<GraphEdge>(edges);
    }

    public int getLastPolyNameNumber()
    {
        return ShapeCreator.getLastId<GraphPolygon4>(polygons);
    }



    public string getNewEdgeName()
    {
        string newName = null;

        newName = "edge" + (getLastEdgeNameNumber() + 1);


        return newName;
    }


    public string getNewPolyName()
    {
        string newName = null;

        newName = "poly" + (getLastPolyNameNumber() + 1);


        return newName;
    }



    public string printNodes(List<string> nodeList)
    {

        string neighbours = "";


        foreach (string node in nodeList)
        {
            neighbours = neighbours + " " + node.ToString();
        }

        return "[" + neighbours + "]";
    }


}

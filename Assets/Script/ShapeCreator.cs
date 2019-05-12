using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class ShapeCreator : MonoBehaviour
{
    [HideInInspector]
    public List<Vector2> points = new List<Vector2>();
    private Dictionary<string, GraphNode> nodes;
    private Dictionary<string, GraphEdge> edges;
    private Dictionary<string, GraphPolygon4> polygons;
    private Dictionary<string, GraphPointOnEdge> poes;


    public ShapeCreator()
    {
        Debug.Log("Construct ShapeCreator");
        nodes = new Dictionary<string, GraphNode>();
        edges = new Dictionary<string, GraphEdge>();
        polygons = new Dictionary<string, GraphPolygon4>();
        poes = new Dictionary<string, GraphPointOnEdge>();
    }

    /*
    graphSource.addNode(200,200, "n1");		
    graphSource.addNode(400,250, "n2");
    graphSource.addEdge("n1", "n2", "e2");
    float[] polyFloat = {100,100,200,100,200,200,100,200};
    float[] polyFloat2 = {150,200,250,200,250,300,150,300};
    float[] polyFloat3 = {200,300,300,300,300,400,200,400};
    float[] polyFloat4 = {250,400,350,400,350,500,250,500};
    graphSource.addPoly(polyFloat, "poly1");
    graphSource.addPoly(polyFloat2, "poly2");
    graphSource.addPoly(polyFloat3, "poly3");
    graphSource.addPoly(polyFloat4, "poly4");
    loadGraph();
*/

    void Awake()
    {
        Debug.Log("Awake ShapeCreator");

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
        Debug.Log("Get Nodes " + this.nodes.GetType());
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
                catch (NullReferenceException nex)
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


    static class GenericGetter<T>
    {
        public static int getLastId(Dictionary<string, T> dict)
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

    }




    public int getLastNodeNameNumber()
    {
        return ShapeCreator.GenericGetter<GraphNode>.getLastId(nodes);
    }

    public int getLastPOENameNumber()
    {
        return ShapeCreator.GenericGetter<GraphPointOnEdge>.getLastId(poes);
    }

    public int getLastEdgeNameNumber()
    {
        return ShapeCreator.GenericGetter<GraphEdge>.getLastId(edges);
    }

    public int getLastPolyNameNumber()
    {
        return ShapeCreator.GenericGetter<GraphPolygon4>.getLastId(polygons);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPointOnEdge
{
    [System.NonSerialized]
    ShapeCreator shapeCreator;
    [System.NonSerialized]
    private Vector2 position;
    [System.NonSerialized]
    private string childNode;
    private int parentEdge;
    private float edgePosition;
    private string parentPoly;

    private GraphPointOnEdge() { }

    public GraphPointOnEdge(string poly, int parentEdge, float k)
    {

        GameObject thePlayer = GameObject.Find("shapeCreator");
        shapeCreator = thePlayer.GetComponent<ShapeCreator>();
        this.parentEdge = parentEdge;
        this.position = new Vector2();
        this.edgePosition = k;
        this.parentPoly = poly;
    }

    public string getThisId()
    {
        string ret = null;

        foreach (KeyValuePair<string, GraphPointOnEdge> entry in shapeCreator.getPOEs())
        {
            if (entry.Value.Equals(this))
            {
                ret = entry.Key;
                break;
            }
        }

        return ret;
    }

    public void setChildNode(string name)
    {
        this.childNode = name;
    }


    public string getChildNode()
    {
        return this.childNode;
    }


    public GraphPointOnEdge(string poly, int parentEdge, float k, GraphNode node)
    {
        this.parentEdge = parentEdge;
        this.position = new Vector2();
        this.edgePosition = k;
        this.parentPoly = poly;
    }


    public int getParentEdge()
    {
        return this.parentEdge;
    }

    public string getParentPolygon()
    {
        return this.parentPoly;
    }


    public float getEdgePosition()
    {
        return this.edgePosition;
    }


    public Vector2 getPosition()
    {
        return this.position;
    }

    public void setPosition(Vector2 pos)
    {
        this.position = pos;
    }


    public void setPointPosition()
    {
        int edge = this.parentEdge;
        float length = this.edgePosition;
        Vector2[] edVec = new Vector2[2];
        Vector2 tmp;

        edVec = shapeCreator.getPolygons()[this.parentPoly].getEdge(edge);
        tmp = new Vector2(edVec[1].x - edVec[0].x, edVec[1].y - edVec[0].y);
        tmp = MathGame.scl(tmp, length);
        tmp.x = tmp.x + edVec[0].x;
        tmp.y = tmp.y + edVec[0].y;
        setPosition(tmp);
    }
}

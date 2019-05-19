using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNodePoly : Tool
{

    ShapeCreator shapeCreator;
    GraphPolygon4 currentPoly;
    int currentVertex;

    public MoveNodePoly()
    {
        GameObject go = GameObject.Find("shapeCreator");
        this.shapeCreator = go.GetComponent<ShapeCreator>();
        shapeCreator = go.GetComponent<ShapeCreator>();
    }



    private ToolStatus toolStat;


    public ToolStatus getStatus()
{
    return toolStat;
}

    public void setStatus(ToolStatus toolStatus)
{
    toolStat = toolStatus;
}


    public void prepare()
{
    toolStat = ToolStatus.PROCESSING;
}


    public void select()
{
    //Пропускаем, двигаем по одному все
}


    public void process()
{

        //Debug.Log("MoveNodePoly process");

    if (shapeCreator.NODE == true && shapeCreator.POLY == true)
    {
        movePolygonVertex();
        moveNode();
        shapeCreator.needsRepaint = true;
    }
    else if (shapeCreator.NODE == true)
    {
        moveNode();
    }
    else if (shapeCreator.POLY == true)
    {
        movePolygonVertex();
    }

}


    public void finish()
{
    toolStat = ToolStatus.FINISHED;
    shapeCreator.setToolDisplayStatus(ShapeCreator.ToolDisplayStatus.NORMAL);
}




public void movePolygonVertex()
{


    Vector2 touchPos = shapeCreator.getMouseRay();
    Event guiEvent = Event.current;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0) {//chose poly
            //Debug.Log("movePolygonVertex");

            if (shapeCreator.POLY == true)
            {
            List<GraphPolygon4> polys = new List<GraphPolygon4>();

            foreach (KeyValuePair<string, GraphPolygon4> entry in shapeCreator.getPolygons())
            {
                polys.Add(entry.Value);
            }
            GraphPolygon4 curPolygon, movePolygon;
            int moveVertex;  //IDs of vertex in poly
            int curVertex;
            float curDistance;
            float distance;
            float theDistance;

                if (currentPoly == null)
                {
                    if (polys.Count > 0)
                    {
                        curPolygon = polys[polys.Count - 1];// Test only
                        movePolygon = curPolygon;
                        moveVertex = 1;
                        theDistance = curPolygon.getDistanceToVertex(1, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y);
                        curDistance = 0;

                        foreach (GraphPolygon4 pol in polys)
                        {//Loop polys
                            curVertex = 1;
                            curDistance = pol.getDistanceToVertex(1, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y);
                            for (int i = 1; i <= 4; i++)
                            {
                                distance = pol.getDistanceToVertex(i, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y);
                                if (distance < curDistance)
                                {
                                    curVertex = i;
                                    curDistance = distance;
                                }
                            }
                            if (curDistance < theDistance)
                            {
                                curPolygon = pol;
                                theDistance = curDistance;
                                moveVertex = curVertex;

                            }
                            movePolygon = curPolygon;

                        }
                        currentPoly = movePolygon;//set working poly
                        currentVertex = moveVertex;
                    //Debug.Log("the distance " + movePolygon.getDistanceToVertex(moveVertex, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y));

                    }
                }
            }
        }

            if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
        {
            if (currentPoly != null && currentVertex != 0)
            {
                if (currentPoly.getDistanceToVertex(currentVertex, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y) < 20)
                {
                    currentPoly.setVertexXY(currentVertex, shapeCreator.getMouseRay().x, shapeCreator.getMouseRay().y);
                }
            }
        }

            if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
        {
            currentPoly = null;
            currentVertex = 0;
        }



    }



public void moveNode()
{
        Event guiEvent = Event.current;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
    {
            Vector2 touchPos = shapeCreator.getMouseRay();
            if (shapeCreator.NODE == true)
        {
            GraphNode moveNode;
            List<GraphNode> nodes = new List<GraphNode>();

            foreach (KeyValuePair<string, GraphNode> entry in shapeCreator.getNodes())
            {
                nodes.Add(entry.Value);
            }

            if (nodes.Count > 0)
            {
                moveNode = nodes[nodes.Count - 1];
                foreach (GraphNode nod in nodes)
                {
                        
                    if (nod.getDistance(touchPos.x, shapeCreator.getMouseScreen().y) < moveNode.getDistance(shapeCreator.getMouseScreen().x, shapeCreator.getMouseScreen().y))
                    {
                        moveNode = nod;
                    }
                }
                if (moveNode.getDistance(shapeCreator.getMouseScreen().x, shapeCreator.getMouseScreen().y) < 20)
                {
                    moveNode.setX(shapeCreator.getMouseScreen().x);
                    moveNode.setY(shapeCreator.getMouseScreen().y);
                }

            }

        }





    }
}


}


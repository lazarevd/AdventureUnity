using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNodePoly : Tool
{

    CoordHolder coordHolder;
    ShapeCreator shapeCreator;

    public MoveNodePoly()
    {
        GameObject go = GameObject.Find("shapeCreator");
        this.shapeCreator = go.GetComponent<ShapeCreator>();
        coordHolder = go.GetComponent<CoordHolder>();
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

        Debug.Log("MoveNodePoly process");

    if (shapeCreator.NODE == true && shapeCreator.POLY == true)
    {
        movePolygonVertex();
        moveNode();
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

    Vector2 touchPos = coordHolder.getMouseRay();


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
            float nodDistance;

            if (polys.Count > 0)
            {

                curPolygon = polys[polys.Count - 1];// Test only
                movePolygon = curPolygon;
                moveVertex = 1;
                nodDistance = curPolygon.getDistanceToVertex(1, coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y);

                curDistance = 0;
                foreach (GraphPolygon4 pol in polys)
                {//Loop polys

                    curVertex = 1;
                    curDistance = pol.getDistanceToVertex(1, coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y);


                    for (int i = 1; i <= 4; i++)
                    {
                        distance = pol.getDistanceToVertex(i, coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y);
                        if (distance < curDistance)
                        {
                            curVertex = i;
                            curDistance = distance;
                        }
                    }
                    if (curDistance < nodDistance)
                    {
                        curPolygon = pol;
                        nodDistance = curDistance;
                        moveVertex = curVertex;

                    }
                    movePolygon = curPolygon;
                }

                if (movePolygon.getDistanceToVertex(moveVertex, coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y) < 20)
                {
                    movePolygon.setVertexXY(moveVertex, coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y);
                    //RenderShapes.drawPoint(UI.getCursor(), 5, RenderShapes.Colour.RED);
                }
            }

        }

}



public void moveNode()
{
        Event guiEvent = Event.current;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
    {
            Vector2 touchPos = coordHolder.getMouseRay();
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
                        
                    if (nod.getDistance(touchPos.x, coordHolder.getMouseScreen().y) < moveNode.getDistance(coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y))
                    {
                        moveNode = nod;
                    }
                }
                if (moveNode.getDistance(coordHolder.getMouseScreen().x, coordHolder.getMouseScreen().y) < 20)
                {
                    moveNode.setX(coordHolder.getMouseScreen().x);
                    moveNode.setY(coordHolder.getMouseScreen().y);
                }

            }

        }





    }
}


}


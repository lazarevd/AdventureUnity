using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    bool needsRepaint;


    public Vector2 getMouseRay()
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector2 pos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
        Debug.Log("getMouseRay: " + pos);
        return pos;
    }

    public Vector2 getMouseScreen()
    {
        Vector2 pos = HandleUtility.GUIPointToScreenPixelCoordinate(Event.current.mousePosition);
        Debug.Log("getMouseRay: " + pos);
        return pos;
    }


    void OnSceneGUI()
    {
        Event guiEvent = Event.current;
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

        renderNodes();
        renderEdges();
        renderPolygons();

        if (needsRepaint)
        {
            HandleUtility.Repaint();
            needsRepaint = false;
        }

        if (guiEvent.type == EventType.Layout) {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

    private void renderNodes()
    {
        foreach (KeyValuePair<string, GraphNode> gn in shapeCreator.getNodes())
        {
            Handles.DrawSolidDisc(new Vector2(gn.Value.getX(), gn.Value.getY()), Vector3.forward, .3f);
        }
    }


    private void renderEdges()
    {
        Handles.color = Color.white;
        foreach (KeyValuePair<string, GraphEdge> ge in shapeCreator.getEdges())
        {
            float x1 = shapeCreator.getNodeByName(ge.Value.getNodes()[0]).getX();
            float y1 = shapeCreator.getNodeByName(ge.Value.getNodes()[0]).getY();
            float x2 = shapeCreator.getNodeByName(ge.Value.getNodes()[1]).getX();
            float y2 = shapeCreator.getNodeByName(ge.Value.getNodes()[1]).getY();
            Handles.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2));
        }
    }


    private void renderPolygons()
    {
        
        foreach (KeyValuePair<string, GraphPolygon4> gp in shapeCreator.getPolygons())
        {
            

            for (int i = gp.Value.getVertices().Length-4; i >= 0; i=i-2)
            {
                Debug.Log("dr poly: " + i);
                Handles.color = Color.magenta;
                Handles.DrawLine(new Vector2(gp.Value.getVertices()[i], gp.Value.getVertices()[i+1]), new Vector2(gp.Value.getVertices()[i+2], gp.Value.getVertices()[i+3]));
                Handles.color = Color.yellow;
                Handles.DrawSolidDisc(new Vector2(gp.Value.getVertices()[i], gp.Value.getVertices()[i + 1]), Vector3.forward, .1f);
            }
            Handles.color = Color.magenta;
            Handles.DrawLine(new Vector2(gp.Value.getVertices()[gp.Value.getVertices().Length-2], gp.Value.getVertices()[gp.Value.getVertices().Length - 1]), new Vector2(gp.Value.getVertices()[0], gp.Value.getVertices()[1]));
            Handles.color = Color.yellow;
            Handles.DrawSolidDisc(new Vector2(gp.Value.getVertices()[gp.Value.getVertices().Length - 2], gp.Value.getVertices()[gp.Value.getVertices().Length - 1]), Vector3.forward, .1f);
            /*
           [0 1 2 3 4 5 6 7]
            1 2 3 4 5 6 7 8
            */
        }
    }




    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Add two nodes"))
        {
            Debug.Log("hit Add two nodes");
            shapeCreator.addNode("n1", new GraphNode(-4.5f, 2.5f, 5));
            shapeCreator.addNode("n2", new GraphNode(1f, -2f, 5));
            shapeCreator.addEdge("e2", new GraphEdge("n1", "n2"));
        }

        if (GUILayout.Button("Add poly"))
        {
            Debug.Log("hit Add poly");
            float[] polyFloat = { -1.5f, 0f, -1.5f, -1.0f, 1.5f, 0f, 0f, 1.5f };
            shapeCreator.addPolygon("poly1", new GraphPolygon4(polyFloat));

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

    }

    void OnEnable()
    {
        //shapeCreator = target as ShapeCreator;
        GameObject thePlayer = GameObject.Find("shapeCreator");
        Debug.Log("thePlayer: " + thePlayer);
        this.shapeCreator = thePlayer.GetComponent<ShapeCreator>();
        Debug.Log("shapeCreator: " + shapeCreator);
    }
}

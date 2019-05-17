using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;






    void OnSceneGUI()
    {
        shapeCreator.render();
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

        if (GUILayout.Button("Move"))
        {
            ToolBox.stopTool(shapeCreator.curTool);
            if (shapeCreator.curTool == null)
            {
                shapeCreator.curTool = new MoveNodePoly();
                shapeCreator.curTool.prepare();
            }
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
        this.shapeCreator = thePlayer.GetComponent<ShapeCreator>();
    }
}

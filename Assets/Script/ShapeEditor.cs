using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    bool needsRepaint;

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Vector2 pos = new Vector2(mouseRay.origin.x, mouseRay.origin.y);
            shapeCreator.points.Add(pos);
            Debug.Log("add: " + pos);
            needsRepaint = true;

        }

        if (shapeCreator.points.Count > 4)
        {
            shapeCreator.points.Clear();
        }

        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            Handles.DrawSolidDisc(shapeCreator.points[i], Vector3.forward, .3f);
        }

        if (needsRepaint)
        {
            HandleUtility.Repaint();
            needsRepaint = false;
        }

        if (guiEvent.type == EventType.Layout) {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

     void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
    }
}

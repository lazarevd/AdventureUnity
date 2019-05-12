using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge
{

    ShapeCreator shapeCreator;

    private List<string> nodesToLink;

    private GraphEdge() {
        GameObject thePlayer = GameObject.Find("shapeCreator");
        shapeCreator = thePlayer.GetComponent<ShapeCreator>();
    }

    public GraphEdge(string n1, string n2) : this() 
    {

        if (n1.Equals(n2))
        {
            Debug.Log("Node1 must be != Node2");
            throw new ArgumentOutOfRangeException("Node1 must be != Node2");
        }
        nodesToLink = new List<string>();
        nodesToLink.Add(n1);
        nodesToLink.Add(n2);
    }


    public List<string> getNodes()
    {
        return nodesToLink;
    }


    public string getThisId()
    {
        string ret = null;

        foreach (KeyValuePair<string, GraphEdge> entry in shapeCreator.getEdges())
        {
            if (entry.Value.Equals(this))
            {
                ret = entry.Key;
                break;
            }
        }

        return ret;
    }


    public bool isPointOver(Vector2 xy)
    {

        bool res = false;

        GraphPolygon4 square = new GraphPolygon4(getEdgeSquare(), 0);

        res = square.isPointInside(xy);

        return res;
    }

    private float[] getEdgeSquare()
    {

        int width = 10;


        float[] vertices = new float[8];

        //Получаем координаты грани
        Vector2 st = new Vector2(shapeCreator.getNodes()[nodesToLink[0]].getX(), shapeCreator.getNodes()[nodesToLink[0]].getY());
        Vector2 fn = new Vector2(shapeCreator.getNodes()[nodesToLink[1]].getX(), shapeCreator.getNodes()[nodesToLink[1]].getY());
        //Результирующий вектор
        Vector2 edgeVector = MathGame.lineToVector(st, fn); //базовый вектор

        //Теперь нам нужно найти нормаль к вектору. Координаты вектора-нормали ищется по формулам:
        //x = (-y*y')/x', y = (-x*x')/y'. Тут x' и y' - координаты базового вектора
        //x или y выбираем любой, например y = 1.

        float normY;

        if (edgeVector.x >= 0)//Проверяем, что векто рв правой части графика - тогда Y положительный и наоборот
            normY = 1;
        else
            normY = -1;

        float normX = -(normY * edgeVector.y) / edgeVector.x;//X компонента вектора нормали
        Vector2 resNormal1 = new Vector2(normX, normY); //Y компоненту мы уже определили


        resNormal1.Normalize(); //нормализуем вектор нормали

        resNormal1.x = resNormal1.x * width;// умножаем на ширину нашего прямоугольника
        resNormal1.y = resNormal1.y * width;


        Vector2 resNormal2 = new Vector2(resNormal1.x, resNormal1.y); //Назначаем оппозитный вектор к нормали, чтобы была вторая сторона прямоугольника		
        resNormal2.x = -resNormal2.x;//отражаем оппозитный вектор
        resNormal2.y = -resNormal2.y;


        vertices[0] = resNormal1.x + st.x;
        vertices[1] = resNormal1.y + st.y;
        vertices[2] = resNormal2.x + st.x;
        vertices[3] = resNormal2.y + st.y;
        vertices[4] = resNormal2.x + fn.x;
        vertices[5] = resNormal2.y + fn.y;
        vertices[6] = resNormal1.x + fn.x;
        vertices[7] = resNormal1.y + fn.y;



        return vertices;

    }

}

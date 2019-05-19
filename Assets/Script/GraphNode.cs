using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
   public enum GraphNodeType { STANDART, POE };

    [System.NonSerialized]
    ShapeCreator shapeCreator;

    private List<string> poeList;
    private GraphNodeType nodeType;
    [System.NonSerialized]
    public bool start = false;
    public bool finish = false;
	private float x, y;
    private float renderScale;
    [System.NonSerialized]
    private List<string> neighbours;
    [System.NonSerialized]
    private int G, H; //G - from start cost, H - to finish heuristic cost.
    [System.NonSerialized]
    public Color color;
	


    private GraphNode()
    {
        this.neighbours = new List<string>();
        GameObject thePlayer = GameObject.Find("shapeCreator");
        shapeCreator = thePlayer.GetComponent<ShapeCreator>();
    }

    public GraphNode(float x2, float y2, float renderScale) : this()
    {
        nodeType = GraphNodeType.STANDART;
        this.x = x2;
        this.y = y2;
        this.renderScale = renderScale;
        this.color = Color.yellow;
        this.G = 0;
    }


    public GraphNode(float x2, float y2, float renderScale, int sf) : this(x2, y2, renderScale)
    {

        switch (sf)
        {
            case 1:
                start = true;
                break;
            case 2:
                finish = true;
                break;
        }
    }


    public GraphNode(List<string> poeList, float renderScale) : this(100, 100, renderScale)
    {
      
        this.poeList = poeList;
        nodeType = GraphNodeType.POE;
        setPOEPosition();
        foreach (string poe in poeList)
        {

            try
            {
                shapeCreator.getPOEs()[poe].setChildNode(this.getThisId());
            }
            catch (NullReferenceException nex)
            {
                Debug.Log("Can`t set child POE node");
            }
        }
    }




    public string getThisId()
    {
        string ret = null;

        foreach (KeyValuePair<string, GraphNode> entry in shapeCreator.getNodes())
        {
            if (entry.Key.Equals(this))
            {
                ret = entry.Key;
                break;
            }
        }

        return ret;
    }


    public void clearLinked()
    {

        deleteFromNeighbours();

        foreach (string edg in getNeighbourEdges())
        {
            shapeCreator.getEdges().Remove(edg);
        }

        deleteLinkedPOE();

    }






    public void deleteFromNeighbours()
    {
        foreach (GraphNode nod in shapeCreator.getNodes().Values)
        {
            nod.getNeighbours().Remove(shapeCreator.getNodeKey(this));
        }
    }


    public GraphNode getNearestGraphNode(Vector2 inputXY)
    {
        float curDist, dist;
        GraphNode curGraphNode = this;
        Dictionary<string, GraphNode>.Enumerator nodEnum = shapeCreator.getNodes().GetEnumerator();

        if (nodEnum.MoveNext()) {
            KeyValuePair<string, GraphNode> getGraphNode = nodEnum.Current;
            curGraphNode = getGraphNode.Value;
            curDist = Vector2.Distance(inputXY, new Vector2(curGraphNode.getX(), curGraphNode.getY()));

            dist = curDist;

            foreach (KeyValuePair<string, GraphNode> entry in shapeCreator.getNodes())
            {
                curDist = Vector2.Distance(inputXY, new Vector2(entry.Value.getX(), entry.Value.getY()));
                if (dist > curDist)
                {
                    dist = curDist;
                    curGraphNode = entry.Value;
                }
            }
        }
        return curGraphNode;

    }


    public List<string> getNeighbourEdges()
    {
        List<string> retEdge = new List<string>();
        foreach (KeyValuePair<string, GraphEdge> entry in shapeCreator.getEdges())
        {
            if (getThisId().Equals(entry.Value.getNodes()[0]) || getThisId().Equals(entry.Value.getNodes()[1]))
            {

                if (!retEdge.Contains(entry.Key)) retEdge.Add(entry.Key);
            }
        }

        return retEdge;

    }


    public bool deleteNeighbour(string name)
    {
        bool result = neighbours.Remove(name);
        return result;
    }



    public string printNeighbours()
    {

        string neighbours = "";
        foreach (string node in getNeighbours())
        {
            neighbours = neighbours + " " + node.ToString();
        }

        return "[" + neighbours + "]";
    }


    public void addNeighbour(string name)
    {
        //Gdx.app.log("Fill", "start");

        if (neighbours.Count > 0)
        {
            foreach (string nod in neighbours)
            {
                if (nod.Equals(name))
                {
                    return;
                }
            }
        }
        neighbours.Add(name);

    }



    public void setNeighbours(List<string> names)
    {
        this.neighbours = names;
    }

    /*
	public float getCost(GraphNode node) { //Get Heuristic cost. We need this when we chose next node to move.
		
		
		
		
		
		class CustomComparator implements Comparator<GraphNode> {
		    @Override
		    public int compare(GraphNode o1, GraphNode o2) {
		        return o1.getCost(node).compareTo(o2.getCost(node));
		    }

		}
		
		Vector2 vec;
		
		for (GraphNode nod : getNeighbours())
			
			
	}
	

	*/
    public List<string> getNeighbours()
    {
        return this.neighbours;
    }



    public GraphNodeType GetNodeType()
    {
        return this.nodeType;
    }



    public float getX()
    {
        return this.x;
    }

    public float getY()
    {
        return this.y;
    }

    public void setX(float x2)
    {
        this.x = x2;
    }

    public void setY(float y2)
    {
        this.y = y2;

    }




    public int getCost()
    {
        return this.G + this.H;
    }


    public void setG(int g)
    {
        this.G = g;
    }

    public void setH(int h)
    {
        this.H = h;
    }

    public int getG()
    {
        return this.G;
    }

    public int getH()
    {
        return this.H;
    }

    public float getDistance(GraphNode node)
    {
        float dist;
        Vector2 vec1 = new Vector2(this.x, this.y);
        dist = Vector2.Distance(vec1, new Vector2(node.getX(), node.getY()));
        return dist;
    }

    public float getDistance(float x, float y)
    {
        float dist;
        dist = Vector2.Distance(new Vector2(this.x, this.y), new Vector2(this.x, this.y));
        return dist;
    }



    public float getAngle(GraphNode node)
    {
        float deltaX, deltaY;
        float angle;

        deltaX = node.getX() - this.x;
        deltaY = node.getY() - this.y;

        angle = (float)Math.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        return angle;
    }




    public void updateStatus()
    {
        if (this.nodeType == GraphNodeType.POE)
            setPOEPosition();
    }



    //Methods for POE type
    public List<string> getListOfPOE()
    {
        return this.poeList;
    }


    public void setPOEPosition()
    {
        GraphPointOnEdge poe1 = shapeCreator.getPOEById(poeList[0]);
        GraphPointOnEdge poe2 = shapeCreator.getPOEById(poeList[1]);

        try
        {
            Vector2 pos = getMiddleOfLine(poe1.getPosition().x, poe1.getPosition().y, poe2.getPosition().x, poe2.getPosition().y);
            this.setX((int)pos.x);
            this.setY((int)pos.y);
        }
        catch (NullReferenceException nex)
        {
            Debug.Log(poeList[0] + " : " + poe1 + "|" + poeList[1] + " : " + poe2);
            Debug.Log("setPOEPosition() - no poe! ");
        }
    }


    public Vector2 getMiddleOfLine(float x1, float y1, float x2, float y2)
    {

        float x = (x1 + x2) / 2;
        float y = (y1 + y2) / 2;

        Vector2 ret = new Vector2(x, y);

        return ret;
    }


    public void deleteLinkedPOE()
    {
        if (poeList != null)
        {
            foreach (string poe in poeList)
            {
                Debug.Log("Deleting POE: " + poe);
                shapeCreator.getPOEs().Remove(poe);
            }
            poeList.Clear();
        }

    }



    public void setRenderScale(float distance)
    {
        this.renderScale = distance;
    }

    public float getRenderScale()
    {
        return renderScale;
    }




}

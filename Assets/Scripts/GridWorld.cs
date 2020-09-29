using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridWorld : MonoBehaviour
{
    public static GridWorld instance;
    public LayerMask WallMask;//This is the mask that the program will look for when trying to find obstructions to the path.
    public LayerMask FenceMask;
    public LayerMask MudMask;
    public LayerMask GravelMask;
    public Vector2 vGridWorldSize;//A vector2 to store the width and height of the graph in world units.
    public float fNodeRadius;//This stores how big each square on the graph will be
    public float fDistanceBetweenNodes;//The distance that the squares will spawn from eachother.
    public int mudCost;
    public int gravelCost;

    Node[,] NodeArray;//The array of nodes that the A Star algorithm uses.
    public List<Node> FinalPath;//The completed path that the red line will be drawn along

    float fNodeDiameter;//Twice the amount of the radius (Set in the start function)
    public int iGridSizeX, iGridSizeY;//Size of the Grid in Array units.

    public enum Scenario { Select, Known, Unknown, ThreeAgents }
    public Scenario scenario;

    public enum Algorithm { AStar, Dijkstra }
    public Algorithm algorithm;


    public enum SearchMode { CellByCell, Randomly }
    public SearchMode searchMode ;

    public int speed;
    public int nbTrees;

    private void Awake()//Ran once the program starts
    {
        instance = this;

        GameObject player2 = GameObject.Find("Lumberjack2");
        player2.SetActive(false);
        GameObject player3 = GameObject.Find("Lumberjack3");
        player3.SetActive(false);

        fNodeDiameter = fNodeRadius * 2;//Double the radius to get diameter
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        CreateGrid();//Draw the grid

        if (scenario == Scenario.Known && algorithm == Algorithm.AStar)
        {
            print("Scenario : " + scenario + ", Algorithm = " + algorithm);
            PathfindingA PAscript = this.gameObject.AddComponent(typeof(PathfindingA)) as PathfindingA;
            GameObject player = GameObject.Find("Agent");
            Unit unitScript = player.gameObject.AddComponent(typeof(Unit)) as Unit;
            unitScript.enabled = true;
            PAscript.enabled = true;
        }
        else if (scenario == Scenario.Known && algorithm == Algorithm.Dijkstra)
        {
            print("Scenario : " + scenario + ", Algorithm = " + algorithm);
            Dijkstra DIJscript = this.gameObject.AddComponent(typeof(Dijkstra)) as Dijkstra;
            GameObject player = GameObject.Find("Agent");
            UnitDijkstra unitDijkstraScript = player.gameObject.AddComponent(typeof(UnitDijkstra)) as UnitDijkstra;
            unitDijkstraScript.enabled = true;
            DIJscript.enabled = true;
        }
        else if (scenario == Scenario.Unknown && searchMode == SearchMode.CellByCell)
        {
            print("Scenario : " + scenario + ", Search Mode : " + searchMode + " , Algorithm = " + algorithm);
            GameObject player = GameObject.Find("Agent");
            UnitSearchUnkCell UnitSearchscript = player.gameObject.AddComponent(typeof(UnitSearchUnkCell)) as UnitSearchUnkCell;
            UnitSearchscript.enabled = true;
        }
        else if (scenario == Scenario.Unknown && searchMode == SearchMode.Randomly)
        {
            print("Scenario : " + scenario + ", Search Mode : " + searchMode + " , Algorithm = " + algorithm);

            GameObject player = GameObject.Find("Agent");
            UnitRandom UnitRandomscript = player.gameObject.AddComponent(typeof(UnitRandom)) as UnitRandom;
            UnitRandomscript.enabled = true;

        }
        else if (scenario == Scenario.ThreeAgents)
        {

            player2.SetActive(true);
            player3.SetActive(true);

            print("Scenario : " + scenario + ", Algorithm = " + algorithm);
            ThreeAgents CompThreeAgents = this.gameObject.AddComponent(typeof(ThreeAgents)) as ThreeAgents;
            CompThreeAgents.enabled = true;
        }
    }

    void CreateGrid()
    {
        NodeArray = new Node[iGridSizeX, iGridSizeY];//Declare the array of nodes.
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < iGridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < iGridSizeY; y++)//Loop through the array of nodes
            {

                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Walk = true;//Make the node a wall
                bool Mud = false;
                bool Gravel = false;
                bool Discovered = false;
                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a WallMask,
                //The if statement will return false.
                if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask) || (Physics.CheckSphere(worldPoint, fNodeRadius, FenceMask)))
                {
                    Walk = false;//Object is not a wall

                }
                if (Physics.CheckSphere(worldPoint, fNodeRadius, GravelMask))
                {
                    Gravel = true;
                }
                if (Physics.CheckSphere(worldPoint, fNodeRadius, MudMask))
                {
                    Mud = true;
                }

                NodeArray[x, y] = new Node(Discovered, Walk, Mud, Gravel, worldPoint, x, y);//Create a new node in the array.
            }
        }
    }

    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }

    public Node NodeFromGridPos(int x, int y)
    {
        return NodeArray[x, y];
    }


    //Function that draws the wireframe
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (NodeArray != null)//If the grid is not empty
        {
            foreach (Node n in NodeArray)//Loop through every node in the grid
            {
                if (n.isWalkable)//If the current node is a wall node
                {
                    if (n.isMud)
                    {
                        Gizmos.color = Color.blue;
                    }
                    else if (n.isGravel)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else if (n.isDiscovered)
                    {
                        Color OrangePath = new Color(218f, 0f, 255f);
                        Gizmos.color = OrangePath;
                    }
                    else 
                    {
                        Color GreyPath = new Color(211f, 211f, 211f);
                        Gizmos.color = GreyPath;
                    }
                } 
                else
                {
                    Gizmos.color = Color.red;//Set the color of the node
                }


                if (FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.green;//Set the color of that node
                        Gizmos.DrawCube(n.vPosition, Vector3.one);
                    }

                }
                Gizmos.DrawCube(n.vPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
}




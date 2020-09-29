using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : MonoBehaviour {

    public static Dijkstra instance;
    GridWorld GridReference;//For referencing the grid class
    [HideInInspector]
    public Transform TargetPosition;//Starting position to pathfind to
    public static List<Node> PathToWalk;

    private void Awake()//When the program starts
    {
        instance = this;
        GridReference = GetComponent<GridWorld>();//Get a reference to the game manager
    }

    public void FindShortPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

        List<Node> UnexploredList = new List<Node>();//List of nodes for the open list
        HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

        UnexploredList.Add(StartNode);//Add the starting node to the open list to begin the program

        while (UnexploredList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = UnexploredList[0];//Create a node and set it to the first item in the open list
            CurrentNode.ihCost = 0;


            for (int i = 1; i < UnexploredList.Count; i++)//Loop through the open list starting from the second object
            {
                UnexploredList[i].ihCost = Mathf.FloorToInt(Mathf.Infinity);

                if (UnexploredList[i].ihCost < CurrentNode.ihCost)//If the distance of that object is less than or equal to the distance of the current node
                {
                    CurrentNode = UnexploredList[i];//Set the current node to that object
                }
            }
            UnexploredList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                PathToWalk = FinalPath(StartNode, TargetNode);//Calculate the final path
            }


            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.isWalkable || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                if (NeighborNode.isMud)
                {
                    CurrentNode.ihCost += GridWorld.instance.mudCost;
                }
                if (NeighborNode.isGravel)
                {
                    CurrentNode.ihCost += GridWorld.instance.gravelCost;
                }

                int MoveCost = CurrentNode.ihCost + GetManhattanDistance(CurrentNode, NeighborNode);//Get the cost of that neighbor



                if (MoveCost < NeighborNode.ihCost || !UnexploredList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.ihCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!UnexploredList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        UnexploredList.Add(NeighborNode);//Add it to the list
                    }
                }
            }
        }
    }


    List<Node> FinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        GridReference.FinalPath = FinalPath;//Set the final path
        return FinalPath;
    }

    int GetManhattanDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

        return ix + iy;//Return the sum
    }
}

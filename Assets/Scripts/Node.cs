using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int iGridX;//X Position in the Node Array
    public int iGridY;//Y Position in the Node Array

    public bool isDiscovered;
    public bool isWalkable;//Tells the program if this node is being obstructed. 
    public bool isMud;
    public bool isGravel;
    public Vector3 vPosition;//The world position of the node.

    public Node ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public int igCost;//The cost of moving to the next square.
    public int ihCost;//The distance to the goal from this node.

    public int FCost { get { return igCost + ihCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

    public Node(bool a_isDiscovered, bool a_IsWalkable, bool a_IsMud, bool a_IsGravel, Vector3 a_vPos, int a_igridX, int a_igridY)//Constructor
    {
        a_isDiscovered = isDiscovered;
        isGravel = a_IsGravel;
        isMud = a_IsMud;
        isWalkable = a_IsWalkable;//Tells the program if this node is being obstructed.
        vPosition = a_vPos;//The world position of the node.
        iGridX = a_igridX;//X Position in the Node Array
        iGridY = a_igridY;//Y Position in the Node Array
    }

}
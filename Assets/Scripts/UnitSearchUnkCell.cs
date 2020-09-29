using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSearchUnkCell : MonoBehaviour {

    GridWorld GridReference; //For referencing the grid class
    int speed;
    Node TargetNode;
    Node Up;
    Node Left;
    Node Right;
    private Quaternion targetRotation;
    float rotationSpeed = 5;
    bool foundTree = false;
    bool foundSawmill = false;

    private void Awake()//When the program starts
    {
        GameObject GameManager = GameObject.Find("GameManager");
        speed = GridWorld.instance.speed * 3;

        if (GameManager != null)
        {
            GridReference = GameManager.GetComponent<GridWorld>();
        }
    }

    private void Start()
    {
        TargetNode = GridReference.NodeFromWorldPoint(transform.position);
        print("Player X = " + TargetNode.iGridX + "and Y = " + TargetNode.iGridY);
        TargetNode.isDiscovered = true;
        TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);
        TargetNode.isDiscovered = true;
    }

    bool movingLeft = false;
    bool avoidWall = false;

    private void Update()
    {

        if ((foundSawmill && foundTree) && (GridWorld.instance.scenario != GridWorld.Scenario.ThreeAgents))
        {
            print("Found Both!!");

            if (GridReference.algorithm == GridWorld.Algorithm.AStar)
            {
                GameObject GameManager = GameObject.Find("GameManager");
                PathfindingA PAscript = GameManager.gameObject.AddComponent(typeof(PathfindingA)) as PathfindingA;
                PAscript.enabled = true;
                Unit unitScript = this.gameObject.AddComponent(typeof(Unit)) as Unit;
                unitScript.enabled = true;

                Destroy(this.GetComponent<UnitSearchUnkCell>()); //remove
                return;
            }

            if (GridReference.algorithm == GridWorld.Algorithm.Dijkstra)
            {
                GameObject GameManager = GameObject.Find("GameManager");
                Dijkstra DJscript = GameManager.gameObject.AddComponent(typeof(Dijkstra)) as Dijkstra;
                DJscript.enabled = true;
                UnitDijkstra UnitDijkstraScript = this.gameObject.AddComponent(typeof(UnitDijkstra)) as UnitDijkstra;
                UnitDijkstraScript.enabled = true;

                Destroy(this.GetComponent<UnitSearchUnkCell>()); //remove
                return;
            }
        }

        if (TargetNode.vPosition != transform.position)
        {
            Vector3 forward = TargetNode.vPosition - transform.position;

            targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (TargetNode.iGridX == 0)
        {
            //we are the left edge, move up
            TargetNode = GridReference.NodeFromGridPos(1, TargetNode.iGridY+1);
            movingLeft = false;
        }
        if (TargetNode.iGridX >= GridReference.vGridWorldSize.x - 1)
        {
            //we are on right edge, move up
            TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX-1, TargetNode.iGridY + 1);
            movingLeft = true;
        }

        if ((TargetNode.iGridX <= GridReference.vGridWorldSize.x - 1) && (TargetNode.iGridY <= GridReference.vGridWorldSize.y - 1))
        {
            if (avoidWall)
            {
                Up = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);

                if ((Up.isWalkable) && (Up.isDiscovered))
                {
                    TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);

                }
                if ((Up.isWalkable) && (!Up.isDiscovered))
                {
                    TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);
                    avoidWall = false;
                }
            }

            Left = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY);

            if ((!Left.isWalkable) && (Left.iGridX >= 1))
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY - 1);
                avoidWall = true;
            }

            Right = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);

            if ((!Right.isWalkable) && (!(Right.iGridX >= GridReference.vGridWorldSize.x - 1)))
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY - 1);
                avoidWall = true;
            }


        }

        if (Vector3.Distance(transform.position, TargetNode.vPosition) < 0.1f)
        {
            TargetNode = GridReference.NodeFromGridPos((movingLeft==false)?TargetNode.iGridX + 1: TargetNode.iGridX - 1, TargetNode.iGridY);
        }

        transform.position = Vector3.Lerp(transform.position, TargetNode.vPosition, speed * Time.deltaTime);
        TargetNode.isDiscovered = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Tree")
        {
            foundTree = true;
            print("Found the tree!!");
        }
        else
        if (other.gameObject.name == "Sawmill")
        {
            foundSawmill = true;
            print("Found the Sawmill!!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRandom : MonoBehaviour
{

    GridWorld GridReference; 
    int speed;
    Node TargetNode;
    Node Up;
    Node Left;
    Node Right;
    Node Bottom;
    private Quaternion targetRotation;
    float rotationSpeed = 5;
    bool foundTree = false;
    bool foundSawmill = false;
    int randomMove;
    int moveX;
    int moveY;
    bool movingLeft = false;
    bool movingRight = false;
    bool movingTop = false;
    bool movingBottom = false;
    bool movingTopLeft = false;
    bool movingTopRight = false;
    bool movingBottomLeft = false;
    bool movingBottomRight = false;


    private void Awake()
    {
        speed = GridWorld.instance.speed * 3;
        GameObject GameManager = GameObject.Find("GameManager");

        if (GameManager != null)
        {
            GridReference = GameManager.GetComponent<GridWorld>();
        }
    }

    private void Start()
    {
        TargetNode = GridReference.NodeFromWorldPoint(transform.position);
        print("Player X = " + TargetNode.iGridX + "and Y = " + TargetNode.iGridY);
        TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);
        TargetNode.isDiscovered = true;
    }

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

                Destroy(this.GetComponent<UnitRandom>()); //remove
                return;
            }

            if (GridReference.algorithm == GridWorld.Algorithm.Dijkstra)
            {
                GameObject GameManager = GameObject.Find("GameManager");
                Dijkstra DJscript = GameManager.gameObject.AddComponent(typeof(Dijkstra)) as Dijkstra;
                DJscript.enabled = true;
                UnitDijkstra UnitDijkstraScript = this.gameObject.AddComponent(typeof(UnitDijkstra)) as UnitDijkstra;
                UnitDijkstraScript.enabled = true;

                Destroy(this.GetComponent<UnitRandom>()); //remove
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
            //we are the left edge
            randomMove = Random.Range(1, 4);

            switch (randomMove)
            {
                case 1:
                    moveX = 1;
                    moveY = 0;
                    break;
                case 2:
                    moveX = 1;
                    moveY = 1;
                    break;
                case 3:
                    moveX = 1;
                    moveY = -1;
                    break;
                default:
                    break;
            }
        }
        if (TargetNode.iGridX >= GridReference.vGridWorldSize.x - 1)
        {
            //we are on right edge
            randomMove = Random.Range(1, 4);

            switch (randomMove)
            {
                case 1:
                    moveX = -1;
                    moveY = 0;
                    break;
                case 2:
                    moveX = -1;
                    moveY = 1;
                    break;
                case 3:
                    moveX = -1;
                    moveY = -1;
                    break;
                default:
                    break;
            }
        }
        if (TargetNode.iGridY == 0)
        {
            //we are the Bottom edge
            randomMove = Random.Range(1, 4);

            switch (randomMove)
            {
                case 1:
                    moveX = -1;
                    moveY = 1;
                    break;
                case 2:
                    moveX = 1;
                    moveY = 1;
                    break;
                case 3:
                    moveX = 0;
                    moveY = 1;
                    break;
                default:
                    break;
            }
        }
        if (TargetNode.iGridY >= GridReference.vGridWorldSize.y - 1)
        {
            //we are on Top edge
            randomMove = Random.Range(1, 4);

            switch (randomMove)
            {
                case 1:
                    moveX = 0;
                    moveY = -1;
                    break;
                case 2:
                    moveX = 1;
                    moveY = -1;
                    break;
                case 3:
                    moveX = -1;
                    moveY = -1;
                    break;
                default:
                    break;
            }
        }
        if ((TargetNode.iGridX == 0) && (TargetNode.iGridY == 0))
        {
            //we are the left bottom corner
            randomMove = Random.Range(1, 5);

            switch (randomMove)
            {
                case 1:
                    moveX = 1;
                    moveY = 0;
                    break;
                case 2:
                    moveX = 0;
                    moveY = 1;
                    break;
                case 3:
                    moveX = 1;
                    moveY = 1;
                    break;
                case 4:
                    moveX = 1;
                    moveY = 1;
                    break;
                default:
                    break;
            }
        }
        if ((TargetNode.iGridX == 0) && (TargetNode.iGridY >= GridReference.vGridWorldSize.y - 1))
        {
            //we are the left top corner
            randomMove = Random.Range(1, 5);

            switch (randomMove)
            {
                case 1:
                    moveX = 1;
                    moveY = 0;
                    break;
                case 2:
                    moveX = 0;
                    moveY = -1;
                    break;
                case 3:
                    moveX = 1;
                    moveY = -1;
                    break;
                case 4:
                    moveX = 1;
                    moveY = -1;
                    break;
                default:
                    break;
            }
        }
        if ((TargetNode.iGridX >= GridReference.vGridWorldSize.x - 1) && ((TargetNode.iGridY >= GridReference.vGridWorldSize.y - 1)))
        {
            //we are on right top corner
            randomMove = Random.Range(1, 5);

            switch (randomMove)
            {
                case 1:
                    moveX = -1;
                    moveY = 0;
                    break;
                case 2:
                    moveX = 0;
                    moveY = -1;
                    break;
                case 3:
                    moveX = -1;
                    moveY = -1;
                    break;
                case 4:
                    moveX = -1;
                    moveY = -1;
                    break;
                default:
                    break;
            }
        }
        if ((TargetNode.iGridX >= GridReference.vGridWorldSize.x - 1) && (TargetNode.iGridY == 0))
        {
            //we are on right bottom corner
            randomMove = Random.Range(1, 5);

            switch (randomMove)
            {
                case 1:
                    moveX = 0;
                    moveY = 1;
                    break;
                case 2:
                    moveX = -1;
                    moveY = 0;
                    break;
                case 3:
                    moveX = -1;
                    moveY = 1;
                    break;
                case 4:
                    moveX = -1;
                    moveY = 1;
                    break;
                default:
                    break;
            }
        }

        if ((TargetNode.iGridX < GridReference.vGridWorldSize.x - 1) && 
            (TargetNode.iGridY < GridReference.vGridWorldSize.y - 1) &&
            (TargetNode.iGridX > 0) && (TargetNode.iGridY > 0))
        {
            Left = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY);
            Bottom = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY - 1);
            Up = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);
            Right = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);

            if (!Left.isWalkable)
            {
                randomMove = Random.Range(1, 4);

                switch (randomMove)
                {
                    case 1:
                        moveX = 1;
                        moveY = 0;
                        break;
                    case 2:
                        moveX = 1;
                        moveY = 1;
                        break;
                    case 3:
                        moveX = 1;
                        moveY = -1;
                        break;
                    default:
                        break;
                }
            }
            else if (!Right.isWalkable)
            {
                randomMove = Random.Range(1, 4);

                switch (randomMove)
                {
                    case 1:
                        moveX = -1;
                        moveY = 0;
                        break;
                    case 2:
                        moveX = -1;
                        moveY = 1;
                        break;
                    case 3:
                        moveX = -1;
                        moveY = -1;
                        break;
                    default:
                        break;
                }
            }
            else if (!Up.isWalkable)
            {
                randomMove = Random.Range(1, 4);

                switch (randomMove)
                {
                    case 1:
                        moveX = 0;
                        moveY = -1;
                        break;
                    case 2:
                        moveX = 1;
                        moveY = -1;
                        break;
                    case 3:
                        moveX = -1;
                        moveY = -1;
                        break;
                    default:
                        break;
                }
            }
            else  if (!Bottom.isWalkable)
            {
                randomMove = Random.Range(1, 4);

                switch (randomMove)
                {
                    case 1:
                        moveX = -1;
                        moveY = 1;
                        break;
                    case 2:
                        moveX = 1;
                        moveY = 1;
                        break;
                    case 3:
                        moveX = 0;
                        moveY = 1;
                        break;
                    default:
                        break;
                }
            }
        }

        if ((moveX == -1) && (moveY == 0))
        {
            movingLeft = true;
            movingBottom = false;
            movingRight = false;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == 1) && (moveY == 0))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = true;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == 0) && (moveY == -1))
        {
            movingLeft = false;
            movingBottom = true;
            movingRight = false;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == 0) && (moveY == 1))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = false;
            movingTop = true;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == 1) && (moveY == 1))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = false;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = true;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == -1) && (moveY == 1))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = false;
            movingTop = false;
            movingTopLeft = true;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = false;
        }
        else if ((moveX == 1) && (moveY == -1))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = false;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = false;
            movingBottomRight = true;
        }
        else if ((moveX == -1) && (moveY == -1))
        {
            movingLeft = false;
            movingBottom = false;
            movingRight = false;
            movingTop = false;
            movingTopLeft = false;
            movingTopRight = false;
            movingBottomLeft = true;
            movingBottomRight = false;
        }


        if (Vector3.Distance(transform.position, TargetNode.vPosition) < 0.1f)
        {
            if (movingLeft)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY);
            }
            else if (movingBottom)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY - 1);
            }
            else if (movingRight)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);
            }
            else if (movingTop)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);
            }
            else if (movingTopLeft)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY + 1);
            }
            else if (movingTopRight)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY + 1);
            }
            else if (movingBottomLeft)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY - 1);
            }
            else if (movingBottomRight)
            {
                TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY - 1);
            }
        }

        //print("Position X = " + TargetNode.iGridX + " and Y = " + TargetNode.iGridY);
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


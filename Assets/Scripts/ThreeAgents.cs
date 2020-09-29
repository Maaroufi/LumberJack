using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeAgents : MonoBehaviour {

    GridWorld GridReference;
    private GameObject player;
    private GameObject player2;
    private GameObject tree;
    private GameObject sawmill;
    private GameObject player3;
    LostForest lostForestPlayer;
    LostForest lostForestPlayer2;
    LostForest lostForestPlayer3;
    public static ThreeAgents instance;
    private bool firstCall = true;
    public int nbTrees;
    private GameObject trees;
    private int nbGameobjectTagSpawn;
    private GameObject[] allMovableThings;

    public List<GameObject> playerMeet;
    private LostForest lostForestPlayerFound1;
    private LostForest lostForestPlayerFound2;
    private GameObject treeShared;

    private void Awake()
    {
        instance = this;
        player2 = GameObject.Find("Lumberjack2");
        player3 = GameObject.Find("Lumberjack3");
        player = GameObject.Find("Agent");

        lostForestPlayer = player.GetComponent<LostForest>();
        lostForestPlayer2 = player2.GetComponent<LostForest>();
        lostForestPlayer3 = player3.GetComponent<LostForest>();
        allMovableThings = GameObject.FindGameObjectsWithTag("avoidSpawn");
    }

    void Start () {
        playerMeet = new List<GameObject>();
        nbTrees = GridWorld.instance.nbTrees;
        UnitRandom UnitRandomscript2 = player2.gameObject.AddComponent(typeof(UnitRandom)) as UnitRandom;
        UnitRandomscript2.enabled = true;
        LostForest CompLostForest2 = player2.gameObject.AddComponent(typeof(LostForest)) as LostForest;
        CompLostForest2.enabled = true;

        UnitRandom UnitRandomscript3 = player3.gameObject.AddComponent(typeof(UnitRandom)) as UnitRandom;
        UnitRandomscript3.enabled = true;
        LostForest CompLostForest3 = player3.gameObject.AddComponent(typeof(LostForest)) as LostForest;
        CompLostForest3.enabled = true;

        UnitSearchUnkCell UnitSearchscript = player.gameObject.AddComponent(typeof(UnitSearchUnkCell)) as UnitSearchUnkCell;
        UnitSearchscript.enabled = true;
        LostForest CompLostForest = player.gameObject.AddComponent(typeof(LostForest)) as LostForest;
        CompLostForest.enabled = true;

        Vector3 positionSawmill = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));

        if (checkIfPosEmpty(positionSawmill))
        {
            GameObject sawmill = GameObject.Find("Sawmill");
            sawmill.transform.position = positionSawmill;
        }

        for (int i = 0; i < nbTrees; i++)
        {
     
            Vector3 positionTree = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
            if (checkIfPosEmpty(positionTree))
            {
                GameObject trees = GameObject.Find("Tree");
                Instantiate(trees, positionTree, Quaternion.identity);
            }
        }
    }

    public void groupLumber(GameObject lumberjack1, GameObject lumberjack2, GameObject tree)
    {
        if (firstCall)
        {
            firstCall = false;

            if (lumberjack1.GetComponent<UnitRandom>() != null)
            {
                Destroy(lumberjack1.GetComponent<UnitRandom>()); //remove
            }
            else
            {
                Destroy(lumberjack1.GetComponent<UnitSearchUnkCell>());
            }
            if (lumberjack2.GetComponent<UnitRandom>() != null)
            {
                Destroy(lumberjack2.GetComponent<UnitRandom>()); //remove
            }
            else
            {
                Destroy(lumberjack2.GetComponent<UnitSearchUnkCell>());
            }

            if (GridWorld.instance.algorithm == GridWorld.Algorithm.AStar)
            {
                PathfindingA Pscript = this.gameObject.AddComponent(typeof(PathfindingA)) as PathfindingA;
                Pscript.enabled = true;

                UnitThree unitScript = lumberjack1.gameObject.AddComponent(typeof(UnitThree)) as UnitThree;
                unitScript.enabled = true;
                UnitThree unitScript2 = lumberjack2.gameObject.AddComponent(typeof(UnitThree)) as UnitThree;
                unitScript2.enabled = true;
            }

            if (GridWorld.instance.algorithm == GridWorld.Algorithm.Dijkstra)
            {
                Dijkstra DJscript = this.gameObject.AddComponent(typeof(Dijkstra)) as Dijkstra;
                DJscript.enabled = true;

                UnitThreeDJ unitScript = lumberjack1.gameObject.AddComponent(typeof(UnitThreeDJ)) as UnitThreeDJ;
                unitScript.enabled = true;
                UnitThreeDJ unitScript2 = lumberjack2.gameObject.AddComponent(typeof(UnitThreeDJ)) as UnitThreeDJ;
                unitScript2.enabled = true;
            }
        }
    }

    public bool checkIfPosEmpty(Vector3 targetPos)
    {      
        foreach (GameObject current in allMovableThings)
        {
            if (current.transform.position == targetPos)
            return false;
        }
        return true;
    }

    public bool foundAPlayer(GameObject Player1)
    {
        print("FUNCTION REACHED!!!!");
        playerMeet.Add(Player1);

        if (playerMeet.Count == 2)
        {
            GameObject lumberjack1;
            GameObject lumberjack2;

            lumberjack1 = playerMeet[0];
            lumberjack2 = playerMeet[1];

            lostForestPlayerFound1 = lumberjack1.GetComponent<LostForest>();
            lostForestPlayerFound2 = lumberjack2.GetComponent<LostForest>();

            if ((lostForestPlayerFound1.foundATree) && (!lostForestPlayerFound2.foundATree))
            {
                print("TREE SHARED!!!!");
                lostForestPlayerFound2.hereIsTheTree = lostForestPlayerFound1.hereIsTheTree;
                lostForestPlayerFound2.foundATree = true;
                treeShared = lostForestPlayerFound2.hereIsTheTree;
            }
            if ((lostForestPlayerFound1.foundATree) && (lostForestPlayerFound2.foundATree))
            {
                print("TREE SHARED!!!!");
                lostForestPlayerFound2.hereIsTheTree = lostForestPlayerFound1.hereIsTheTree;
                lostForestPlayerFound2.foundATree = true;
                treeShared = lostForestPlayerFound2.hereIsTheTree;
            }
            if ((!lostForestPlayerFound1.foundATree) && (lostForestPlayerFound2.foundATree))
            {
                print("TREE SHARED!!!!");
                lostForestPlayerFound1.hereIsTheTree = lostForestPlayerFound2.hereIsTheTree;
                lostForestPlayerFound1.foundATree = true;
                treeShared = lostForestPlayerFound1.hereIsTheTree;
            }
            if ((lostForestPlayerFound1.foundTheSawmill) && (!lostForestPlayerFound2.foundTheSawmill))
            {
                print("TREE SHARED!!!!");
                lostForestPlayerFound2.hereIsTheSawmill = lostForestPlayerFound1.hereIsTheSawmill;
                lostForestPlayerFound2.foundTheSawmill = true;
                sawmill = lostForestPlayerFound2.hereIsTheSawmill;
            }
            if ((!lostForestPlayerFound1.foundTheSawmill) && (lostForestPlayerFound2.foundTheSawmill))
            {
                print("SAWMILL SHARED!!!!");
                lostForestPlayerFound1.hereIsTheSawmill = lostForestPlayerFound2.hereIsTheSawmill;
                lostForestPlayerFound1.foundTheSawmill = true;
                sawmill = lostForestPlayerFound2.hereIsTheSawmill;
            }

            if (lostForestPlayerFound1.foundATree && lostForestPlayerFound2.foundATree 
                && lostForestPlayerFound1.foundTheSawmill && lostForestPlayerFound2.foundTheSawmill)
            {
                print("ATTEINT!!!!");

                groupLumber(lumberjack1, lumberjack2, treeShared);
                return true;
            }

            playerMeet.Clear();
            return false;
        }
        return false;
    }
}

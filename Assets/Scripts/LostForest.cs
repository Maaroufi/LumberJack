using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostForest : MonoBehaviour {

    public static LostForest instance;
    public bool foundATree = false;
    public bool foundTheSawmill = false;
    public bool foundAPlayer = false;
    ThreeAgents agentsManager;
    public GameObject hereIsTheTree;
    public GameObject hereIsTheSawmill;
    public GameObject playerFound;

    public static LostForest lostForestPlayerFound;

    private void Awake()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        instance = this;
        agentsManager = GameManager.GetComponent<ThreeAgents>();
    }
	
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TagTree")
        {
            foundATree = true;
            print("Found the tree!!");
            hereIsTheTree = other.gameObject;
        }
        else if (other.gameObject.name == "Sawmill")
        {
            foundTheSawmill = true;
            print("Found the Sawmill!!");
            hereIsTheSawmill = other.gameObject;
        }
        else if (other.gameObject.tag == "Player")
        {
            bool test;
            print("Found a player!!");
            playerFound = other.gameObject;
            foundAPlayer = true;
            test = agentsManager.foundAPlayer(this.gameObject);

            if (!test)
            {
                foundAPlayer = false;
            }
            //lostForestPlayerFound = playerFound.GetComponent<LostForest>();

            //if ((lostForestPlayerFound.foundATree) && (foundATree == false))
            //{
            //    hereIsTheTree = lostForestPlayerFound.hereIsTheTree;
            //    foundATree = true;
            //}
            //else if ((lostForestPlayerFound.foundATree) && (foundATree == true))
            //{
            //    hereIsTheTree = lostForestPlayerFound.hereIsTheTree;
            //    foundATree = true;
            //}
            //if ((lostForestPlayerFound.foundTheSawmill) && (foundTheSawmill == false))
            //{
            //    hereIsTheSawmill = lostForestPlayerFound.hereIsTheSawmill;
            //    foundTheSawmill = true;
            //}

            //if (foundAPlayer && foundATree && foundTheSawmill)
            //{
            //    ThreeAgents.instance.groupLumber(this.gameObject, playerFound, hereIsTheTree);
            //}
        }
    }
}

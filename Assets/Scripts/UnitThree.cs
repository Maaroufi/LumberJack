using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitThree : MonoBehaviour
{
    public int speed;
    private Vector3 waypointPos;
    public static List<Node> WalkFinalPath;
    private Quaternion targetRotation;
    public float rotationSpeed = 5;
    private bool treeReached = false;
    private float startTime;
    bool cutting = false;
    float timerCount;
    private GameObject hereIsTheTree;

    int count = 0;
    Vector3 targetP;

    private GameObject Target;


    public void Awake()
    {
        NewTarget(LostForest.instance.hereIsTheTree);
    }

    private void Start()
    {
        startTime = Time.time;
        speed = 7;
    }

    void FixedUpdate()
    {
        if (!treeReached)
        {
            Walk();
        }
        else
        {
            if (cutting == false)
            {
                cutting = true;
                StartCoroutine(Wait());
            }
            Walk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TagTree")
        {
            hereIsTheTree = other.gameObject;
            timerCount = Time.time - startTime;
            Debug.Log("Agent found the Tree, time: " + timerCount);
            treeReached = true;
        }
        else if (other.gameObject.name == "Sawmill")
        {
            timerCount = Time.time - startTime;
            Debug.Log("Agent found the Sawmill, time: " + timerCount);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3);
        hereIsTheTree.SetActive(false);
        Target = GameObject.Find("Sawmill");
        NewTarget(Target);
    }

    void NewTarget(GameObject gameObject)
    {
        PathfindingA.instance.TargetPosition = gameObject.transform;
        PathfindingA.instance.FindPath(transform.position, gameObject.transform.position);
        WalkFinalPath = PathfindingA.PathToWalk;
        count = 0;
        targetP = WalkFinalPath[count].vPosition;
    }

    void Walk()
    {
        if (Vector3.Distance(transform.position, targetP) < 0.2f)
        {
            count += 1;
            count = Mathf.Clamp(count, 0, WalkFinalPath.Count - 1);
            targetP = WalkFinalPath[count].vPosition;
        }

        transform.position = Vector3.Lerp(transform.position, targetP, speed * Time.deltaTime);

        if (targetP != transform.position)
        {
            Vector3 forward = targetP - transform.position;

            targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

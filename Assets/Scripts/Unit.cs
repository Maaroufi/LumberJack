using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public int speed;
    private Vector3 waypointPos;
    public static List<Node> WalkFinalPath;
    private Quaternion targetRotation;
    public float rotationSpeed = 5;
    private bool treeReached = false;
    private float startTime;
    bool cutting = false;
    float timerCount;

    int count = 0;
    Vector3 targetP;

    private GameObject Target;


    public void Awake()
    {
        speed = GridWorld.instance.speed * 3;
        NewTarget("Tree");

    }

    private void Start()
    {
        startTime = Time.time;

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
        if (other.gameObject.name == "Tree")
        {
            timerCount = Time.time - startTime;
            Debug.Log("Agent found the Tree, time: " + timerCount);
            treeReached = true;
        }
        else
        if (other.gameObject.name == "Sawmill")
        {
            timerCount = Time.time - startTime;
            Debug.Log("Agent found the Sawmill, time: " + timerCount);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3);
        GameObject.Find("Tree").SetActive(false);
        NewTarget("Sawmill");
    }

    void NewTarget(string gameObject)
    {
        Target = GameObject.Find(gameObject);
        PathfindingA.instance.TargetPosition = Target.transform;
        PathfindingA.instance.FindPath(transform.position, Target.transform.position);
        WalkFinalPath = PathfindingA.PathToWalk;
        count = 0;
        targetP = WalkFinalPath[count].vPosition;
    }

    void Walk()
    {
        if (Vector3.Distance(transform.position, targetP) < 0.2f)
        {
            count += 1;
            count = Mathf.Clamp(count, 0, WalkFinalPath.Count-1);
            targetP = WalkFinalPath[count].vPosition;
        }

        transform.position = Vector3.Lerp(transform.position, targetP, speed * Time.deltaTime);

        if(targetP != transform.position)
        {
            Vector3 forward = targetP - transform.position;

            targetRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

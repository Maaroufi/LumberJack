﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandom : MonoBehaviour {

    GridWorld GridReference;
    Node TargetNode;
    Node Bottom;
    Node Left;
    Node Up;
    Node Right;
    private Quaternion targetRotation;
    float rotationSpeed = 5;

    public float velocidadMax;

    public float xMax;
    public float zMax;
    public float xMin;
    public float zMin;

    private float x;
    private float z;
    private float tiempo;
    private float angulo;

    private void Awake()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        if (GameManager != null)
        {
            GridReference = GameManager.GetComponent<GridWorld>();
        }
    }

    // Use this for initialization
    void Start()
    {
        x = Random.Range(-velocidadMax, velocidadMax);
        z = Random.Range(-velocidadMax, velocidadMax);
        angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
        transform.localRotation = Quaternion.Euler(0, angulo, 0);
    }

    // Update is called once per frame
    void Update()
    {
        TargetNode = GridReference.NodeFromWorldPoint(transform.position);
        TargetNode = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY);
        TargetNode.isDiscovered = true;

        Left = GridReference.NodeFromGridPos(TargetNode.iGridX - 1, TargetNode.iGridY);
        Bottom = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY - 1);
        Up = GridReference.NodeFromGridPos(TargetNode.iGridX, TargetNode.iGridY + 1);
        Right = GridReference.NodeFromGridPos(TargetNode.iGridX + 1, TargetNode.iGridY);

        if ((!Left.isWalkable) || (!Up.isWalkable) || (!Right.isWalkable) || (!Bottom.isWalkable))
        {
            tiempo += Time.deltaTime;
        }

        tiempo += Time.deltaTime;

        if (transform.localPosition.x > xMax)
        {
            x = Random.Range(-velocidadMax, 0.0f);
            angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            tiempo = 0.0f;
        }
        if (transform.localPosition.x < xMin)
        {
            x = Random.Range(0.0f, velocidadMax);
            angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            tiempo = 0.0f;
        }
        if (transform.localPosition.z > zMax)
        {
            z = Random.Range(-velocidadMax, 0.0f);
            angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            tiempo = 0.0f;
        }
        if (transform.localPosition.z < zMin)
        {
            z = Random.Range(0.0f, velocidadMax);
            angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            tiempo = 0.0f;
        }

        if (tiempo > 1.0f)
        {
            x = Random.Range(-velocidadMax, velocidadMax);
            z = Random.Range(-velocidadMax, velocidadMax);
            angulo = Mathf.Atan2(x, z) * (180 / 3.141592f) + 90;
            transform.localRotation = Quaternion.Euler(0, angulo, 0);
            tiempo = 0.0f;
        }

        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z + z);
    }
}

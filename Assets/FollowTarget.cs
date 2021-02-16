using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float z = -20;

    [SerializeField]
    private float zmin = -50;

    [SerializeField]
    private float zmax = -10;

    [SerializeField]
    private float zSpeed = 2000;

    private void Update()
    {
        ChangeZ();
        MoveToTarget();
    }

    private void ChangeZ()
    {
        float zIncrement = Input.GetAxis("Mouse ScrollWheel") * zSpeed * Time.deltaTime;
        Debug.Log(zIncrement);
        z += zIncrement;
        if (z > zmin) z = zmin;
        else if (z < zmax) z = zmax;
    }

    private void MoveToTarget()
    {
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, z);
        transform.position = newPosition;
    }
}

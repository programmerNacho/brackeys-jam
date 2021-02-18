using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Camera cam = null;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float size = 5;

    [SerializeField]
    private float sizeMin = 50;

    [SerializeField]
    private float SizeMax = 5;

    [SerializeField]
    private float sizeSpeed = 2000;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        ChangeZ();
        MoveToTarget();
    }

    private void ChangeZ()
    {
        float zIncrement = Input.GetAxis("Mouse ScrollWheel") * sizeSpeed * Time.deltaTime;
        size -= zIncrement;
        if (size > sizeMin) size = sizeMin;
        else if (size < SizeMax) size = SizeMax;

        cam.orthographicSize = size;

    }

    private void MoveToTarget()
    {
        if (target)
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y, -10);
            transform.position = newPosition;
        }
        else
        {
            target = null;
        }
        
    }
}

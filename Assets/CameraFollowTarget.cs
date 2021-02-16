using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target = null;
    [SerializeField]
    private float z = -10;

    private void Update()
    {
        Vector3 targetPosition = target.transform.position;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, z);
    }
}

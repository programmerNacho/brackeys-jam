using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowTarget : MonoBehaviour
{
    private CinemachineVirtualCamera followCamera = null;

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
        followCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if(followCamera.Follow == null && Game.GameManager.Instance.spawnedPlayer)
        {
            followCamera.Follow = Game.GameManager.Instance.spawnedPlayer.transform;
        }

        ChangeZ();
    }

    private void ChangeZ()
    {
        float zIncrement = Input.GetAxis("Mouse ScrollWheel") * sizeSpeed * Time.deltaTime;
        size -= zIncrement;
        if (size > sizeMin) size = sizeMin;
        else if (size < SizeMax) size = SizeMax;
        if (followCamera) followCamera.m_Lens.OrthographicSize = size;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShot : MonoBehaviour
{
    private Game.CoreBlock myCore = null;
    private float tickRate = 0.1f;

    private Game.CoreBlock target = null;

    private Game.CoreBlock[] coresList = new Game.CoreBlock[0];

    private void Start()
    {
        myCore = GetComponent<Game.CoreBlock>();
        if (!myCore) Destroy(this);

        Invoke("SetCoreList", 1);

        Invoke("Tick", tickRate);
    }

    private void SetCoreList()
    {
        coresList = FindObjectsOfType<Game.CoreBlock>();
    }
    private void Tick()
    {
        Invoke("Tick", tickRate);

        FindTarget();
        if (target)
        {
            AimTarget();
        }
    }

    private void FindTarget()
    {
        target = null;
        float distanceToTarget = 0;

        SetCoreList();

        foreach (var coreFound in coresList)
        {
            if (!coreFound)
            {
                continue;
            }

            bool coreIsEnemy = coreFound != myCore && coreFound.CurrentAffiliation != myCore.CurrentAffiliation;

            if (coreIsEnemy && coreFound.isAlive)
            {
                float distanceToCoreFound = Vector2.Distance((Vector2)transform.position, (Vector2)coreFound.transform.position);
                bool coreIsCloser = !target || distanceToCoreFound < distanceToTarget;
                if (coreIsCloser)
                {
                    target = coreFound;
                    distanceToTarget = distanceToCoreFound;
                }
            }
        }
    }

    private void AimTarget()
    {
        Rigidbody2D targetBody = target.GetComponentInParent<Rigidbody2D>();

        Vector2 targetDirection = targetBody.velocity.normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        float targetSpeed = targetBody.velocity.magnitude * distanceToTarget * Time.deltaTime;
        targetSpeed *= 0.8f;

        Vector2 targetPredectePosition = (Vector2)target.transform.position + (targetDirection * targetSpeed);

        foreach (var turret in myCore.GetComponentsInChildren<Game.BlockTurretPower>())
        {
            turret.AimTarget(target, targetPredectePosition, tickRate);
        }
    }
}

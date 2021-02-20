using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShot : MonoBehaviour
{
    private Game.CoreBlock myCore = null;
    private float tickRate = 0.05f;

    private Game.CoreBlock target = null;

    private void Start()
    {
        myCore = GetComponent<Game.CoreBlock>();
        if (!myCore) Destroy(this);

        Invoke("Tick", tickRate);
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

        foreach (var coreFound in FindObjectsOfType<Game.CoreBlock>())
        {
            bool coreIsEnemy = coreFound != myCore && coreFound.CurrentAffiliation != myCore.CurrentAffiliation;

            if (coreIsEnemy)
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
        targetSpeed *= 8f;

        Vector2 targetPredectePosition = (Vector2)target.transform.position + (targetDirection * targetSpeed);

        foreach (var turret in myCore.GetComponentsInChildren<Game.BlockTurretPower>())
        {
            turret.AimTarget(target, targetPredectePosition, tickRate);
        }
    }
}

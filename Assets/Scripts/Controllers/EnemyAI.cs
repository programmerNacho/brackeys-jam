using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private ShipController shipController;

    private enum CombatBehavior
    {
        Pasive,
        Defensive,
        Ofensive
    }

    [SerializeField]
    private CombatBehavior combatBehavior = CombatBehavior.Pasive;

    [SerializeField]
    private float rangeOfVisionAgainstPlayer  = 20;

    [SerializeField]
    private float rangeOfVisionAgainstNodes = 5;

    [SerializeField]
    private LayerMask nodeLayerMask = new LayerMask();

    private Game.CoreBlock playerTarget;
    private Game.SimpleBlock moduleTarget;

    [SerializeField]
    private float wanderTime = 3.0f;
    private bool isWander = false;
    private Vector2 wanderDirection = Vector2.zero;

    private void Start()
    {
        SerializeVariables();
    }

    private void SerializeVariables()
    {
        if (shipController == null) shipController = GetComponent<ShipController>();
        Invoke("Tick", 0.2f);
    }

    private void Tick()
    {
        if (!CheckPlayersInRange())
        {
            if (!CheckNodesInRange())
            {
                Wander();
            }
            else
            {
                isWander = false;
            }
        }
        else
        {
            isWander = false;
        }
        Invoke("Tick", 0.2f);
    }
    private void StopWander()
    {
        isWander = false;
    }

    private void Wander()
    {
        if (!isWander)
        {
            isWander = true;

            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            wanderDirection = new Vector2(x, y);
            shipController.MoveTowardsDirectionAcceleration(wanderDirection);

            CancelInvoke("StopWander");
            Invoke("StopWander", wanderTime);
        }
        else
        {
            if (!shipController.GetMoving())
            {
                isWander = false;
            }
            else
            {
                shipController.MoveTowardsDirectionAcceleration(wanderDirection);
            }
        }
    }

    private bool CheckNodesInRange()
    {
        moduleTarget = null;

        Vector2 sourceCast = transform.position;
        float radio = rangeOfVisionAgainstNodes;

        RaycastHit2D[] blocksInRange = Physics2D.CircleCastAll(sourceCast, radio, Vector2.right, 1, nodeLayerMask);
        float targetDistance = 9999;

        foreach (var item in blocksInRange)
        {
            Game.SimpleBlock block = item.collider.GetComponentInParent<Game.SimpleBlock>();
            if (block)
            {
                bool isFree = block.CurrentAffiliation == Affiliation.Free;
                if (isFree)
                {
                    float moduleDistance = Vector2.Distance(transform.position, block.transform.position);
                    bool itsClose = moduleDistance < targetDistance;
                    if (itsClose)
                    {
                        moduleTarget = block;
                        targetDistance = moduleDistance;
                    }
                }
            }
        }

        if (moduleTarget != null)
        {
            NodeInRange();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckPlayersInRange()
    {
        playerTarget = null;
        Game.CoreBlock[] coresInGame = FindObjectsOfType<Game.CoreBlock>();
        float targetDistance = 9999;

        foreach (var core in coresInGame)
        {
            if (core.CurrentAffiliation == Affiliation.Player)
            {
                float playerDistance = Vector2.Distance(transform.position, core.transform.position);
                bool playerIsNear = playerDistance <= rangeOfVisionAgainstPlayer;
                if (playerIsNear)
                {
                    bool itsClose = playerDistance < targetDistance;
                    if (itsClose)
                    {
                        playerTarget = core;
                        targetDistance = playerDistance;
                    }
                }
            }
        }

        if (playerTarget != null)
        {
            if (combatBehavior == CombatBehavior.Pasive)
            {
                return false;
            }

            EnemyInRange();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private void NodeInRange()
    {
        shipController.GoToPoint(moduleTarget.transform.position);
    }
    private void EnemyInRange()
    {
            switch (combatBehavior)
            {
                case CombatBehavior.Pasive:
                    break;
                case CombatBehavior.Defensive:
                    RunAway();
                    break;
                case CombatBehavior.Ofensive:
                    AttackTarget();
                    break;
                default:
                    break;
            }
    }

    private void RunAway()
    {
        Vector2 direction = transform.position - playerTarget.transform.position;
        shipController.MoveTowardsDirectionAcceleration(direction);
    }

    private void AttackTarget()
    {
        shipController.GoToPoint(playerTarget.transform.position);
    }
}

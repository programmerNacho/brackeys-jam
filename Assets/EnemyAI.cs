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

    private PlayerModule playerTarget;
    private SimpleModule nodeTarget;

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
    }

    private void Update()
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
        nodeTarget = null;

        Vector2 sourceCast = transform.position;
        float radio = rangeOfVisionAgainstNodes / 2;

        RaycastHit2D[] objectInRange = Physics2D.CircleCastAll(sourceCast, radio, Vector2.right, 1, nodeLayerMask);
        float targetDistance = 9999;

        foreach (var item in objectInRange)
        {
            SimpleModule node = null;
            if (item.collider.TryGetComponent<SimpleModule>(out node))
            {
                bool isFree = node.currentState == SimpleModule.State.Free;
                if (isFree)
                {
                    float nodeDistance = Vector2.Distance(transform.position, node.transform.position);
                    bool itsClose = nodeDistance < targetDistance;
                    if (itsClose)
                    {
                        nodeTarget = node;
                        targetDistance = nodeDistance;
                    }
                }
            }
        }

        if (nodeTarget != null)
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
        PlayerModule[] playersInGame = FindObjectsOfType<PlayerModule>();
        float targetDistance = 9999;

        foreach (var player in playersInGame)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            bool playerIsVear = playerDistance <= rangeOfVisionAgainstPlayer;
            if (playerIsVear)
            {
                bool itsClose = playerDistance < targetDistance;
                if (itsClose)
                {
                    playerTarget = player;
                    targetDistance = playerDistance;
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
        shipController.GoToPoint(nodeTarget.transform.position);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private ShipController shipController;

    [SerializeField]
    private float tickTime = 0.2f;
    [SerializeField]
    private float timeToAwake = 1f;
    [SerializeField]
    private float activateDistance = 300;

    private enum CombatBehavior
    {
        Pasive,
        Defensive,
        Ofensive
    }

    [SerializeField]
    private bool flank;

    [SerializeField]
    [Range(1, 10)]
    private float flankTime = 2;
    private float timeToChangeFlankDirection = 0;
    private bool flankDirectionIsRight = false;

    private Game.CoreBlock player = null;

    [SerializeField]
    [Range(1, 50)]
    private float flankAngle = 50;

    [SerializeField]
    private bool rotate;
    private bool isRottating = false;

    [SerializeField]
    private bool lookToTarget = false;

    [SerializeField]
    [Range(1, 10)]
    private float rotateTime = 2;
    private float timeToChangeRotateDirection = 0;
    private float rotateDirectionTarget = 0;

    private bool continuousMove = false;
    private Vector2 moveDirection = Vector2.zero;



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

    private void Start()
    {
        SerializeVariables();
        player = FindObjectOfType<PlayerInput>()?.GetComponent<Game.CoreBlock>();
    }

    private void Update()
    {
        if (!player)
        {
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) > activateDistance)
        {
            return;
        }

        if (rotate)
        {
            shipController.RotateAngleAcceleration(rotateDirectionTarget);
        }
        if (continuousMove)
        {
            shipController.MoveTowardsDirectionAcceleration(moveDirection);
        }
    }

    private void SerializeVariables()
    {
        if (shipController == null) shipController = GetComponent<ShipController>();
        Invoke("Tick", tickTime);
    }

    private void Tick()
    {
        Invoke("Tick", 0.2f);

        if (timeToAwake > 0)
        {
            timeToAwake -= tickTime;
            return;
        }

        Rotate();

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

    private void Rotate()
    {
        if (timeToChangeRotateDirection <= 0)
        {
            rotateDirectionTarget = Random.Range(0, 360);
            timeToChangeRotateDirection = rotateTime;
        }
        else
        {
            timeToChangeRotateDirection -= tickTime;
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

            Vector2 direction = new Vector2(x, y);

            rotateDirectionTarget = shipController.GetAngle(direction);
            moveDirection = direction;
            continuousMove = true;

            CancelInvoke("StopWander");
            Invoke("StopWander", wanderTime);
        }
        else
        {
            if (!shipController.GetMoving())
            {
                isWander = false;
                continuousMove = false;
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
                if (!block.GetCore())
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
        continuousMove = false;
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
        if (flank)
        {
            Vector2 targetDirection = ((Vector2)transform.position - (Vector2)playerTarget.transform.position).normalized;
            FlankMove(targetDirection);
        }
        else
        {
            Vector2 direction = transform.position - playerTarget.transform.position;
            shipController.MoveTowardsDirectionAcceleration(direction);
            continuousMove = false;
        }
    }

    private void AttackTarget()
    {
        if (flank)
        {
            Vector2 targetDirection = ((Vector2)playerTarget.transform.position - (Vector2)transform.position).normalized;
            FlankMove(targetDirection);
        }
        else
        {
            shipController.GoToPoint(playerTarget.transform.position);
            continuousMove = false;
        }
    }

    private void FlankMove(Vector2 targetDirection)
    {
        float targetAngle = shipController.GetAngle(targetDirection);

        if (flankDirectionIsRight)
        {
            targetAngle += flankAngle;
        }
        else
        {
            targetAngle -= flankAngle;
        }

        moveDirection = (Vector2)(Quaternion.Euler(0, 0, targetAngle) * Vector2.up);
        continuousMove = true;
        rotateDirectionTarget = shipController.GetAngle(moveDirection);

        ResetFlankTime();
    }

    private void ResetFlankTime()
    {
        if (timeToChangeFlankDirection <= 0)
        {
            flankDirectionIsRight = !flankDirectionIsRight;
            timeToChangeFlankDirection = flankTime;
        }
        else
        {
            timeToChangeFlankDirection -= tickTime;
        }
    }
}

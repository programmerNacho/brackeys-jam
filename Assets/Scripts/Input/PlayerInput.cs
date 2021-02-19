using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private ShipController shipController = null;

    private Vector2 moveDirection = Vector2.zero;

    private bool shoting = false;

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
        Move();
        LookAtMouse();
        KeyboardRotation();
        Shot();
        Disassemble();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shipController.GoToPoint(targetPosition);
        }
    }

    private void Disassemble()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 source = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = Vector2.zero;

            RaycastHit2D[] hit = Physics2D.RaycastAll(source, direction, 0);

            foreach (var item in hit)
            {
                Game.BlockCenter center = item.collider.GetComponent<Game.BlockCenter>();
                if (center)
                {
                    Game.Block block = center.GetMyBlock();
                    if (block.GetCore()?.CurrentAffiliation == Affiliation.Player)
                    {
                        block.DockManager.DisconnectBlock(block);
                    }
                }
            }
        }

    }
    private void Shot()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    shipController.ShotStart();
        //    shoting = true;
        //}
        //else if (!Input.GetMouseButton(0))
        //{
        //    if (shoting)
        //    {
        //        shoting = false;
        //        shipController.ShotEnd();
        //    }
        //}
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        bool isMoving = x != 0 || y != 0;

        if (isMoving)
        {
            Vector2 direction = new Vector2(x, y);
            shipController.MoveTowardsDirectionAcceleration(direction);
        }
    }

    private void KeyboardRotation()
    {

        // No tiene un else para que se anulen si se pulsan a la vez.
        if (Input.GetKey(KeyCode.Q))
        {
            shipController.RotateContinuous(false);
        }

        if (Input.GetKey(KeyCode.E))
        {
            shipController.RotateContinuous(true);
        }
    }
    private void LookAtMouse()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetLookDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;

            shipController.RotateTowardDirectionInstant(targetLookDirection);
        }
    }
}

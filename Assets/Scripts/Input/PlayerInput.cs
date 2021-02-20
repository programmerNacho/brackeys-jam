using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private ShipController shipController = null;

    private Vector2 moveDirection = Vector2.zero;

    private bool shoting = false;

    private Shop shop = null;



    private void Start()
    {
        SerializeVariables();
        shop = FindObjectOfType<Shop>();

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
        BuyBlock();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shipController.GoToPoint(targetPosition);
        }
    }

    private void BuyBlock()
    {
        if (!shop) return;
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        shop.CheckBlockPosition(mouseWorldPosition);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shop.CreateNewBlock(0, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shop.CreateNewBlock(1, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            shop.CreateNewBlock(2, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            shop.CreateNewBlock(3, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            shop.CreateNewBlock(4, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            shop.CreateNewBlock(5, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            shop.CreateNewBlock(6, mouseWorldPosition);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            shop.CreateNewBlock(7, mouseWorldPosition);
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

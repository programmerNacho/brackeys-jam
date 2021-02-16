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
    }

    private void Disassemble()
    {
        if (Input.GetMouseButtonDown(2))
        {
            shipController.Disassemble();
        }
    }
    private void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shipController.ShotStart();
            shoting = true;
        }
        else if (!Input.GetMouseButton(0))
        {
            if (shoting)
            {
                shoting = false;
                shipController.ShotEnd();
            }
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y);

        shipController.MoveTowardsDirectionAcceleration(direction);
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

            shipController.RotateTowardDirectionAcceleration(targetLookDirection);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private ShipController shipController = null;

    private Vector2 moveDirection = Vector2.zero;

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
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(x, y);
        print(direction);

        shipController.MoveTowardsDirectionAcceleration(direction);
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

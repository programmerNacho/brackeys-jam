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
            Vector2 mousePosition = Input.mousePosition;
            Vector2 centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);

            Vector2 targetLookDirection = (mousePosition - centerScreen).normalized;

            shipController.RotateTowardDirectionAcceleration(targetLookDirection);
        }
    }
}

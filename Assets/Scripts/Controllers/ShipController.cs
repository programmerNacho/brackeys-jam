using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body = null;

    [SerializeField]
    private float rotateSpeed = 10.0f;
    [SerializeField]
    private float countinuousRotateSpeed = 100.0f;

    [SerializeField]
    private float moveSpeed = 1000.0f;
    [SerializeField]
    private float maxSpeed = 6.0f;

    [SerializeField]
    private float shotRate = 0.5f;

    private bool moveToPoint = false;
    private Vector2 targetPosition = Vector2.zero;

    [SerializeField]
    private float stopDistance = 1;

    private bool moving = false;

    public UnityEvent OnShotStart = new UnityEvent();
    public UnityEvent OnShot = new UnityEvent();
    public UnityEvent OnShotEnd = new UnityEvent();
    public UnityEvent OnDisassemble = new UnityEvent();


    private void Start()
    {
        Inicializevariables();
    }

    private void FixedUpdate()
    {
        LimitVelocity();
    }

    private void Update()
    {
        MoveGoToPointContinuous();
        CheckVelocity();
    }

    private void CheckVelocity()
    {
        if (moving)
        {
            if (body.velocity.magnitude < 0.1f)
            {
                moving = false;
            }
        }
        else
        {
            if (body.velocity.magnitude > 0.1f)
            {
                moving = true;
            }
        }
    }

    private void MoveGoToPointContinuous()
    {
        if (moveToPoint)
        {
            bool isToClose = Vector2.Distance(transform.position, targetPosition) <= stopDistance;
            if (isToClose)
            {
                moveToPoint = false;
            }
            else
            {
                MoveTowardToPoint(targetPosition);
            }
        }
    }

    private void LimitVelocity()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }

    private void Inicializevariables()
    {
        body = GetComponent<Rigidbody2D>();
    }

    #region Rotate
    public void RotateContinuous(bool clockwise)
    {
        float angleSpeed = countinuousRotateSpeed;
        if (clockwise) angleSpeed = -countinuousRotateSpeed;

        Vector2 currentLookDirection = transform.up;

        float currentAngle = -GetAngle(currentLookDirection);
        float newAngle = currentAngle + (angleSpeed * Time.deltaTime);

        RotateAngleInstant(newAngle);
    }

    public void RotateTowardDirectionAcceleration(Vector2 targetLookDirection)
    {

        Vector2 currentLookDirection = transform.up;

        Vector2 newLookDirection = currentLookDirection + Vector2.MoveTowards(currentLookDirection, targetLookDirection, rotateSpeed * Time.deltaTime);

        float newAngle = -GetAngle(newLookDirection);

        RotateAngleInstant(newAngle);
    }

    public void RotateTowardDirectionInstant(Vector2 targetLookDirection)
    {
        float angle = -GetAngle(targetLookDirection);
        RotateAngleInstant(angle);
    }

    public void RotateAngleInstant(float angle)
    {
        body.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    #endregion

    public void Teleport(Vector2 destination)
    {
        transform.position = destination;
        moveToPoint = false;
    }

    public void MoveTowardsDirectionAcceleration(Vector2 direcion)
    {
        direcion = direcion.normalized;
        body.AddForce(direcion * moveSpeed * Time.deltaTime, ForceMode2D.Force);
        moveToPoint = false;
    }

    public void MoveTowardToPoint(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        body.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode2D.Force);
    }

    public float GetAngle(Vector2 vector)
    {
        if (vector.x < 0)
        {
            return 360 - (Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
        }
    }

    public void GoToPoint(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
        moveToPoint = true;
    }

    public void ShotStart()
    {
        OnShotStart.Invoke();
        OnShot.Invoke();
        Invoke("Shot", shotRate);
    }

    private void Shot()
    {
        OnShot.Invoke();
        Invoke("Shot", shotRate);
    }

    public void ShotEnd()
    {
        OnShotStart.Invoke();
        CancelInvoke("Shot");
    }

    public void Disassemble()
    {
        OnDisassemble.Invoke();
    }

    public bool GetMoving()
    {
        return moving;
    }
}
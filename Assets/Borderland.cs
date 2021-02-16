using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borderland : MonoBehaviour
{
    [SerializeField]
    private float force = 1200;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Hit");
        Rigidbody2D body = null;
        if (collision.TryGetComponent<Rigidbody2D>(out body))
        {
            Debug.Log("Body");
            body.AddForce(transform.up * force * Time.deltaTime, ForceMode2D.Force);
        }
    }
}

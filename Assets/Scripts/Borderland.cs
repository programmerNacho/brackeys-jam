using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borderland : MonoBehaviour
{
    [SerializeField]
    private float force = 120;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Force");
        Rigidbody2D body = collision.GetComponentInParent<Rigidbody2D>();
        if (body)
        {
            body.AddForce(transform.up * force * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}

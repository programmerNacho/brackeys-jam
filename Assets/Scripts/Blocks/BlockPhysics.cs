using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPhysics : MonoBehaviour
{
    public new Rigidbody2D rigidbody = null;

    public void AddRigidbody2D()
    {
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
        }
        rigidbody.mass = 0.25f;
        rigidbody.drag = 3;
        rigidbody.angularDrag = 3;
        rigidbody.useFullKinematicContacts = true;
        rigidbody.isKinematic = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 0f;
    }

    public void RemoveRigidbody2D()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (body) Destroy(body);
    }

    public void AddExplosionForce(Vector2 sourceExplosion, float force)
    {
        Vector2 explosionDirection = ((Vector2)transform.position - sourceExplosion).normalized;

        rigidbody?.AddForce(explosionDirection * force, ForceMode2D.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPhysics : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody = null;

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
        rigidbody.useFullKinematicContacts = true;
        rigidbody.isKinematic = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.gravityScale = 0f;
    }

    public void RemoveRigidbody2D()
    {
        Destroy(rigidbody);
    }
}

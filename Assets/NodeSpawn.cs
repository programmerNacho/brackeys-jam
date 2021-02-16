using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawn : MonoBehaviour
{
    private Rigidbody2D body = null;

    [SerializeField]
    private float speed = 3;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);

        Vector2 direction = new Vector2(x, y);

        body.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}

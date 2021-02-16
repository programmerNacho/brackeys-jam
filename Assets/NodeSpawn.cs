using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject nodePrefab = null;

    private Rigidbody2D body = null;

    [SerializeField]
    private float speed = 5;

    [SerializeField]
    private float spawnRate = 1;

    [SerializeField]
    private int maxNodes = 200;

    void Start()
    {
        Invoke("CheckNodeCount", spawnRate);
    }

    private void CheckNodeCount()
    {
        int nodes = FindObjectsOfType<SimpleModule>().Length;
        if (nodes < maxNodes) SpawnNode();

        Invoke("CheckNodeCount", spawnRate);
    }
    private void SpawnNode()
    {
        GameObject prefab = Instantiate(nodePrefab, transform.position, transform.rotation);
        body = prefab.GetComponent<Rigidbody2D>();

        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Vector2 direction = new Vector2(x, y);

        body.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Zone : MonoBehaviour
    {
        [SerializeField]
        private float maxDistance = 200f;

        private List<Transform> children = new List<Transform>();

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if(child.gameObject.activeSelf == true)
                {
                    children.Add(child);
                }
            }
        }

        private void Update()
        {
            List<Transform> childsToSeparate = new List<Transform>();

            foreach (Transform child in children)
            {
                if (!child) return;

                float distanceToZone = Vector2.Distance(child.position, transform.position);

                if(distanceToZone >= maxDistance)
                {
                    childsToSeparate.Add(child);
                }
            }

            foreach (Transform child in childsToSeparate)
            {
                children.Remove(child);

                if(child.parent == transform)
                {
                    child.parent = null;
                }
            }

            if(children.Count <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

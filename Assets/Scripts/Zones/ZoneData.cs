using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Zone", menuName = "Objectives/Zone", order = 1)]
    public class ZoneData : ScriptableObject
    {
        public List<GameObject> possibilities = new List<GameObject>();

        public GameObject GetRandomZone()
        {
            return possibilities[Random.Range(0, possibilities.Count)];
        }
    }
}

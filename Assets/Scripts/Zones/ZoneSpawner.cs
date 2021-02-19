using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ZoneSpawner : MonoBehaviour
    {
        [SerializeField]
        private ZoneData zoneData = null;

        private void Start()
        {
            SpawnZone();
        }

        private void SpawnZone()
        {
            Instantiate(zoneData.GetRandomZone(), transform.position, Quaternion.identity);
        }
    }
}

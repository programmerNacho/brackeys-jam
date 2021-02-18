using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RadarTarget : MonoBehaviour
    {
        public string TypeOfTarget = "";

        private void Start()
        {
            if (Radar.Instance)
            {
                Radar.Instance.Subscribe(this);
            }
        }

        private void OnEnable()
        {
            if(Radar.Instance)
            {
                Radar.Instance.Subscribe(this);
            }
        }

        private void OnDisable()
        {
            if(Radar.Instance)
            {
                Radar.Instance.UnSubscribe(this);
            }
        }

        private void OnDestroy()
        {
            if (Radar.Instance)
            {
                Radar.Instance.UnSubscribe(this);
            }
        }
    }
}

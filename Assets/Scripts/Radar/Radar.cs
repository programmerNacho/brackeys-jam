using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Radar : MonoBehaviour
    {
        public static Radar Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [SerializeField]
        private Transform center = null;
        [SerializeField]
        private float range = 50f;
        [SerializeField]
        private List<RadarTarget> radarTargets = new List<RadarTarget>();



        public float Range
        {
            get
            {
                return range;
            }
        }

        private List<RadarTarget> radarTargetsInsideRangeAndOutOfScreen = new List<RadarTarget>();

        private Camera mainCamera = null;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            CalculateRadarTargetsInsideRangeAndOutOfScreen();
        }

        public void Subscribe(RadarTarget newTarget)
        {
            if(radarTargets.Contains(newTarget))
            {
                Debug.LogWarning("RadarTarget already subscribed.");
                return;
            }

            radarTargets.Add(newTarget);
        }

        public void UnSubscribe(RadarTarget subscribedTarget)
        {
            if(!radarTargets.Contains(subscribedTarget))
            {
                Debug.LogWarning("RadarTarget was not subscribed previously.");
                return;
            }

            radarTargets.Remove(subscribedTarget);
        }

        private void CalculateRadarTargetsInsideRangeAndOutOfScreen()
        {
            if(mainCamera == null)
            {
                Debug.LogError("No MainCamera found. Cannot find RadarTargetsInsideRangeAndOutOfScreen");
                return;
            }

            radarTargetsInsideRangeAndOutOfScreen.Clear();

            foreach (RadarTarget t in radarTargets)
            {
                Vector3 radarTargetWorldPosition = t.transform.position;

                if(Vector2.Distance(center.position, radarTargetWorldPosition) <= range)
                {
                    Vector3 radarTargetViewportPosition = mainCamera.WorldToViewportPoint(t.transform.position);

                    bool radarTargetOutsideLeftOrRightOfScreen = radarTargetViewportPosition.x < 0 || radarTargetViewportPosition.x > 1;
                    bool radarTargetOutsideTopOrBottomOfScreen = radarTargetViewportPosition.y < 0 || radarTargetViewportPosition.y > 1;
                    
                    if(radarTargetOutsideLeftOrRightOfScreen || radarTargetOutsideTopOrBottomOfScreen)
                    {
                        radarTargetsInsideRangeAndOutOfScreen.Add(t);
                    }
                }
            }
        }

        public List<RadarTarget> GetRadarTargetsInsideRangeAndOutOfScreen()
        {
            return radarTargetsInsideRangeAndOutOfScreen;
        }
    }
}

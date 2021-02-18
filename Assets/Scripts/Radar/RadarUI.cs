using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RadarUI : MonoBehaviour
    {
        [SerializeField]
        private RadarUIAppearances appearances = null;
        [SerializeField]
        private RadarTargetUI targetUIPrefab = null;
        [SerializeField]
        private float viewportOffset = 0.05f;

        private Radar radar = null;

        private List<RadarTargetUI> radarTargetUIs = new List<RadarTargetUI>();

        private void Start()
        {
            radar = Radar.Instance;
        }

        private void Update()
        {
            AssignRadarTargetsToRadarTargetsUI();
            MoveAndRotateRadarTargetsUI();
        }

        private void AssignRadarTargetsToRadarTargetsUI()
        {
            List<RadarTarget> radarTargets = radar.GetRadarTargetsInsideRangeAndOutOfScreen();

            ResetRadarTargetUINotUsed(radarTargets);

            foreach (RadarTarget target in radarTargets)
            {
                RadarTargetUI uiAssigned = null;

                uiAssigned = GetRadarTargetUIWhoseTargetIs(target);

                if (uiAssigned == null)
                {
                    uiAssigned = GetFirstDeActiveRadarTargetUI();

                    if (uiAssigned == null)
                    {
                        uiAssigned = CreateRadarTargetUIAndAddToList();
                    }
                }

                InitializeRadarTargetUI(uiAssigned, target);
            }
        }

        private void ResetRadarTargetUINotUsed(List<RadarTarget> radarTargets)
        {
            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                bool found = false;

                foreach (RadarTarget target in radarTargets)
                {
                    if (target == targetUI.Target)
                    {
                        found = true;
                    }
                }

                if (found == false)
                {
                    targetUI.Target = null;
                    targetUI.gameObject.SetActive(false);
                }
            }
        }

        private RadarTargetUI GetRadarTargetUIWhoseTargetIs(RadarTarget target)
        {
            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                if (targetUI.Target && targetUI.Target == target)
                {
                    return targetUI;
                }
            }

            return null;
        }


        private RadarTargetUI GetFirstDeActiveRadarTargetUI()
        {
            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                if (targetUI.Target == null)
                {
                    return targetUI;
                }
            }

            return null;
        }

        private RadarTargetUI CreateRadarTargetUIAndAddToList()
        {
            RadarTargetUI radarTargetUI = Instantiate(targetUIPrefab, transform);
            radarTargetUIs.Add(radarTargetUI);

            return radarTargetUI;
        }

        private void InitializeRadarTargetUI(RadarTargetUI targetUI, RadarTarget target)
        {
            targetUI.Target = target;
            targetUI.Appearance = appearances.GetAppearance(target.TypeOfTarget);
            targetUI.gameObject.SetActive(true);
        }

        private void MoveAndRotateRadarTargetsUI()
        {
            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                if(targetUI.Target)
                {
                    
                }
            }
        }
    }
}

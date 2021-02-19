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
        [SerializeField]
        private float maxScaleMultiplier = 1;
        [SerializeField]
        private float minScaleMultiplier = 0.1f;

        private Radar radar = null;
        private Camera mainCamera = null;

        private List<RadarTargetUI> radarTargetUIs = new List<RadarTargetUI>();

        private void Start()
        {
            radar = Radar.Instance;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            AssignRadarTargetsToRadarTargetsUI();
            MoveAndRotateRadarTargetsUI();
            ScaleRadarTargetsUI();
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
        }

        private void MoveAndRotateRadarTargetsUI()
        {
            float viewportOffsetX = 0f;
            float viewportOffsetY = 0f;

            if (Screen.width >= Screen.height)
            {
                viewportOffsetX = viewportOffset;
                viewportOffsetY = viewportOffset + (Screen.width / Screen.height) * viewportOffset;
            }
            else
            {
                viewportOffsetX = viewportOffset + (Screen.width / Screen.height) * viewportOffset;
                viewportOffsetY = viewportOffset;
            }

            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                if(targetUI.Target)
                {
                    Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(targetUI.Target.transform.position);

                    Vector3 UIViewportPosition = Vector3.zero;

                    bool outsideLeft = targetViewportPosition.x < 0;
                    bool outsideRight = targetViewportPosition.x > 1;
                    bool outsideBottom = targetViewportPosition.y < 0;
                    bool outsideTop = targetViewportPosition.y > 1;

                    if ((outsideLeft || outsideRight) && (outsideBottom || outsideTop))
                    {
                        if(outsideRight)
                        {
                            if(outsideBottom)
                            {
                                UIViewportPosition = new Vector2(1 - viewportOffsetX, viewportOffsetY);
                            }
                            else if(outsideTop)
                            {
                                UIViewportPosition = new Vector2(1 - viewportOffsetX, 1 - viewportOffsetY);
                            }
                        }
                        else if(outsideLeft)
                        {
                            if (outsideBottom)
                            {
                                UIViewportPosition = new Vector2(viewportOffsetX, viewportOffsetY);
                            }
                            else if (outsideTop)
                            {
                                UIViewportPosition = new Vector2(viewportOffsetX, 1 - viewportOffsetY);
                            }
                        }
                    }
                    else if(outsideLeft || outsideRight)
                    {
                        if(outsideLeft)
                        {
                            UIViewportPosition = new Vector2(viewportOffsetX, targetViewportPosition.y);
                        }
                        else if(outsideRight)
                        {
                            UIViewportPosition = new Vector2(1 - viewportOffsetX, targetViewportPosition.y);
                        }
                    }
                    else if(outsideBottom || outsideTop)
                    {
                        if(outsideBottom)
                        {
                            UIViewportPosition = new Vector2(targetViewportPosition.x, viewportOffsetY);
                        }
                        else if(outsideTop)
                        {
                            UIViewportPosition = new Vector2(targetViewportPosition.x, 1 - viewportOffsetY);
                        }
                    }

                    UIViewportPosition.x = Mathf.Clamp(UIViewportPosition.x, viewportOffsetX, 1 - viewportOffsetX);
                    UIViewportPosition.y = Mathf.Clamp(UIViewportPosition.y, viewportOffsetY, 1 - viewportOffsetY);

                    targetUI.SetScreenPosition(mainCamera.ViewportToScreenPoint(UIViewportPosition));
                }
            }
        }

        private void ScaleRadarTargetsUI()
        {
            foreach (RadarTargetUI targetUI in radarTargetUIs)
            {
                if (targetUI.Target)
                {
                    float radarRange = radar.Range;
                    float distanceToTarget = Vector2.Distance(targetUI.Target.transform.position, mainCamera.ScreenToWorldPoint(targetUI.transform.position));

                    float scaleMultiplier = Mathf.Lerp(minScaleMultiplier, maxScaleMultiplier, 1 - distanceToTarget / radarRange);
                    targetUI.SetScale(scaleMultiplier);
                }
            }
            
        }
    }
}

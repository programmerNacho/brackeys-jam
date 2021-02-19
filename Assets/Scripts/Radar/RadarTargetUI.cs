using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RadarTargetUI : MonoBehaviour
    {
        [SerializeField]
        private Image targetIcon = null;
        [SerializeField]
        private Transform arrowPivot = null;
        [SerializeField]
        private Vector3 initialLocalScale = Vector3.one;

        private RadarTarget target = null;
        private Sprite appearance = null;

        public RadarTarget Target
        {
            get
            {
                return target;
            }

            set
            {
                target = value;

                gameObject.SetActive(target != null);
            }
        }

        public Sprite Appearance
        {
            get
            {
                return appearance;
            }

            set
            {
                appearance = value;

                if(appearance)
                {
                    targetIcon.sprite = appearance;
                }
            }
        }

        public void SetScreenPosition(Vector2 screenPosition)
        {
            transform.position = screenPosition;

            if(target)
            {
                Vector2 arrowLookDirection = ((Vector2)Camera.main.WorldToScreenPoint(target.transform.position) - screenPosition).normalized;
                arrowPivot.up = arrowLookDirection;
            }
        }

        public void SetScale(float multiplierInitialScale)
        {
            transform.localScale = initialLocalScale * multiplierInitialScale;
        }
    }
}

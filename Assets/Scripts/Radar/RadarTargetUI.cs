using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RadarTargetUI : MonoBehaviour
    {
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
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "RadarTargetsAppearance", menuName = "Radar/RadarTargetsAppearance", order = 1)]
    public class RadarUIAppearances : ScriptableObject
    {
        [System.Serializable]
        public class TargetAppearance
        {
            public string identifier = "";
            public Sprite sprite = null;
        }

        public List<TargetAppearance> appearances = new List<TargetAppearance>();

        public TargetAppearance GetAppearance(string identifier)
        {
            foreach (TargetAppearance a in appearances)
            {
                if (identifier == a.identifier)
                {
                    return a;
                }
            }

            return null;
        }
    }
}

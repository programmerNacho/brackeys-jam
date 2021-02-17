using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BlockTurretPower : BlockPower
    {
        [SerializeField]
        protected int maxNumberOfBoosts = 3;

        protected int currentNumberOfBoosts = 0;

        public virtual void AddBoost()
        {
            currentNumberOfBoosts = Mathf.Clamp(currentNumberOfBoosts + 1, 0, maxNumberOfBoosts + 1);
        }

        public virtual void RemoveBoost()
        {
            currentNumberOfBoosts = Mathf.Clamp(currentNumberOfBoosts - 1, 0, maxNumberOfBoosts + 1);
        }
    }
}

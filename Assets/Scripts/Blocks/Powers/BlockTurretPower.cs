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

        public virtual void SetBoost(int boostCount)
        {
            currentNumberOfBoosts = Mathf.Clamp(boostCount, 0, maxNumberOfBoosts);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockTurretBoostPower : BlockPower
    {
        public override void OnBlockConnected()
        {
            CoreBlock coreBlock = myBlock.GetComponentInParent<CoreBlock>();
            if (coreBlock)
            {
                coreBlock.rateOfFireBoost++;
            }
        }
    }
}

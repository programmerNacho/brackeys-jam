using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockVelocityPower : BlockPower
    {
        public override void OnBlockConnected()
        {
            CoreBlock coreBlock = myBlock.GetComponentInParent<CoreBlock>();
            if (coreBlock)
            {
                coreBlock.speedBoost++;
            }
        }

        public override void OnBlockDisconnected()
        {
        }
    }
}

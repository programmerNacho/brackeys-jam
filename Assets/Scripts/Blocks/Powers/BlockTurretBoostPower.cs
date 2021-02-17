using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockTurretBoostPower : BlockPower
    {
        private List<BlockTurretPower> blockTurretPowersAffected = new List<BlockTurretPower>();

        public override void OnBlockConnected()
        {

        }

        public override void OnBlockDisconnected()
        {

        }
    }
}

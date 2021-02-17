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
            BlockTurretPower[] fromParents = myBlock.GetComponentsInParent<BlockTurretPower>();
            BlockTurretPower[] fromChildren = myBlock.GetComponentsInChildren<BlockTurretPower>();

            List<BlockTurretPower> blockTurretPowersNotAffected = new List<BlockTurretPower>();

            foreach (BlockTurretPower turret in fromParents)
            {
                if(blockTurretPowersAffected.Contains(turret) == false)
                {
                    blockTurretPowersNotAffected.Add(turret);
                }
            }

            foreach (BlockTurretPower turret in fromChildren)
            {
                if(blockTurretPowersAffected.Contains(turret) == false)
                {
                    blockTurretPowersNotAffected.Add(turret);
                }
            }

            foreach (BlockTurretPower turret in blockTurretPowersNotAffected)
            {
                turret.AddBoost();

                blockTurretPowersAffected.Add(turret);
            }
        }

        public override void OnBlockDisconnected()
        {
            foreach (BlockTurretPower turret in blockTurretPowersAffected)
            {
                turret.RemoveBoost();
            }

            blockTurretPowersAffected.Clear();
        }
    }
}

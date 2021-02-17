using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockVelocityPower : BlockPower
    {
        private ShipController shipControllerAffected = null;

        public override void OnBlockConnected()
        {
            if(shipControllerAffected == null)
            {
                shipControllerAffected = myBlock.GetComponentInParent<ShipController>();

                if(shipControllerAffected)
                {
                    // Le aplico el boost.
                }
            }
        }

        public override void OnBlockDisconnected()
        {
            if(shipControllerAffected)
            {
                // Le quito el boost.

                shipControllerAffected = null;
            }
        }
    }
}

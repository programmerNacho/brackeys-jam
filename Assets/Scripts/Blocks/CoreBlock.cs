using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoreBlock : Block
    {
        private ShipController shipController;

        public int rateOfFireBoost = 0;
        public int speedBoost = 0;
        public int weight = 0;

        private void Start()
        {
            shipController = GetComponent<ShipController>();
        }

        public void SetPowers()
        {
            weight = 0;

            rateOfFireBoost = 0;
            speedBoost = 0;

            foreach (var item in GetComponentsInChildren<Block>())
            {
                weight++;
                item.OnSetPowers.Invoke();
            }

            SetShipControllerSpeed();
            SetTurretRateOfFire();
        }

        protected void SetTurretRateOfFire()
        {
            foreach (var turret in GetComponentsInChildren<BlockTurretPower>())
            {
                turret.SetBoost(rateOfFireBoost);
            }
        }

        protected void SetShipControllerSpeed()
        {
            shipController.ChangeWeight(weight);
            shipController.ChangeBoost(speedBoost);
        }

        protected override void CheckOverlaying()
        {
        }
    }
}

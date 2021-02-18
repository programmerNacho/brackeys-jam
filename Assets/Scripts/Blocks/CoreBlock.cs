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

        protected override void Start()
        {
            base.Start();
            shipController = GetComponent<ShipController>();
        }

        public void CheckShipStatus()
        {
            RemoveOtherRigibody2D();
            GetWeight();
            SetPowers();
        }

        private void GetWeight()
        {
            weight = 0;
            foreach (var item in GetComponentsInChildren<Block>())
            {
                weight++;
                item.OnSetPowers.Invoke();
            }
        }
        private void RemoveOtherRigibody2D()
        {
            foreach (var item in GetComponentsInChildren<Rigidbody2D>())
            {
                if (item != GetComponent<Rigidbody2D>()) Destroy(item);
            }
        }

        public void SetPowers()
        {
            rateOfFireBoost = 0;
            speedBoost = 0;

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

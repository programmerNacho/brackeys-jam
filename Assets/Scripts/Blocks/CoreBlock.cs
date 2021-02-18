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
            ResetCore();
            ResetBlockStatus();
            SetPowers();
            SetShipFeatures();
        }

        private void ResetCore()
        {
            weight = 0;
            rateOfFireBoost = 0;
            speedBoost = 0;
        }

        private void ResetBlockStatus()
        {
            foreach (var block in GetChildrensBlocks())
            {
                weight++;

                block.ClearShields();

                Rigidbody2D body = block.GetComponent<Rigidbody2D>();
                bool noIsMyBody = body && block != this;
                if (noIsMyBody) Destroy(body);
            }
        }

        private void SetPowers()
        {
            foreach (var block in GetChildrensBlocks())
            {
                block.OnSetPowers.Invoke();
            }
        }

        public void SetShipFeatures()
        {
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

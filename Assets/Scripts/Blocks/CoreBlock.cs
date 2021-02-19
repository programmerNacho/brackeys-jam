using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoreBlock : Block
    {
        private ShipController shipController;

        [SerializeField]
        private List<Block> blocksList = new List<Block>();

        public int rateOfFireBoost = 0;
        public int speedBoost = 0;
        public int weight = 0;

        public Affiliation CurrentAffiliation = Affiliation.Free;

        protected override void Start()
        {
            base.Start();
            myCore = this;
            shipController = GetComponent<ShipController>();
            blocksList.Add(this);

            CheckShipStatus();
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
            List<Block> blocksToEliminate = new List<Block>();

            foreach (var block in blocksList)
            {
                if (block)
                {
                    weight++;

                    block.ClearShields();

                    Rigidbody2D body = block.GetComponent<Rigidbody2D>();
                    bool noIsMyBody = body && block != this;
                    if (noIsMyBody) Destroy(body);
                }
                else
                {
                    blocksToEliminate.Add(block);
                }
            }

            foreach (var item in blocksToEliminate)
            {
                RemoveBlock(item);
            }
        }

        private void SetPowers()
        {
            foreach (var block in blocksList)
            {
                block.OnSetPowers.Invoke();
            }
        }

        

        

        #region Set
        public void AddBlock(Block newBlock)
        {
            bool newBlockIsAlreadyOnTheList = false;
            foreach (var checkBlock in blocksList)
            {
                if (checkBlock == newBlock)
                {
                    newBlockIsAlreadyOnTheList = true;
                    break;
                }
            }

            if (!newBlockIsAlreadyOnTheList)
            {
                blocksList.Add(newBlock);
            }

            CheckShipStatus();
        }

        public void RemoveBlock(Block newBlock)
        {
            if (newBlock != this)
            {
                blocksList.Remove(newBlock);
            }

            CheckShipStatus();
        }
        #endregion
        #region Process

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
        #endregion
        #region Get
        public List<Block> GetBlockList()
        {
            return blocksList;
        }
        #endregion
    }
}

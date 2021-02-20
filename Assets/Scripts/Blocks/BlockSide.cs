using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockSide : MonoBehaviour
    {
        private Block myBlock = null;

        private bool isLocked = false;
        private bool checkTrigger = false;

        #region Set
        public void SetBlock(Block block)
        {
            myBlock = block;
        }
        public void ToLock()
        {
            isLocked = true;
        }
        public void ToFree()
        {
            isLocked = false;
        }
        public void CheckTrigger()
        {
            checkTrigger = true;
        }
        #endregion
        #region Procces
        private void CheckTriggerCollision(Collider2D collision)
        {
            if (!myBlock)
            {
                return;
            }
            if (myBlock.GetCore())
            {
                BlockSide otherSide = collision.GetComponent<BlockSide>();
                Block otherBlock = otherSide?.GetBlock();

                if (otherBlock)
                {
                    if (!otherBlock.GetCore())
                    {
                        myBlock.TryConnectANewBlock(this, otherSide);
                    }
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isLocked)
            {
                CheckTriggerCollision(collision);
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!isLocked)
            {
                CheckTriggerCollision(collision);
            }
            //if (checkTrigger)
            //{
            //    if (!isLocked)
            //    {
            //        checkTrigger = false;
            //        CheckTriggerCollision(collision);
            //    }
            //}
        }

        public void CheckOverlaying()
        {
            Collider2D collider = GetComponent<Collider2D>();
            RaycastHit2D[] hits = new RaycastHit2D[10];

            collider.Cast(Vector2.zero, hits, 1);

            foreach (var item in hits)
            {
                if (item)
                {
                    BlockSide side = item.collider.GetComponent<BlockSide>();
                    bool noIsMe = side && side != this;
                    if (noIsMe)
                    {
                        bool weHaveTheSameCore = side.myBlock.GetCore() == myBlock.GetCore();
                        if (weHaveTheSameCore)
                        {
                            this.ToLock();
                            side.ToLock();
                        }
                    }
                }
            }
        }
        #endregion
        #region Get
        public Block GetBlock()
        {
            return myBlock;
        }

        public bool GetLocked()
        {
            return isLocked;
        }
        #endregion

        //[SerializeField]
        //private Block blockParent = null;
        //[SerializeField]
        //private Transform dockTransform = null;

        //public bool canDock = true;

        //public Block BlockParent
        //{
        //    get
        //    {
        //        return blockParent;
        //    }
        //}

        //public Vector2 MiddlePointGlobal
        //{
        //    get
        //    {
        //        return dockTransform.position;
        //    }
        //}

        //public Vector2 NormalDirectionGlobal
        //{
        //    get
        //    {
        //        return dockTransform.up;
        //    }
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    BlockSide otherBlockSide = collision.GetComponent<BlockSide>();

        //    bool canDock = otherBlockSide && !IsCollidingWithOwnBlockSide(otherBlockSide) && otherBlockSide.CanBlockParentDock() && CanBlockParentDock();

        //    if (canDock)
        //    {
        //        blockParent.DockTry(otherBlockSide.BlockParent, this, otherBlockSide);
        //    }
        //}

        //private bool IsCollidingWithOwnBlockSide(BlockSide otherBlockSide)
        //{
        //    return otherBlockSide.BlockParent == blockParent;
        //}

        //private bool CanBlockParentDock()
        //{
        //    return blockParent.CanDock;
        //}
    }
}

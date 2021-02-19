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
        private void Start()
        {
            SetMyBlock();
        }
        private void SetMyBlock()
        {
            myBlock = GetComponentInParent<Block>();
            myBlock.AddSide(this);
        }
        private void CheckTriggerCollision(Collider2D collision)
        {
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
            if (checkTrigger)
            {
                if (!isLocked)
                {
                    checkTrigger = false;
                    CheckTriggerCollision(collision);
                }
            }
        }
        #endregion
        #region Get
        public Block GetBlock()
        {
            return myBlock;
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

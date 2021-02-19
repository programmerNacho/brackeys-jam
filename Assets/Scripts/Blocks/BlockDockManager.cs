using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockDockManager : MonoBehaviour
    {
        public void DisconnectBlock(Block block)
        {
            CoreBlock core = block.GetCore();
            foreach (var item in block.gameObject.GetComponentsInChildren<Block>())
            {
                
                if (core)
                {
                    if (!item.gameObject.GetComponent<CoreBlock>())
                    {
                        core.RemoveBlock(item);
                    }
                }
                else break;
                item.SetCore(null);

                item.LockSidesTemporarily();
                item.parentSideConnectedWithMe?.ToFree();
                item.Physics.AddRigidbody2D();

                item.transform.parent = null;
                item.GetCenter().gameObject.layer = LayerMask.NameToLayer("Free");
            }
        }
        public void PrepareBlockToConnect(Block myBlock, BlockSide mySide,Block otherBlock, BlockSide otherside)
        {
            bool iHaveCore = myBlock.GetCore() && !otherBlock.GetCore();
            bool canDock = myBlock.CanDock && otherBlock.CanDock;
            if ( iHaveCore && canDock)
            {
                PlaceOtherBlockInOffsetPosition(myBlock, mySide, otherBlock, otherside);
                if (CheckOverlaying(myBlock, otherBlock))
                {
                    ConnectBlock(myBlock, mySide, otherBlock, otherside);
                }
            }
        }
        private void ConnectBlock(Block myBlock, BlockSide mySide, Block otherBlock, BlockSide otherside)
        {
            SetParentAndCore(myBlock, otherBlock);
            AddToCoreList(myBlock, otherBlock);
            LockSides(mySide, otherBlock, otherside);

            otherBlock.CheckOverlaying();
            otherBlock.Physics.RemoveRigidbody2D();
            otherBlock.GetCenter().gameObject.layer = LayerMask.NameToLayer(myBlock.GetCore().CurrentAffiliation.ToString());
        }
        private bool CheckOverlaying(Block myBlock, Block otherBlock)
        {
            otherBlock.SetCore(myBlock.GetCore());
            return otherBlock.CheckOverlaying();
        }
        private void LockSides(BlockSide mySide, Block otherBlock, BlockSide otherside)
        {
            mySide.ToLock();
            otherside.ToLock();
            otherBlock.parentSideConnectedWithMe = mySide;
        }
        private void PlaceOtherBlockInOffsetPosition(Block myBlock, BlockSide mySide, Block otherBLock, BlockSide otherside)
        {
            Vector2 myBlockPosition = myBlock.transform.position;
            Vector2 mySidePosition = mySide.transform.position;
            Vector2 otherBlockPosition = otherBLock.transform.position;
            Vector2 otherSidePosition = otherside.transform.position;

            Vector2 dockDirection = (mySidePosition - myBlockPosition).normalized;
            float distanceToMySide = Vector2.Distance(mySidePosition, myBlockPosition);
            float distanceOtherBLockToOtherSide = Vector2.Distance(otherSidePosition, otherBlockPosition);
            float offsetDistance = distanceToMySide + distanceOtherBLockToOtherSide;

            Vector2 dockPosition = myBlockPosition + (dockDirection * offsetDistance);

            otherBLock.transform.rotation = mySide.transform.rotation;
            otherBLock.transform.position = dockPosition;
        }

        private void SetParentAndCore(Block myBlock, Block otherBlock)
        {
            otherBlock.transform.parent = myBlock.transform;
            otherBlock.SetCore(myBlock.GetCore());
        }

        private void AddToCoreList(Block myBlock, Block otherBlock)
        {
            CoreBlock myBlockCore = myBlock.GetCore();
            foreach (var item in otherBlock.GetComponentsInChildren<Block>())
            {
                myBlockCore.AddBlock(item);
            }
        }

        //public void Dock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        //{
        //    RotateOtherBlock(myBlockSide, otherBlockSide, otherBlock);
        //    MoveOtherBlock(myBlockSide, otherBlockSide, otherBlock);
        //}

        //private void RotateOtherBlock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        //{
        //    Vector2 inverseMyBlockSideNormal = -myBlockSide.NormalDirectionGlobal;
        //    Vector2 otherBlockSideNormal = otherBlockSide.NormalDirectionGlobal;
        //    float angleRotationZ = Vector2.SignedAngle(otherBlockSideNormal, inverseMyBlockSideNormal);
        //    otherBlock.transform.Rotate(Vector3.forward, angleRotationZ);
        //}

        //private void MoveOtherBlock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        //{
        //    Vector2 myDockPoint = myBlockSide.MiddlePointGlobal;
        //    Vector2 otherDockPoint = otherBlockSide.MiddlePointGlobal;
        //    Vector2 translationOtherBlock = myDockPoint - otherDockPoint;
        //    otherBlock.transform.position = (Vector2)otherBlock.transform.position + translationOtherBlock;
        //}
    }
}

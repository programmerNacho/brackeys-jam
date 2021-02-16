using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockDock : MonoBehaviour
    {
        public void Dock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        {
            RotateOtherBlock(myBlockSide, otherBlockSide, otherBlock);
            MoveOtherBlock(myBlockSide, otherBlockSide, otherBlock);
        }

        private void RotateOtherBlock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        {
            Vector2 inverseMyBlockSideNormal = -myBlockSide.NormalDirectionGlobal;
            Vector2 otherBlockSideNormal = otherBlockSide.NormalDirectionGlobal;
            float angleRotationZ = Vector2.SignedAngle(otherBlockSideNormal, inverseMyBlockSideNormal);
            otherBlock.transform.Rotate(Vector3.forward, angleRotationZ);
        }

        private void MoveOtherBlock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        {
            Vector2 myDockPoint = myBlockSide.MiddlePointGlobal;
            Vector2 otherDockPoint = otherBlockSide.MiddlePointGlobal;
            Vector2 translationOtherBlock = myDockPoint - otherDockPoint;
            otherBlock.transform.position = (Vector2)otherBlock.transform.position + translationOtherBlock;
        }
    }
}

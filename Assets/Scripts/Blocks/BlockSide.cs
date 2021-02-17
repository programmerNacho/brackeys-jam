using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockSide : MonoBehaviour
    {
        [SerializeField]
        private Block blockParent = null;
        [SerializeField]
        private Transform dockTransform = null;

        public bool canDock = true;

        public Block BlockParent
        {
            get
            {
                return blockParent;
            }
        }

        public Vector2 MiddlePointGlobal
        {
            get
            {
                return dockTransform.position;
            }
        }

        public Vector2 NormalDirectionGlobal
        {
            get
            {
                return dockTransform.up;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BlockSide otherBlockSide = collision.GetComponent<BlockSide>();

            bool canDock = otherBlockSide && !IsCollidingWithOwnBlockSide(otherBlockSide) && otherBlockSide.CanBlockParentDock() && CanBlockParentDock();

            if (canDock)
            {
                blockParent.DockTry(otherBlockSide.BlockParent, this, otherBlockSide);
            }
        }

        private bool IsCollidingWithOwnBlockSide(BlockSide otherBlockSide)
        {
            return otherBlockSide.BlockParent == blockParent;
        }

        private bool CanBlockParentDock()
        {
            return blockParent.CanDock;
        }
    }
}

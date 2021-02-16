using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Block : MonoBehaviour
    {
        [SerializeField]
        protected Block parentBlock = null;
        [SerializeField]
        protected List<Block> childBlocks = new List<Block>();
        [SerializeField]
        protected float dockCooldownAfterDisconnect = 2f;

        public bool CanDock { get; private set; } = true;

        public abstract void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide);

        public void ConnectBlock(Block otherBlock, bool isNewParent)
        {
            if (otherBlock == null)
            {
                Debug.LogError("otherBlock is null.");
                return;
            }

            bool iHaveParent = parentBlock != null;

            if (iHaveParent && parentBlock == otherBlock)
            {
                Debug.LogError("otherBlock is already parent.");
                return;
            }

            if (childBlocks.Contains(otherBlock))
            {
                Debug.LogError("otherBlock is already connected.");
                return;
            }

            if (isNewParent)
            {
                otherBlock.ConnectToChildBlocks(this);
                SetBlockParent(otherBlock);
            }
            else
            {
                ConnectToChildBlocks(otherBlock);
                otherBlock.SetBlockParent(this);
            }

            Debug.Log("Conecto");
        }

        public abstract void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection);

        public void DisconnectFromParent()
        {
            if(parentBlock)
            {
                parentBlock.DisconnectFromChildBlocks(this);
                transform.parent = null;
                Debug.Log("Desconexión");
            }
        }

        protected void ConnectToChildBlocks(Block block)
        {
            childBlocks.Add(block);
        }

        protected void DisconnectFromChildBlocks(Block block)
        {
            childBlocks.Remove(block);
        }

        protected void SetBlockParent(Block newParentBlock)
        {
            if (parentBlock != null)
            {

                ConnectToChildBlocks(parentBlock);
                parentBlock.DisconnectFromChildBlocks(this);

                if (newParentBlock != null)
                {
                    parentBlock = newParentBlock;
                    transform.parent = newParentBlock.transform;
                }

                foreach (Block child in childBlocks)
                {
                    child.SetBlockParent(this);
                }
            }
            else
            {
                parentBlock = newParentBlock;
                transform.parent = newParentBlock.transform;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision) { }
        protected virtual void OnTriggerEnter2D(Collider2D collision) { }
    }
}

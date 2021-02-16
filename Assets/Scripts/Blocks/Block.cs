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
        [SerializeField]
        protected Affiliation currentAffiliation = Affiliation.Free;
        [SerializeField]
        protected BlockPhysics blockPhysics = null;
        [SerializeField]
        protected BlockDock blockDock = null;

        public Affiliation CurrentAffiliation 
        {
            get
            {
                return currentAffiliation;
            }
            private set
            {
                currentAffiliation = value;
            }
        }

        public bool CanDock { get; private set; } = true;

        public abstract void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide);

        public void Dock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        {
            blockDock.Dock(myBlockSide, otherBlockSide, otherBlock);   
        }

        public void ChangeBlockAndChildBlocksAffiliation(Affiliation newAffiliation)
        {
            currentAffiliation = newAffiliation;
            foreach (Block child in childBlocks)
            {
                if(child.CurrentAffiliation != newAffiliation && child.transform.parent == transform)
                {
                    child.ChangeBlockAndChildBlocksAffiliation(newAffiliation);
                }
            }
        }

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
                RemovePhysics();
            }
            else
            {
                ConnectToChildBlocks(otherBlock);
                otherBlock.SetBlockParent(this);
                otherBlock.RemovePhysics();
            }
        }

        protected void AddPhysics()
        {
            blockPhysics.AddRigidbody2D();
        }
        protected void RemovePhysics()
        {
            blockPhysics.RemoveRigidbody2D();
        }

        public abstract void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection);

        public void DisconnectFromParent()
        {
            if(parentBlock)
            {
                parentBlock.DisconnectFromChildBlocks(this);
            }

            parentBlock = null;
            transform.parent = null;

            AddPhysics();
            ChangeBlockAndChildBlocksAffiliation(Affiliation.Free);
            StartCoroutine(CanDockCooldown());
        }

        protected IEnumerator CanDockCooldown()
        {
            CanDock = false;
            yield return new WaitForSeconds(dockCooldownAfterDisconnect);
            CanDock = true;
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
                    transform.SetParent(parentBlock.transform, true);
                }

                foreach (Block child in childBlocks)
                {
                    if(child.parentBlock != this && child.transform.parent == transform)
                    {
                        child.SetBlockParent(this);
                    }
                }
            }
            else
            {
                parentBlock = newParentBlock;
                transform.SetParent(parentBlock.transform, true);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block)
            {
                if (CurrentAffiliation == Affiliation.Player && block.CurrentAffiliation == Affiliation.Enemy ||
                   CurrentAffiliation == Affiliation.Enemy && block.CurrentAffiliation == Affiliation.Player)
                {
                    Vector2 globalImpactPoint = collision.GetContact(0).point;
                    Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

                    Attacked(globalImpactPoint, globalImpactDirection);
                }
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block)
            {
                Debug.Log(block.gameObject.name);
                if (CurrentAffiliation == Affiliation.Player && block.CurrentAffiliation == Affiliation.Enemy ||
                   CurrentAffiliation == Affiliation.Enemy && block.CurrentAffiliation == Affiliation.Player)
                {
                    Vector2 globalImpactPoint = collision.transform.position;
                    Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

                    Attacked(globalImpactPoint, globalImpactDirection);
                }
            }
        }
    }
}

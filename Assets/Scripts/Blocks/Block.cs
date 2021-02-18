using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public abstract class Block : MonoBehaviour
    {
        public List<Block> childBlocks = new List<Block>();
        [SerializeField]
        protected float dockCooldownAfterDisconnect = 2f;
        [SerializeField]
        protected Affiliation currentAffiliation = Affiliation.Free;
        [SerializeField]
        protected BlockPhysics blockPhysics = null;
        [SerializeField]
        protected BlockDock blockDock = null;
        [SerializeField]
        private Collider2D centerCollider = null;

        [SerializeField]
        protected float overlayingRadio = 0.5f;

        protected Block oldParentBlock = null;

        public UnityEvent OnConnect = new UnityEvent();
        public UnityEvent OnSetPowers = new UnityEvent();

        public Affiliation CurrentAffiliation 
        {
            get
            {
                return currentAffiliation;
            }
            private set
            {
                currentAffiliation = value;
                if (centerCollider)
                    centerCollider.gameObject.layer = LayerMask.NameToLayer(currentAffiliation.ToString());
            }
        }

        protected virtual void Start()
        {
            OnConnect.AddListener(CheckOverlaying);
            DamageManager = gameObject.AddComponent<BlockDamage>();
            DamageManager.myBlock = this;
        }

        public bool CanDock { get; private set; } = true;
        public BlockDamage DamageManager { get; private set; } = null;

        public void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            if (otherBlock != null && !(otherBlock is CoreBlock))
            {
                if (CurrentAffiliation != Affiliation.Free)
                {
                    bool isFree = otherBlock.CurrentAffiliation == Affiliation.Free;

                    if (isFree)
                    {
                        PrepareDock(otherBlock, mySide, otherSide);
                    }
                }
                else
                {
                    bool isFree = otherBlock.CurrentAffiliation == Affiliation.Free;

                    if (isFree)
                    {
                        otherBlock.InitiateDockCooldown();
                        PrepareDock(otherBlock, mySide, otherSide);
                    }
                }
            }
        }

        private void ResetCorePowers()
        {
            CoreBlock core = GetComponentInParent<CoreBlock>();
            if (core)
            {
                core.CheckShipStatus();
            }
        }

        private void PrepareDock(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            otherSide.GetComponentInParent<Rigidbody2D>()?.GetComponent<Block>()?.RemovePhysics();
            ConnectTargetBlockWithThis(otherBlock);
            Dock(mySide, otherSide, otherBlock);

            ResetCorePowers();
        }

        public void Dock(BlockSide myBlockSide, BlockSide otherBlockSide, Block otherBlock)
        {
            blockDock.Dock(myBlockSide, otherBlockSide, otherBlock);   
        }

        public void ChangeBlockAndChildBlocksAffiliation(Affiliation newAffiliation)
        {
            CurrentAffiliation = newAffiliation;
            foreach (Block child in childBlocks)
            {
                if(child.CurrentAffiliation != newAffiliation && child.transform.parent == transform)
                {
                    child.ChangeBlockAndChildBlocksAffiliation(newAffiliation);
                }
            }
        }

        public void ConnectTargetBlockWithThis(Block targetBlock)
        {
            if (targetBlock == null)
            {
                Debug.LogError("otherBlock is null.");
                return;
            }
            Block parent = GetComponentInParent<Block>();

            bool targetBlockIsMyParent = parent != null && parent == targetBlock;

            if (targetBlockIsMyParent)
            {
                Debug.LogError("otherBlock is already parent.");
                return;
            }

            if (childBlocks.Contains(targetBlock))
            {
                Debug.LogError("otherBlock is already connected.");
                return;
            }

            SaveParents(targetBlock);

            targetBlock.transform.parent = transform;

            RecalculateHierarchy(targetBlock);

            ConnectToChildBlocks(targetBlock);

            targetBlock.ChangeBlockAndChildBlocksAffiliation(CurrentAffiliation);
        }

        protected void RecalculateHierarchy(Block block)
        {
            if (block)
            {
                Block parent = block.transform.parent?.GetComponent<Block>();
                if (parent)
                {
                    block.DisconnectFromChildBlocks(parent);

                    if (block.oldParentBlock)
                    {
                        block.ConnectToChildBlocks(block.oldParentBlock);
                        block.oldParentBlock.transform.parent = block.transform;
                        RecalculateHierarchy(block.oldParentBlock);
                    }
                }
            }
        }

        protected void SaveParents(Block block)
        {
            block.SetOldParent();

            foreach (var item in block.GetComponentsInChildren<Block>())
            {
                item.SetOldParent();
            }
            foreach (var item in block.GetComponentsInParent<Block>())
            {
                item.SetOldParent();
            }
        }

        public void SetOldParent()
        {
            oldParentBlock = null;

            if (transform.parent != null)
            {
                oldParentBlock = transform.parent.GetComponent<Block>();
            }
        }

        public void InitiateDockCooldown()
        {
            StopCoroutine(CanDockCooldown());
            StartCoroutine(CanDockCooldown());
            foreach (var item in GetComponentsInChildren<Block>())
            {
                item.StopCoroutine(CanDockCooldown());
                item.StartCoroutine(CanDockCooldown());
            }
        }

        protected void AddPhysics()
        {
            bool imCore = GetComponent<CoreBlock>();
            if (!imCore)
            {
                blockPhysics.AddRigidbody2D();
            }
        }
        protected void RemovePhysics()
        {
            bool imCore = GetComponent<CoreBlock>();
            if (!imCore)
            {
                blockPhysics.RemoveRigidbody2D();
            }
        }

        public void DisconnectFromParent()
        {
            Block parent = null;
            parent = transform.parent?.GetComponent<Block>();

            if (parent)
            {
                parent.DisconnectFromChildBlocks(this);
            }

            transform.parent = null;
            ChangeBlockAndChildBlocksAffiliation(Affiliation.Free);
            InitiateDockCooldown();
            AddPhysics();

            if (parent)
            {
                parent.ResetCorePowers();
            }
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

        protected abstract void CheckOverlaying();
    }
}

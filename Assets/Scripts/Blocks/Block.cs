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

        public int health = 2;
        protected int currentHealth = 0;

        protected List<BlockPowerShield> shields = new List<BlockPowerShield>();
        private int shieldRange = -1;

        [SerializeField]
        protected float overlayingRadio = 0.5f;

        [SerializeField]
        private bool simultaneousDock = false;

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

            if (!DamageManager)
            {
                DamageManager = gameObject.AddComponent<BlockDamage>();
                DamageManager.myBlock = this;
            }

            if (!blockDock)
            {
                blockDock = gameObject.AddComponent<BlockDock>();
            }
            

            currentHealth = health;
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
                        if (!simultaneousDock)
                        {
                            otherBlock.InitiateDockCooldown(0.01f);
                            InitiateDockCooldown(0.01f);
                        }

                        PrepareDock(otherBlock, mySide, otherSide);
                    }
                }
                else
                {
                    bool isFree = otherBlock.CurrentAffiliation == Affiliation.Free;

                    if (isFree)
                    {
                        if (!simultaneousDock)
                        {
                            otherBlock.InitiateDockCooldown(0.01f);
                            InitiateDockCooldown(0.01f);
                        }
                        PrepareDock(otherBlock, mySide, otherSide);
                    }
                }
            }
        }

        private void ResetCorePowers()
        {
            CoreBlock core = GetComponentInParent<CoreBlock>();
            if (core) core.CheckShipStatus();
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
                if (child)
                {
                    if (child.CurrentAffiliation != newAffiliation && child.transform.parent == transform)
                    {
                        child.ChangeBlockAndChildBlocksAffiliation(newAffiliation);
                    }
                }
            }
        }

        public void ConnectTargetBlockWithThis(Block targetBlock)
        {
            if (targetBlock == null)
            {
                Debug.LogWarning("otherBlock is null.");
                return;
            }
            Block parent = GetComponentInParent<Block>();

            bool targetBlockIsMyParent = parent != null && parent == targetBlock;

            if (targetBlockIsMyParent)
            {
                Debug.LogWarning("otherBlock is already parent.");
                return;
            }

            if (childBlocks.Contains(targetBlock))
            {
                Debug.LogWarning("otherBlock is already connected.");
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

        public void InitiateDockCooldown(float time)
        {
            StopCoroutine(CanDockCooldown(time));
            StartCoroutine(CanDockCooldown(time));

            foreach (var item in GetComponentsInChildren<Block>())
            {
                item.StopCoroutine(CanDockCooldown(time));
                item.StartCoroutine(CanDockCooldown(time));
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
            if (parent)
            {
                parent = transform.parent?.GetComponent<Block>();
                parent.DisconnectFromChildBlocks(this);
            }

            transform.parent = null;

            if (!GetComponent<CoreBlock>()) ChangeBlockAndChildBlocksAffiliation(Affiliation.Free);

            InitiateDockCooldown(dockCooldownAfterDisconnect);
            AddPhysics();

            if (parent)
            {
                parent.ResetCorePowers();
            }

            shields.Clear();
        }

        protected IEnumerator CanDockCooldown(float time)
        {
            CanDock = false;
            yield return new WaitForSeconds(time);
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

        public Block[] GetChildrensBlocks()
        {
            List<Block> childrensBlocks = new List<Block>();

            foreach (var childrenBlock in GetComponentsInChildren<Block>())
            {
                if (childrenBlock) childrensBlocks.Add(childrenBlock);
            }

            return childrensBlocks.ToArray();
        }
        public Block[] GetNearbyBlocks()
        {
            List<Block> nearbyBlocks = new List<Block>();
            nearbyBlocks.AddRange(GetChildrensBlocks());

            Block parent = transform.parent?.GetComponent<Block>();
            if (parent) nearbyBlocks.Add(parent);

            return nearbyBlocks.ToArray();
        }

        public void AddShield(BlockPowerShield shield, int range)
        {
            shields.Add(shield);

            shieldRange = range - 1;

            if (shieldRange > 0)
            {
                foreach (var targetBlock in GetNearbyBlocks())
                {
                    bool iAmSubscribe = false;
                    foreach (var item in shields)
                    {
                        if (item == shield)
                        {
                            iAmSubscribe = true;
                            break;
                        }
                    }

                    if (!iAmSubscribe)
                    {
                        targetBlock.AddShield(shield, shieldRange);
                    }
                }
            }
            ResetShieldDistance();
        }

        public BlockPowerShield[] GetShields()
        {
            return shields.ToArray();
        }
        public void ClearShields()
        {
            shields.Clear();
        }
        public int GetShieldDistance()
        {
            return shieldRange;
        }

        public void ResetShieldDistance()
        {
            this.shieldRange = 0;
        }
    }
}

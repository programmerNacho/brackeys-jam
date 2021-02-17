using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public abstract class Block : MonoBehaviour
    {
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

        private Block oldParentBlock = null;

        public UnityEvent OnConnect = new UnityEvent();
        public UnityEvent OnDisconnect = new UnityEvent();
        public UnityEvent OnDestroy = new UnityEvent();
        public UnityEvent OnCreate = new UnityEvent();

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

        private void PrepareDock(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            otherSide.GetComponentInParent<Rigidbody2D>()?.GetComponent<Block>()?.RemovePhysics();
            ConnectTargetBlockWithThis(otherBlock);
            Dock(mySide, otherSide, otherBlock);
            OnConnect.Invoke();
        }

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

        public abstract void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection);

        public void DisconnectFromParent()
        {
            Block parent = transform.parent?.GetComponent<Block>();

            if (parent)
            {
                parent.DisconnectFromChildBlocks(this);
            }

            transform.parent = null;
            ChangeBlockAndChildBlocksAffiliation(Affiliation.Free);
            InitiateDockCooldown();
            AddPhysics();

            OnDisconnect.Invoke();

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

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Block block = collision.gameObject.GetComponent<Block>();
            if (block)
            {
                bool isMyEnemy = CurrentAffiliation == Affiliation.Player && block.CurrentAffiliation == Affiliation.Enemy ||
                   CurrentAffiliation == Affiliation.Enemy && block.CurrentAffiliation == Affiliation.Player;

                if (isMyEnemy)
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

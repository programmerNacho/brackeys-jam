using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class BlockDamage : MonoBehaviour
    {
        public Block myBlock = null;
        public float destroyExplosionForce = 0;

        public UnityEvent OnBlockDestroyed = new UnityEvent();
        public UnityEvent OnBlockTakeDamage = new UnityEvent();
        public UnityEvent OnBlockShieldDamage = new UnityEvent();

        private void Awake()
        {
            myBlock = GetComponent<Block>();
        }

        public virtual void TakeDamage(bool realDamage)
        {
            if (realDamage)
            {
                HurtBlock();
            }
            else
            {
                if (!CheckShields())
                {
                    HurtBlock();
                }
            }

        }
        private bool CheckShields()
        {
            bool iAmProtected = false;
            foreach (var shield in myBlock.GetShields())
            {
                if (shield.currentShieldHealth > 0)
                {
                    OnBlockShieldDamage.Invoke();
                    shield.currentShieldHealth--;
                    iAmProtected = true;
                    break;
                }
            }

            return iAmProtected;
        }

        private void HurtBlock()
        {
            if (myBlock.CurrentHealth > 1)
            {
                myBlock.CurrentHealth--;
                myBlock.DockManager.DisconnectBlock(myBlock);
                AddExplosionForceInParent();
                OnBlockTakeDamage.Invoke();
            }
            else
            {
                BlockDestroy();
            }
        }

        private void AddExplosionForceInParent()
        {
            GetComponentInParent<Rigidbody2D>()?.GetComponent<BlockPhysics>()?.AddExplosionForce(transform.position, destroyExplosionForce);
        }

        protected virtual void BlockDestroy()
        {
            myBlock.DockManager.DisconnectBlock(myBlock);
            NotifyTheLevelManager();
            Destroy(gameObject, 0.05f);
            OnBlockDestroyed.Invoke();
        }

        private void NotifyTheLevelManager()
        {
            CoreBlock core = gameObject.GetComponent<CoreBlock>();
            if (core)
            {
                LevelManager levelManager = FindObjectOfType<LevelManager>();
                if (levelManager)
                {
                    levelManager.CheckTargetsRemainig(core);
                }
            }

        }
    }
}


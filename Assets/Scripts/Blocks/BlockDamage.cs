using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class BlockDamage : MonoBehaviour
    {
        public Block myBlock = null;
        public float destroyExplosionForce = 5;

        public virtual void TakeDamage()
        {
            if (myBlock.health > 1)
            {
                myBlock.health--;
                Disconnect();
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

        private void Disconnect()
        {
            Block[] tempChildrenBlock = new Block[myBlock.childBlocks.Count];
            myBlock.childBlocks.CopyTo(tempChildrenBlock);

            foreach (var childrenBlock in tempChildrenBlock)
            {
                if (childrenBlock)
                {
                    childrenBlock.DisconnectFromParent();

                    BlockPhysics blockPhysics = childrenBlock.GetComponent<BlockPhysics>();
                    blockPhysics.AddExplosionForce(transform.position, destroyExplosionForce);
                }
            }

            myBlock.DisconnectFromParent();

            AddExplosionForceInParent();
        }
        protected virtual void BlockDestroy()
        {
            Disconnect();
            Destroy(gameObject);
        }
    }
}


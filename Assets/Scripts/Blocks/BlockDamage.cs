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
            Block[] tempChildrenBlock = new Block[myBlock.childBlocks.Count];
            myBlock.childBlocks.CopyTo(tempChildrenBlock);

            foreach (var childrenBlock in tempChildrenBlock)
            {
                childrenBlock.DisconnectFromParent();

                BlockPhysics blockPhysics = childrenBlock.GetComponent<BlockPhysics>();
                blockPhysics.AddExplosionForce(transform.position, destroyExplosionForce);
            }

            myBlock.DisconnectFromParent();

            GetComponentInParent<Rigidbody2D>()?.GetComponent<BlockPhysics>()?.AddExplosionForce(transform.position, destroyExplosionForce);
            BlockDestroy();
        }
        protected virtual void BlockDestroy()
        {
            Destroy(gameObject);
        }
    }
}


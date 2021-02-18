using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockCenter : MonoBehaviour
    {
        [SerializeField]
        private Block blockParent = null;
        public Block BlockParent
        {
            get
            {
                return blockParent;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Block collisionBlock = collision.collider.GetComponent<BlockCenter>()?.BlockParent;
            if (collisionBlock)
            {
                bool isMyEnemy = BlockParent.CurrentAffiliation == Affiliation.Player && collisionBlock.CurrentAffiliation == Affiliation.Enemy ||
                   BlockParent.CurrentAffiliation == Affiliation.Enemy && collisionBlock.CurrentAffiliation == Affiliation.Player;

                if (isMyEnemy)
                {
                    SpriteRenderer mesh = BlockParent.GetComponentInChildren<SpriteRenderer>();
                    mesh.color = Color.yellow;
                    SpriteRenderer meshB = collisionBlock.GetComponentInChildren<SpriteRenderer>();
                    mesh.color = Color.red;

                    Vector2 globalImpactPoint = collision.GetContact(0).point;
                    Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

                    collisionBlock.DamageManager.TakeDamage();
                    BlockParent.DamageManager.TakeDamage();
                }
            }
            else if(collision.transform.tag == "Damager")
            {
                Destroy(collision.gameObject);
                BlockParent.DamageManager.TakeDamage(); 
            }
        }
    }
}

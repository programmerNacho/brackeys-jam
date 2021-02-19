using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockCenter : MonoBehaviour
    {
        [SerializeField]
        private Block myBlock = null;

        private void Start()
        {
            SetMyBlock();
        }
        private void SetMyBlock()
        {
            myBlock = GetComponentInParent<Block>();
            myBlock.AddCenter(this);
        }
        public bool CheckOverlaying()
        {
            Collider2D collider = GetComponent<Collider2D>();
            RaycastHit2D[] hits = new RaycastHit2D[10];

            collider.Cast(Vector2.zero, hits, 1);

            foreach (var item in hits)
            {
                if (item)
                {
                    BlockCenter center = item.collider.GetComponent<BlockCenter>();
                    bool noIsMe = center && center != this;
                    if (noIsMe)
                    {
                        bool weDontHaveTheSameCore = center.myBlock.GetCore() != myBlock.GetCore();
                        if (weDontHaveTheSameCore)
                        {
                            myBlock.Damage.TakeDamage(true);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!myBlock)
            {
                return;
            }
            Block collisionBlock = collision.collider.GetComponent<BlockCenter>()?.GetMyBlock();
            if (collisionBlock)
            {
                if (!collisionBlock.GetCore() || !myBlock.GetCore())
                {
                    return;
                }

                bool isMyEnemy = myBlock.GetCore().CurrentAffiliation == Affiliation.Player && collisionBlock.GetCore().CurrentAffiliation == Affiliation.Enemy ||
                   myBlock.GetCore().CurrentAffiliation == Affiliation.Enemy && collisionBlock.GetCore().CurrentAffiliation == Affiliation.Player;

                if (isMyEnemy)
                {
                    Vector2 globalImpactPoint = collision.GetContact(0).point;
                    Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

                    collisionBlock.Damage.TakeDamage(true);
                    myBlock.Damage.TakeDamage(true);
                }
            }
            else if (collision.transform.tag == "Damager")
            {
                Destroy(collision.gameObject);
                myBlock.Damage.TakeDamage(false);
            }
        }

        public Block GetMyBlock()
        {
            return myBlock;
        }

        //[SerializeField]
        //private Block blockParent = null;
        //public Block BlockParent
        //{
        //    get
        //    {
        //        return blockParent;
        //    }
        //}

        //protected virtual void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Block collisionBlock = collision.collider.GetComponent<BlockCenter>()?.BlockParent;
        //    if (collisionBlock)
        //    {
        //        bool isMyEnemy = BlockParent.CurrentAffiliation == Affiliation.Player && collisionBlock.CurrentAffiliation == Affiliation.Enemy ||
        //           BlockParent.CurrentAffiliation == Affiliation.Enemy && collisionBlock.CurrentAffiliation == Affiliation.Player;

        //        if (isMyEnemy)
        //        {
        //            Vector2 globalImpactPoint = collision.GetContact(0).point;
        //            Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

        //            collisionBlock.DamageManager.TakeDamage();
        //            BlockParent.DamageManager.TakeDamage();
        //        }
        //    }
        //    else if(collision.transform.tag == "Damager")
        //    {
        //        Destroy(collision.gameObject);
        //        BlockParent.DamageManager.TakeDamage(); 
        //    }
        //}
    }
}

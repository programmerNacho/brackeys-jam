using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoreBlock : Block
    {
        public override void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            if (otherBlock is SimpleBlock simpleBlock)
            {
                if (simpleBlock.CurrentAffiliation == Affiliation.Free)
                {
                    ConnectBlock(otherBlock, false);
                    otherBlock.ChangeBlockAndChildBlocksAffiliation(CurrentAffiliation);
                    blockDock.Dock(mySide, otherSide, otherBlock);
                }
            }
        }

        public override void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
        {
            Debug.Log(gameObject.name + " muerto.");
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
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

        protected override void OnTriggerEnter2D(Collider2D collision)
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

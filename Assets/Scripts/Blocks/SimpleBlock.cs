using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SimpleBlock : Block
    {
        public enum Affiliation { Free, Player, Enemy }

        public Affiliation CurrentAffiliation { get; private set; }

        public override void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            if (otherBlock is SimpleBlock simpleBlock)
            {
                if (CurrentAffiliation == Affiliation.Free)
                {
                    if (simpleBlock.CurrentAffiliation == Affiliation.Free)
                    {
                        ConnectBlock(otherBlock, false);
                    }
                    else if (simpleBlock.CurrentAffiliation == Affiliation.Player || simpleBlock.CurrentAffiliation == Affiliation.Enemy)
                    {
                        ConnectBlock(otherBlock, true);
                    }
                }
            }
        }

        public override void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
        {
            DisconnectFromParent();
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 globalImpactPoint = collision.GetContact(0).point;
            Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

            SimpleBlock simpleBlock = collision.gameObject.GetComponent<SimpleBlock>();
            if(simpleBlock)
            {
                if(CurrentAffiliation == Affiliation.Player && simpleBlock.CurrentAffiliation == Affiliation.Enemy ||
                   CurrentAffiliation == Affiliation.Enemy && simpleBlock.CurrentAffiliation == Affiliation.Player)
                {
                    Attacked(globalImpactPoint, globalImpactDirection);
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            Vector2 globalImpactPoint = collision.transform.position;
            Vector2 globalImpactDirection = (transform.position - collision.transform.position).normalized;

            SimpleBlock simpleBlock = collision.GetComponent<SimpleBlock>();
            if (simpleBlock)
            {
                if (CurrentAffiliation == Affiliation.Player && simpleBlock.CurrentAffiliation == Affiliation.Enemy ||
                   CurrentAffiliation == Affiliation.Enemy && simpleBlock.CurrentAffiliation == Affiliation.Player)
                {
                    Attacked(globalImpactPoint, globalImpactDirection);
                }
            }
        }
    }
}
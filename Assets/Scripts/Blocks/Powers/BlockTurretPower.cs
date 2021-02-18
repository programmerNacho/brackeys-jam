using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BlockTurretPower : BlockPower
    {
        protected int currentBoostLevel = 0;

        public virtual void SetBoost(int boostCount)
        {
            currentBoostLevel = boostCount;
        }

        protected Block GetNearestEnemyBlock(float radius)
        {
            Collider2D[] thingsInRadius = Physics2D.OverlapCircleAll(myBlock.transform.position, radius);

            float minDistance = float.MaxValue;
            Block nearestEnemyBlock = null;

            Affiliation enemyAffiliation = Affiliation.Free;

            switch (myBlock.CurrentAffiliation)
            {
                case Affiliation.Player:
                    enemyAffiliation = Affiliation.Enemy;
                    break;
                case Affiliation.Enemy:
                    enemyAffiliation = Affiliation.Player;
                    break;
            }

            foreach (Collider2D thing in thingsInRadius)
            {
                Block block = thing.GetComponentInParent<Block>();

                if(block && block.CurrentAffiliation == enemyAffiliation)
                {
                    float distance = Vector2.Distance(myBlock.transform.position, block.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemyBlock = block;
                    }
                }
            }

            return nearestEnemyBlock;
        }
    }
}

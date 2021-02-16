using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SimpleBlock : Block
    {
        public override void DockTry(Block otherBlock, BlockSide mySide, BlockSide otherSide)
        {
            if (otherBlock is SimpleBlock simpleBlock)
            {
                if (CurrentAffiliation == Affiliation.Free)
                {
                    if (simpleBlock.CurrentAffiliation == Affiliation.Free)
                    {
                        ConnectBlock(otherBlock, false);
                        Dock(mySide, otherSide, otherBlock);
                    }
                    else if (simpleBlock.CurrentAffiliation == Affiliation.Player || simpleBlock.CurrentAffiliation == Affiliation.Enemy)
                    {
                        ConnectBlock(otherBlock, true);
                        ChangeBlockAndChildBlocksAffiliation(simpleBlock.CurrentAffiliation);
                        otherBlock.Dock(otherSide, mySide, this);
                    }
                }
            }
        }

        public override void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
        {
            DisconnectFromParent();
        }
    }
}
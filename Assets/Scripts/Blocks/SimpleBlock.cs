using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SimpleBlock : Block
    {
        public override void Attacked(Vector2 globalImpactPoint, Vector2 globalImpactDirection)
        {
            DisconnectFromParent();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class SimpleBlock : Block
    {
        protected override void CheckOverlaying()
        {
            Vector2 source = transform.position;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(source, overlayingRadio);

            foreach (var item in colliders)
            {
                Block block = item.GetComponent<BlockCenter>()?.BlockParent;
                if (block && block != this)
                {
                    DamageManager.TakeDamage();
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class SimpleBlock : Block
    {

        protected override void CheckOverlaying()
        {
            //Vector2 source = transform.position;

            //RaycastHit2D[] rayCast = Physics2D.CircleCastAll(source, overlayingRadio, Vector2.up);
            
            //foreach (var item in rayCast)
            //{
            //    Block block = item.collider.GetComponentInChildren<Block>();
            //    if (block && block != this)
            //    {
            //        BlockHurt();
            //    }
            //}
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockPowerShield : BlockPower
    {
        [SerializeField]
        private int shieldHealth = 5;
        public int currentShieldHealth = 5;

        [SerializeField]
        private int range = 5;

        [SerializeField]
        private float timeToRegenerateShieldHealth = 10;

        private void Start()
        {
            Invoke("RegenerateShield", timeToRegenerateShieldHealth);
        }

        private void RegenerateShield()
        {
            if (GetComponentInParent<CoreBlock>())
            {
                if (currentShieldHealth < shieldHealth)
                {
                    currentShieldHealth++;
                }
            }
            Invoke("RegenerateShield", timeToRegenerateShieldHealth);
        }
        // Sistema por distancia real
        public override void OnBlockConnected()
        {
            Debug.Log("Check");
            Vector2 raySource = transform.position;

            foreach (var item in Physics2D.OverlapCircleAll(raySource, range))
            {
                Block targetBlock = item.GetComponent<BlockCenter>()?.GetMyBlock();
                if (targetBlock)
                {
                    targetBlock.AddShield(this, 0);
                }
            }
        }

        // Sistema por distancia de conexion
        //public override void OnBlockConnected()
        //{
        //    foreach (var targetBlock in myBlock.GetNearbyBlocks())
        //    {
        //        targetBlock.AddShield(this, shieldRange);
        //    }
        //}
    }
}

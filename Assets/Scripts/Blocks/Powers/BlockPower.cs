using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BlockPower : MonoBehaviour
    {
        [SerializeField]
        protected Block myBlock = null;

        private void OnEnable()
        {
            if (myBlock)
            {
                myBlock.OnSetPowers.AddListener(OnBlockConnected);
            }
        }

        private void OnDisable()
        {
            if (myBlock)
            {
                myBlock.OnSetPowers.RemoveListener(OnBlockConnected);
            }
        }

        private void OnDestroy()
        {
            if (myBlock)
            {
                myBlock.OnSetPowers.RemoveListener(OnBlockConnected);
            }
        }

        public abstract void OnBlockConnected();
        public abstract void OnBlockDisconnected();
    }
}

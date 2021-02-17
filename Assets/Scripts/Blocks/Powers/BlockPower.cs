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
            if(myBlock)
            {
                myBlock.OnConnect.AddListener(OnBlockConnected);
                myBlock.OnDisconnect.AddListener(OnBlockDisconnected);
            }
        }

        private void OnDisable()
        {
            if (myBlock)
            {
                myBlock.OnConnect.RemoveListener(OnBlockConnected);
                myBlock.OnDisconnect.RemoveListener(OnBlockDisconnected);
            }
        }

        private void OnDestroy()
        {
            if (myBlock)
            {
                myBlock.OnConnect.RemoveListener(OnBlockConnected);
                myBlock.OnDisconnect.RemoveListener(OnBlockDisconnected);
            }
        }

        public abstract void OnBlockConnected();
        public abstract void OnBlockDisconnected();
    }
}

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
                myBlock.OnStartDisconnect.AddListener(OnBlockDisconnected);
                myBlock.OnCompleteDisconnect.AddListener(OnBlockConnected);
            }
        }

        private void OnDisable()
        {
            if (myBlock)
            {
                myBlock.OnConnect.RemoveListener(OnBlockConnected);
                myBlock.OnStartDisconnect.RemoveListener(OnBlockDisconnected);
                myBlock.OnCompleteDisconnect.RemoveListener(OnBlockConnected);
            }
        }

        private void OnDestroy()
        {
            if (myBlock)
            {
                myBlock.OnConnect.RemoveListener(OnBlockConnected);
                myBlock.OnStartDisconnect.RemoveListener(OnBlockDisconnected);
                myBlock.OnCompleteDisconnect.RemoveListener(OnBlockConnected);
            }
        }

        public abstract void OnBlockConnected();
        public abstract void OnBlockDisconnected();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlocksTest : MonoBehaviour
    {
        [SerializeField]
        private Block blockA = null;
        [SerializeField]
        private Block blockB = null;

        private void Start()
        {
            //BlockBShouldBeFatherOfA();
            DisconnectBlockB();
        }

        private void BlockAShouldBeFatherOfB()
        {
            blockA.ConnectBlock(blockB, false);
        }

        private void BlockBShouldBeFatherOfA()
        {
            blockA.ConnectBlock(blockB, true);
        }

        private void DisconnectBlockB()
        {
            blockB.DisconnectFromParent();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Round", menuName = "Game/Round", order = 1)]
    public class Round : ScriptableObject
    {
        public List<Zone> enemies = new List<Zone>();
        public int moneyWon = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class ShopSaleUI : MonoBehaviour
    {
        [SerializeField]
        private Image icon = null;
        [SerializeField]
        private TextMeshProUGUI buttonText = null;
        [SerializeField]
        private TextMeshProUGUI moneyText = null;
        [SerializeField]
        private TextMeshProUGUI nameText = null;

        public void Initialize(string blockName, Sprite sprite, Color color, int index, int cost)
        {
            icon.sprite = sprite;
            icon.color = color;
            buttonText.text = (index + 1).ToString();
            moneyText.text = cost.ToString();
            nameText.text = blockName;
        }
    }
}

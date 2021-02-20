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

        public void Initialize(Sprite sprite, int index, int cost)
        {
            icon.sprite = sprite;
            buttonText.text = (index + 1).ToString();
            moneyText.text = cost.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField]
        private Shop shop = null;
        [SerializeField]
        private ShopSaleUI shopSaleUI = null;
        [SerializeField]
        private Transform parent = null;
        [SerializeField]
        private TextMeshProUGUI moneyText = null;

        private void Start()
        {
            List<BlockForSale> sales = shop.saleList;

            for (int i = 0; i < sales.Count; i++)
            {
                ShopSaleUI clone = Instantiate(shopSaleUI, parent);
                clone.Initialize(sales[i].blockName, sales[i].icon, sales[i].color, i, sales[i].price);
            }
        }

        private void Update()
        {
            moneyText.text = GameManager.Instance.Money.ToString();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct BlockForSale
{
    public Sprite icon;
    public Color color;
    public GameObject block;
    public int price;
    public string blockName;

    public BlockForSale(Sprite icon,Color color , GameObject block, int price, string blockName)
    {
        this.icon = icon;
        this.color = color;
        this.block = block;
        this.price = price;
        this.blockName = blockName;
    }

}
public class Shop : MonoBehaviour
{
    [SerializeField]
    public List<BlockForSale> saleList = new List<BlockForSale>();

    public LayerMask sideLayer;
    public GameObject freeSide = null;
    public float freeSideDistance = 2;
    public bool isActive = true;

    public void SetIsActive(bool value)
    {
        isActive = value;
    }

    public void CreateNewBlock(int saleListPos, Vector2 mousePosition)
    {
        if (!isActive) return;
        if (saleListPos >= saleList.Count) return;

        bool iHaveMoney = Game.GameManager.Instance.Money >= saleList[saleListPos].price;
        GameObject block = saleList[saleListPos].block;

        if (iHaveMoney && block)
        {
            //Game.BlockSide side = CheckBlockPosition(mousePosition);

            Game.Block newBlock = Instantiate(block, mousePosition, new Quaternion()).GetComponentInParent<Game.Block>();
            RemoveMoney(saleList[saleListPos].price);

            //if (side)
            //{
            //    Game.Block newBlock = Instantiate(block, mousePosition, new Quaternion()).GetComponentInParent<Game.Block>();

            //    //if (newBlock)
            //    //{
            //    //    side.GetBlock().TryConnectANewBlock(side, newBlock.GetMainSide());
            //    //}

            //    RemoveMoney(saleList[saleListPos].price);
            //}
        }
    }

    public Game.BlockSide CheckBlockPosition(Vector2 mousePosition)
    {
        //freeSide.SetActive(false);
        //Vector2 direction = Vector2.zero;

        //RaycastHit2D[] hit = Physics2D.RaycastAll(mousePosition, direction, 0, sideLayer);

        //foreach (var item in hit)
        //{
        //    Game.BlockSide side = item.collider.GetComponent<Game.BlockSide>();
        //    if (side && !side.GetLocked())
        //    {
        //        PlaceFreeSide(side);
        //        return side;
        //    }
        //}

        return null;
    }

    private void PlaceFreeSide(Game.BlockSide side)
    {
        if (!side) return;

        Game.Block block = side.GetBlock();

        if (!block) return;

        freeSide.SetActive(true);

        Vector2 blockPosition = block.transform.position;
        Vector2 sidePosition = side.transform.position;

        Vector2 dockDirection = (sidePosition - blockPosition).normalized;
        float distanceToMySide = Vector2.Distance(sidePosition, blockPosition);
        float distanceOtherBLockToOtherSide = freeSideDistance;
        float offsetDistance = distanceToMySide + distanceOtherBLockToOtherSide;

        Vector2 dockPosition = blockPosition + (dockDirection * offsetDistance);

        freeSide.transform.rotation = side.transform.rotation;
        freeSide.transform.position = dockPosition;
    }

    public void SetMoney(int money)
    {
        Game.GameManager.Instance.Money = money;
    }
    public void AddMoney(int money)
    {
        Game.GameManager.Instance.Money += money;
    }
    public void RemoveMoney(int money)
    {
        Game.GameManager.Instance.Money -= money;
        if (Game.GameManager.Instance.Money < 0) Game.GameManager.Instance.Money = 0;
    }
    public int GetMoney()
    {
        return Game.GameManager.Instance.Money;
    }
}

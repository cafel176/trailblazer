using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chestPanel : MonoBehaviour {

    public Image[] btns;
    [HideInInspector]
    public Chest chest;//拿走道具后要记得移除

    private int[] itemIndexs;
    private int itemNum = 0;
    private Sprite UIMask;

    void Start()
    {
        itemIndexs = new int[btns.Length];
        for (int i = 0; i < itemIndexs.Length; i++)
        {
            itemIndexs[i] = -1;
        }
        UIMask = btns[0].sprite;

        for (int i = 0; i < chest.itemlist.Count; i++)
        {
            setItem(chest.itemlist[i]);
        }
    }

    //取走道具，输入按钮序号
    public void btnDown(int index)
    {
        if (itemIndexs[index] >= 0 && itemNum >= 1)
        {
            bool can = PlayerManager.instance.getItem(itemIndexs[index]);
            if (can)
            {
                chest.removeItem(index);
                for (int i = index; i < itemNum - 1; i++)
                {
                    itemIndexs[i] = itemIndexs[i + 1];
                    btns[i].sprite = gameManager.instance.itemIcons[itemIndexs[i]];
                }
                itemIndexs[itemNum - 1] = -1;
                btns[itemNum - 1].sprite = UIMask;
                itemNum--;
            }
        }
    }

    //设置下一个按钮放置的物品，输入道具编号
    public void setItem(int itemIndex)
    {
        if(itemNum >= 0 && itemNum < btns.Length)
        {
            itemIndexs[itemNum] = itemIndex;
            btns[itemNum].sprite = gameManager.instance.itemIcons[itemIndex];
            itemNum++;
        }
    }

    public void close()
    {
        Destroy(this.gameObject);
    }
}

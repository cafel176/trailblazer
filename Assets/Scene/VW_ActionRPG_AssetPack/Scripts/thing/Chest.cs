using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour {

    //有关道具箱子的逻辑
	public float minDistance;
	public ParticleSystem ChestParticles;
    public GameObject chestPanel;
    public List<int> itemlist = new List<int>();

    private Transform player;
    private bool opened = false;
    private chestPanel cp;

    void Start()
	{
        player = PlayerManager.instance.player.transform;
        if (ChestParticles) {
            ParticleSystem go = Instantiate(ChestParticles, this.transform.position, this.transform.rotation);
            go.transform.SetParent(transform);
        }
        spawnItem();

    }
	
    //鼠标点击开箱
	void OnMouseDown()
	{
		if (player) {
			float dist = Vector3.Distance (player.position, transform.position);

			if (dist < minDistance)
            {
                if (opened == false)
                {
                    GetComponent<Animation>().Play("Opening");
                    GetComponent<AudioSource>().Play();
                    opened = true;
                }

                if(!cp)
                    spawnChestPanel();
            }
		}
		else {
			Debug.Log ("没有生成角色，无法打开箱子");
		}
	}

    public void spawnItem()
    {
        int j = gameManager.instance.itemSeed % 4+1;//目前最多生成3个道具，最少1个
        for(int i = 0; i < j; i++)
        {
            int w = (gameManager.instance.itemSeed + gameManager.instance.itemIcons.Length) % gameManager.instance.itemIcons.Length;//最多6种道具
            itemlist.Add(w);
            gameManager.instance.refreshSeed(1);
        }
    }

    public void removeItem(int index)
    {
        itemlist.RemoveAt(index);
    }

    public void spawnChestPanel()
    {
        GameObject go = GameObject.Instantiate(chestPanel) as GameObject;
        go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        go.GetComponent<RectTransform>().localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().localScale = Vector3.one;

        cp = go.GetComponent<chestPanel>();
        cp.chest=this;
    }
}


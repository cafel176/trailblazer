using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour {
    //针对BOSS,可以用这种方式生成下一层的入口
    public GameObject chest;
    public Transform chestPos;

    private bool clear = false;
	
	// Update is called once per frame
	void Update ()
    {
        if (!clear && transform.childCount <= 1)
        {
            clear = true;
            spawnChest();
        }
	}

    void spawnChest()
    {
        GameObject go = Instantiate(chest) as GameObject;
        go.transform.SetParent(transform);
        go.transform.localPosition = chestPos.localPosition;
    }

}

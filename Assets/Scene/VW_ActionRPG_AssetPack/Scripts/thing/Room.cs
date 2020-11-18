using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MapElement
{
    public GameObject[] enemys;

    void Start () {
        gameManager.instance.enemySeed %= enemys.Length;
        GameObject go = Instantiate(enemys[gameManager.instance.enemySeed]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(0,2,0);
        go.transform.localRotation = Quaternion.identity;
        gameManager.instance.refreshSeed(2);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCreate : MonoBehaviour {
    //随机地图生成算法
    public Room[] lrooms;
    public Room[] rrooms;
    public Room[] lbossrooms;
    public Room[] rbossrooms;
    public Ways[] lways;
    public Ways[] rways;
    public Starts[] starts;

    //基础房间数和层数加权
    public int baseRoomNum = 3,levelWeight=3;
    //当前朝向
    private int nowTurn;
    //当前房间接口，startPos记录角色开始时所处的位置
    private Transform nowPos,startPos;
    //地图根节点
    private Transform root;

    void Awake()
    {
        UIManager.instance.spawnGameControler();
    }

    void Start ()
    {
        baseRoomNum += gameManager.instance.mapSeed % 5;
        levelWeight += (gameManager.instance.mapSeed % 3 - 1);
        GameObject go = new GameObject();
        root = go.transform;

        gameManager.instance.spawnCamera3();
        spawnMap();
        if (PlayerManager.instance.player==null)
            PlayerManager.instance.spawnPlayer(startPos);
        else
            PlayerManager.instance.movePlayer(startPos);
        PlayerManager.instance.spawnPlayerPanel();

        UIManager.instance.beWhite(0.02f);
    }

    public void spawnMap()
    {
        spawnMapElement(0);
        int y = baseRoomNum + gameManager.instance.level * levelWeight;
        for (int i=0;i< y; i++)
        {
            if (i % 2 < 1)
            {
                spawnMapElement(1);
                if (i == y - 1)
                {
                    spawnMapElement(3);
                    spawnMapElement(4);
                }
            }
            else
            {
                if (i == y - 1)
                {
                    spawnMapElement(3);
                    spawnMapElement(4);
                }
                else
                {
                    spawnMapElement(2);
                }
            }
        }
    }

    private void spawnMapElement(int type)
    {
        if (type == 0)//生成起始点
        {
            int y = gameManager.instance.mapSeed % starts.Length;
            GameObject start = Instantiate(starts[y].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            start.transform.SetParent(root);
            nowPos = start.GetComponent<Starts>().next;
            nowTurn = start.GetComponent<Starts>().outType;
            startPos = start.GetComponent<Starts>().startPoint;
        }
        else if (type == 1)//生成道路
        {
            if (nowTurn == 0)
            {
                int y = gameManager.instance.mapSeed % lways.Length;
                GameObject way = Instantiate(lways[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                way.transform.SetParent(root);
                nowPos = way.GetComponent<Ways>().next;
                nowTurn = way.GetComponent<Ways>().outType;
            }
            else
            {
                int y = gameManager.instance.mapSeed % rways.Length;
                GameObject way = Instantiate(rways[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                way.transform.SetParent(root);
                nowPos = way.GetComponent<Ways>().next;
                nowTurn = way.GetComponent<Ways>().outType;
            }
        }
        else if (type == 2)//生成房间
        {
            if (nowTurn == 0)
            {
                int y = gameManager.instance.mapSeed % lrooms.Length;
                GameObject room = Instantiate(lrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
                nowPos = room.GetComponent<Room>().next;
                nowTurn = room.GetComponent<Room>().outType;
            }
            else
            {
                int y = gameManager.instance.mapSeed % rrooms.Length;
                GameObject room = Instantiate(rrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
                nowPos = room.GetComponent<Room>().next;
                nowTurn = room.GetComponent<Room>().outType;
            }
        }
        else if (type == 3)//生成通往BOSS的房间
        {
            if (nowTurn == 0)
            {
                int y = gameManager.instance.mapSeed % lrooms.Length;
                GameObject room = Instantiate(lrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
                nowPos = room.GetComponent<Room>().next;
                nowTurn = room.GetComponent<Room>().outType;
            }
            else
            {
                int y = gameManager.instance.mapSeed % rrooms.Length;
                GameObject room = Instantiate(rrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
                nowPos = room.GetComponent<Room>().next;
                nowTurn = room.GetComponent<Room>().outType;
            }
        }
        else
        {
            if (nowTurn == 0)
            {
                int y = gameManager.instance.mapSeed % lbossrooms.Length;
                GameObject room = Instantiate(lbossrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
            }
            else
            {
                int y = gameManager.instance.mapSeed % rbossrooms.Length;
                GameObject room = Instantiate(rbossrooms[y].gameObject, nowPos.position, nowPos.rotation) as GameObject;
                room.transform.SetParent(root);
            }
        }

        gameManager.instance.refreshSeed(0);
    }

}

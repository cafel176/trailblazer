using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

    public static gameManager instance=null;

    public Sprite[] itemIcons;//用图标来表示道具

    //随机种子
    [HideInInspector]
    public int mapSeed, itemSeed, enemySeed;

    //相机状态
    [HideInInspector]
    public int cameraState = 3;
    //当前层数
    [HideInInspector]
    public int level = 1;

    public GameObject camera1PrefabHigh;
    public GameObject camera3PrefabHigh;
    public GameObject camera1PrefabMid;
    public GameObject camera3PrefabMid;

    //保存相机引用
    [HideInInspector]
    public GameObject camera1;
    [HideInInspector]
    public GameObject camera3;

    private void Awake()
    {
        //创造管理器实例
        if (gameManager.instance == null)
        {
            gameManager.instance = this;
            DontDestroyOnLoad(gameManager.instance.gameObject);
        }

        if (!UIManager.instance.haveSeed)
            spawnSeed();
        else
        {
            mapSeed = UIManager.instance.seeds[0];
            itemSeed = UIManager.instance.seeds[1];
            enemySeed = UIManager.instance.seeds[2];
        }

    }

    void spawnSeed()
    {
        mapSeed= Random.Range(0, 100);
        itemSeed = Random.Range(0, 100);
        enemySeed = Random.Range(0, 100);
    }

    public void changeCamera()
    {
        if (cameraState == 1)
        {
            camera1.SetActive(false);
            camera3.SetActive(true);
            cameraState = 3;
        }
        else
        {
            camera3.SetActive(false);
            camera1.SetActive(true);
            cameraState = 1;
        }
    }

    public void spawnCamera3()
    {

#if UNITY_ANDROID//安卓
        camera3 = Instantiate(camera3PrefabMid, Vector3.zero, Quaternion.identity) as GameObject;
#endif

#if UNITY_EDITOR
        camera3 = Instantiate(camera3PrefabHigh, Vector3.zero, Quaternion.identity) as GameObject;
#endif

#if UNITY_STANDALONE_WIN
        camera3 = Instantiate(camera3PrefabHigh, Vector3.zero, Quaternion.identity) as GameObject;
#endif

    }

    public void spawnCamera1()
    {
#if UNITY_ANDROID//安卓
        camera1 = Instantiate(camera1PrefabMid, Vector3.zero, Quaternion.identity) as GameObject;
#endif

#if UNITY_EDITOR
        camera1 = Instantiate(camera1PrefabHigh, Vector3.zero, Quaternion.identity) as GameObject;
#endif

#if UNITY_STANDALONE_WIN
        camera1 = Instantiate(camera1PrefabHigh, Vector3.zero, Quaternion.identity) as GameObject;
#endif
    }

    //0是地图种子，1是道具种子，2是敌人种子
    public void refreshSeed(int type)
    {
        if(type==0)
            mapSeed = (mapSeed * 123 + 456) % 25;
        else if (type == 1)
            itemSeed = (itemSeed * 123 + 456) % 25;
        else if (type == 2)
            enemySeed = (enemySeed * 123 + 456) % 25;
    }

    public void useItem(int index)
    {
        if(index>=0 && index< itemIcons.Length)
        {
            switch (index)
            {
                case 0:
                    {
                        PlayerManager.instance.data.addHp(20);
                    }
                    break;
                case 1:
                    {
                        PlayerManager.instance.data.addHp(50);
                    }
                    break;
                case 2:
                    {
                        PlayerManager.instance.data.addHp(80);
                    }
                    break;
                case 3:
                    {
                        PlayerManager.instance.data.addKp(5);
                    }
                    break;
                case 4:
                    {
                        PlayerManager.instance.data.addKp(10);
                    }
                    break;
                case 5:
                    {
                        PlayerManager.instance.data.addKp(20);
                    }
                    break;
                default:break;
            }
        }
    }

    public void nextLevel()
    {
        gameManager.instance.level++;
        if (gameManager.instance.level > 3)
        {
            //进行通关操作
            UIManager.instance.LoadLevel(0);
        }
        else
            UIManager.instance.LoadLevel(1);
    }
}
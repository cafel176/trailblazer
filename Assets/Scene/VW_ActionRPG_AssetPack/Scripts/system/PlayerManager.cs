using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    //记录玩家数据的管理器
    public static PlayerManager instance=null;

    public GameObject playerPrefab;

    public GameObject playerPanelPrefab;

    public int angry = 0;

    public float angryTime = 0.5f;

    //角色引用
    [HideInInspector]
    public GameObject player = null;
    [HideInInspector]
    public ActorData data;
    [HideInInspector]
    public int attackState = 0;

    private playerPanel playerPanel = null;

    [HideInInspector]
    public bool canDo=true;

    private float timer = 0;
    [HideInInspector]
    public int itemNum = 0;
    private int[] itemIndexs = new int[12];

    private void Awake()
    {
        if (PlayerManager.instance == null)
        {
            PlayerManager.instance = this;
            DontDestroyOnLoad(PlayerManager.instance.gameObject);
        }
        data = gameObject.GetComponent<ActorData>();
        for (int i = 0; i < itemIndexs.Length; i++)
        {
            itemIndexs[i] = -1;
        }
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > angryTime)
        {
            timer = 0;
            if (angry > 0)
                angry--;
        }

#if UNITY_EDITOR
        controlPlayer();
#endif

#if UNITY_STANDALONE_WIN
        controlPlayer();
#endif

    }

    public void spawnPlayer(Transform pos)
    {
        player = Instantiate(playerPrefab, pos.position, Quaternion.identity) as GameObject;
        Camera c = player.GetComponentInChildren<Camera>();
        gameManager.instance.camera1 = c.gameObject;
        gameManager.instance.camera3.GetComponent<FollowTrackingCamera>().target = player.transform;
        if (gameManager.instance.cameraState == 3)
        {
            gameManager.instance.camera1.SetActive(false);
            gameManager.instance.camera3.SetActive(true);
        }
        else
        {
            gameManager.instance.camera3.SetActive(false);
            gameManager.instance.camera1.SetActive(true);
        }

        if(attackState == 2)
        {
            data.attack *= 2;
            data.flyattack *= 2;
        }
        attackState = 0;
    }

    public void movePlayer(Transform pos)
    {
        if (player==null)
        {
            Debug.LogError("还没有生成角色，不能进行移动");
        }
        else
        {
            player.transform.position = pos.position;
        }
    }

    public void controlPlayer()
    {
        if (player == null)
        {
            //Debug.LogError("角色还没有生成");
            return;
        }

        if (!player.GetComponentInChildren<PlayerBattle>().death)
        {
            if (gameManager.instance.cameraState == 3)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                moveControl(h, v);
                
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                        changeWeaponControl(0);
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                        changeWeaponControl(1);
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                        changeWeaponControl(2);
                    else if (Input.GetKeyDown(KeyCode.Z))
                        attackControl();
                    else if (Input.GetKeyDown(KeyCode.X))
                        shotControl();
                }
            }
            else
            {

            }
        }
    }

    public void moveControl(float h,float v)
    {
         player.GetComponent<PlayerMove>().Move3(h, v);
    }

    public void attackControl()
    {
        if (canDo)
            player.GetComponentInChildren<PlayerBattle>().Attack();
    }

    public void shotControl()
    {
        if (canDo)
            player.GetComponentInChildren<PlayerBattle>().Shot();
    }

    public void changeWeaponControl(int type)
    {
        if (canDo)
            player.GetComponentInChildren<PlayerBattle>().changeAttackState(attackState = type);
    }

    public void spawnPlayerPanel()
    {
        if (playerPanel == null)
        {
            GameObject go = GameObject.Instantiate(playerPanelPrefab) as GameObject;
            playerPanel = go.GetComponent<playerPanel>();
            go.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
            go.GetComponent<RectTransform>().localPosition = Vector3.zero;
            go.GetComponent<RectTransform>().localScale = Vector3.one;

            playerPanel.attackTypeImage.sprite = playerPanel.attackType[attackState];
            for (int i = 0; i < itemNum; i++)
            {
                playerPanel.changeItem(i, gameManager.instance.itemIcons[itemIndexs[i]]);
            }
        }
    }

    //获取道具，输入道具编号
    public bool getItem(int itemIndex)
    {
        if (itemNum >= 0 && itemNum < itemIndexs.Length)
        {
            itemIndexs[itemNum] = itemIndex;
            playerPanel.changeItem(itemNum,gameManager.instance.itemIcons[itemIndex]);
            itemNum++;
            return true;
        }
        return false;//满了
    }

    //使用道具，输入按钮序号
    public void useItem(int index)
    {
        if (itemIndexs[index] >= 0 && itemNum >= 1)
        {
            gameManager.instance.useItem(itemIndexs[index]);
            for (int i = index; i < itemNum - 1; i++)
            {
                itemIndexs[i] = itemIndexs[i + 1];
                playerPanel.changeItem(i, gameManager.instance.itemIcons[itemIndexs[i]]);
            }
            itemIndexs[itemNum - 1] = -1;
            playerPanel.changeItem(itemNum - 1, null);
            itemNum--;
        }
    }
}

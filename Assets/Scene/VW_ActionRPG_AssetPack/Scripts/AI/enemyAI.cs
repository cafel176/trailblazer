using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {

    public float doTime = 1f;
    public float doInMoveTime = 6f;
    public float afterDoInMoveTime = 3f;
    public float DoRange = 20;
    public float AttackRange = 3;

    [HideInInspector]
    public GameObject player;
    private PlayerBattle playerdata;
    private EnemyBattle battle;
    private float timer = 0,timer2=0;

    void Start () {
        player = PlayerManager.instance.player;
        battle= gameObject.GetComponentInChildren<EnemyBattle>();
        playerdata = player.GetComponentInChildren<PlayerBattle>();
    }
	
	void FixedUpdate ()
    {
        Vector3 u = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 v = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        float d = Vector3.Distance(u, v);
        if(!battle.death)
            battle.Rotating((u - v).x, (u - v).z);

        if (d <= AttackRange)
        {
            battle.Move(0, 0);
            timer += Time.fixedDeltaTime;
            if (timer >= doTime)
            {
                timer = 0;
                if (!playerdata.death)
                {
                    if (battle.far)
                    {
                        battle.Shot();
                    }
                    else
                    {
                        battle.Attack();
                    }
                }
            }
        }
        else if(d <= DoRange)
        {
            if (battle.boss)
            {
                battle.Move(0, 0);
                timer2 += Time.fixedDeltaTime;
                if (timer2 >= doInMoveTime)
                {
                    timer2 = -afterDoInMoveTime;
                    if (!playerdata.death)
                    {
                        battle.Shot();
                    }
                }
                else if(timer2 >=0)
                    battle.Move((u - v).x, (u - v).z);
            }
            else
                battle.Move((u - v).x, (u - v).z);
        }


    }

}

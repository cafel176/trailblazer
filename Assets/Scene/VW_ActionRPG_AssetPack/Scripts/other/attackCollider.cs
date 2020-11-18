using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackCollider : MonoBehaviour {

    public ActorData data=null;
    public bool fly;
    public bool friendly;
    public GameObject[] particles;

    void Start()
    {
        if (friendly && data==null)
        {
            data = PlayerManager.instance.data;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        int damage;
        if(fly)
            damage= data.flyDamageNum(data.attackType);
        else
            damage= data.damageNum(data.attackType);
        if (friendly && col.gameObject.tag == Tags.enemy)
        {
            int finalDamage = 0;
            float d = col.GetComponent<ActorData>().defendNum(data.attackType);
            if (data.attackType == 2)
            {
                finalDamage = (int)((damage / 100f) * (1f - d) * col.GetComponent<ActorData>().hp);
            }
            else
            {
                finalDamage = (int)(damage * (1f - d));
            }
            col.GetComponentInChildren<EnemyBattle>().hit(finalDamage);
            if (fly)
            {
                if (particles.Length > 0)
                {
                    for (int i = 0; i < particles.Length; i++)
                    {
                        Instantiate(particles[i], col.transform.position, this.transform.rotation);
                    }
                }
                Destroy(gameObject);
            }
            else
            {
                if (PlayerManager.instance.attackState == 2 && particles.Length > 0)
                {
                    for (int i = 0; i < particles.Length; i++)
                    {
                        Instantiate(particles[i], col.transform.position, this.transform.rotation);
                    }
                }
                PlayerManager.instance.angry += 8;
            }
        }
        else if (!friendly && col.gameObject.tag == Tags.player)
        {
            int finalDamage = 0;
            float d = PlayerManager.instance.data.defendNum(data.attackType);
            if (data.attackType == 2)
            {
                finalDamage = (int)((damage / 100f) * (1f - d) * PlayerManager.instance.data.hp);
            }
            else
            {
                finalDamage = (int)(damage * (1f - d));
            }
            col.GetComponentInChildren<PlayerBattle>().Hit(finalDamage);
            if (fly)
            {
                if (particles.Length > 0)
                {
                    for (int i = 0; i < particles.Length; i++)
                    {
                        Instantiate(particles[i], col.transform.position, this.transform.rotation);
                    }
                }
                Destroy(gameObject);
            }
        }

    }
}

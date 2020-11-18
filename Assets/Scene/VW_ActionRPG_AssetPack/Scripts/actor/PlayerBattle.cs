using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour {

    private Rigidbody rb;
    private Animator anima;
    private HashIDs hash;               // Reference to the HashIDs.
    private AudioSource audio;
    private ActorData data;

    public GameObject sword;
    public GameObject bow;
    public GameObject knife;
    public GameObject th;
    public GameObject attackCollider;
    public GameObject trail;

    public GameObject flyknife;
    public GameObject thread;

    public float afterHitTime = 5f;

    //0是剑，1是弓，2是匕首
    [HideInInspector]
    public int attackState = 0;
    private bool canHit=true;
    [HideInInspector]
    public bool death = false;
    private float timer = 0;

    void Start()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
        anima = gameObject.GetComponent<Animator>();
        hash = UIManager.instance.gameObject.GetComponent<HashIDs>();
        audio = gameObject.GetComponentInParent<AudioSource>();
        data = PlayerManager.instance.data;
    }

    void FixedUpdate()
    {
        if (!canHit)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= afterHitTime)
            {
                timer = 0;
                canHit = true;
            }
        }

        anima.SetFloat(hash.attackSpeed,data.attackSpeed);
        attackCollider.transform.localScale = new Vector3(data.attackRange, 1, data.attackRange);
    }

    //用于玩家操控，触发动画和效果
    public void changeAttackState(int state)
    {
        if (!PlayerManager.instance.canDo|| death)
            return;

        if (state == 0)
        {
            if (attackState == 1)
            {
                anima.SetTrigger(hash.putBow);
                anima.SetTrigger(hash.takeOutSword);
            }
            else if (attackState == 2)
            {
                anima.SetTrigger(hash.takeOutSword);
                data.attack *= 2;
                data.flyattack *= 2;
            }
        }
        else if (state == 1)
        {
            if (attackState == 0)
            {
                anima.SetTrigger(hash.putSword);
                StartCoroutine(wait(0.2f));
            }
            else if (attackState == 2)
            {
                anima.SetTrigger(hash.takeOutBow);
                data.attack *= 2;
                data.flyattack *= 2;
            }
        }
        else if (state == 2)
        {
            if (attackState == 0)
            {
                anima.SetTrigger(hash.putSword);
                data.attack /= 2;
                data.flyattack /= 2;
            }
            else if (attackState == 1)
            {
                anima.SetTrigger(hash.putBow);
                data.attack /= 2;
                data.flyattack /= 2;
            }
        }

        attackState = state;
    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        anima.SetTrigger(hash.takeOutBow);
    }

    //用于动画后隐藏游戏物体
    public void changeWeapon(int index)
    {
        if (index == 0)
        {
            sword.SetActive(true);
            bow.SetActive(false);
            knife.SetActive(false);
        }
        else if (index == 1)
        {
            sword.SetActive(false);
            bow.SetActive(true);
            knife.SetActive(false);
        }
        else if (index == 2)
        {
            sword.SetActive(false);
            bow.SetActive(false);
            knife.SetActive(true);
        }
        else if (index == 3)
        {
            sword.SetActive(false);
            bow.SetActive(false);
            knife.SetActive(false);
        }
        else if (index == 4)
        {
            th.SetActive(true);
        }
        else if (index == 5)
        {
            th.SetActive(false);
        }
    }

    //动画开始和结束时切换玩家允许操作状态
    public void changeDo(int type)
    {
        if(type==0)
            PlayerManager.instance.canDo = false;
        else
            PlayerManager.instance.canDo = true;
    }

    public void Attack()
    {
        if (!PlayerManager.instance.canDo || death)
            return;

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (attackState == 0)
        {
            anima.SetTrigger(hash.attackSword);
        }
        else if (attackState == 2)
        {
            anima.SetTrigger(hash.attackKnife);
        }
    }

    public void Shot()
    {
        if (!PlayerManager.instance.canDo || death)
            return;

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (attackState == 1)
        {
            anima.SetTrigger(hash.shot);
        }
        else if (attackState == 2)
        {
            anima.SetTrigger(hash.knifeThrow);
        }
    }

    //在射击后动画中调用来生成飞行物，0是飞刀，1是箭
    void spawnFly(int type)
    {
        if (type == 0)
        {
            Quaternion r = Quaternion.LookRotation(transform.forward, Vector3.forward);
            GameObject go=Instantiate(flyknife, transform.parent.position, r) as GameObject;
            go.GetComponent<attackCollider>().friendly = true;
            go.GetComponent<attackCollider>().data = data;
            go.GetComponent<fly>().timer = data.attackRange;
        }
        else if (type == 1)
        {
            Quaternion r = Quaternion.LookRotation(transform.forward, Vector3.forward);
            GameObject go = Instantiate(thread, transform.parent.position, r) as GameObject;
            go.GetComponent<attackCollider>().friendly = true;
            go.GetComponent<attackCollider>().data = data;
            go.GetComponent<fly>().timer = data.attackRange;
        }
    }

    //public void defend() { }

    public void Hit(int damage)
    {
        if (!canHit|| death)
            return;

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        anima.SetTrigger(hash.hit);
        data.getDamage(damage);

        if (data.hp <= 0)
            Die();
    }

    public void Die()
    {
        death = true;
        anima.SetTrigger(hash.die);
        transform.position = new Vector3(transform.position.x, transform.position.y-0.5f,transform.position.z);
    }

    //被攻击时播放动画前调用，会自动取消，也可以手动取消
    public void afterHit(int type)
    {
        if(type==0)
            canHit = false;
        else
        {
            canHit = true;
            timer = 0;
        }
    }

    public void showAttackCollider(int type)
    {
        if (type == 0)
            attackCollider.SetActive(false);
        else
            attackCollider.SetActive(true);
    }

    public void showTrail(int type)
    {
        if (type == 0)
            trail.SetActive(false);
        else
            trail.SetActive(true);
    }
}

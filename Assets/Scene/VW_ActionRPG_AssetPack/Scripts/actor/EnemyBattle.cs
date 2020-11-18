using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour {

    private Rigidbody rb;
    private Animator anima;
    private HashIDs hash;               // Reference to the HashIDs.
    private ActorData data;
    private AudioSource audio;

    public GameObject attackCollider;
    public GameObject thread;
    public GameObject[] magic;
    public GameObject trail;
    public GameObject trail2=null;
    public bool far = false;
    public bool boss = false;
    public int attackNearNum = 2;

    [HideInInspector]
    public bool death = false;
    private bool canHit = true;

    void Awake()
    {
        rb = gameObject.GetComponentInParent<Rigidbody>();
        anima = gameObject.GetComponent<Animator>();
        hash = UIManager.instance.gameObject.GetComponent<HashIDs>();
        audio = gameObject.GetComponentInParent<AudioSource>();
        data = gameObject.GetComponentInParent<ActorData>();
    }

    void FixedUpdate()
    {
        anima.SetFloat(hash.attackSpeed, data.attackSpeed);
        if(!far)
            attackCollider.transform.localScale = new Vector3(data.attackRange, 1, data.attackRange);
    }

    public void Move(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            rb.velocity = new Vector3(-horizontal, 0, -vertical).normalized * data.moveSpeed + new Vector3(0, rb.velocity.y, 0);
            anima.SetBool(hash.inWalk, true);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            if(anima.GetBool(hash.inWalk))
                anima.SetBool(hash.inWalk, false);
        }
    }

    public void Rotating(float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

        // Create a rotation based on this new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, 5 * Time.deltaTime);

        // Change the players rotation to this new rotation.
        rb.MoveRotation(newRotation);
    }

    public void Attack()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        int i=Random.Range(0, attackNearNum); 
        anima.SetTrigger(hash.getAttackNum(i));
    }

    public void Shot()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        anima.SetTrigger(hash.attackFar);
    }

    public void spawnFly()
    {
        Quaternion r = Quaternion.LookRotation(transform.forward, Vector3.forward);
        GameObject go = Instantiate(thread, transform.parent.position, r) as GameObject;
        go.GetComponent<attackCollider>().friendly = false;
        go.GetComponent<attackCollider>().data = data;
        go.GetComponent<fly>().timer = data.attackRange;
    }

    public void magicHit()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        int i=Random.Range(0, magic.Length);
        GameObject go = Instantiate(magic[i], PlayerManager.instance.player.transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<magic>().data = data;
    }

    public void hit(int damage)
    {
        if (!canHit)
            return;

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        anima.SetTrigger(hash.hit);
        data.getDamage(damage);

        if (data.hp <= 0)
            anima.SetTrigger(hash.die);
    }

    public void changeHit(int type)
    {
        if(type==0)
            canHit = false;
        else
            canHit = true;
    }

    //死亡动画结束时调用
    public void enemyDie()
    {
        Destroy(transform.parent.gameObject);
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
        if (trail != null)
        {
            if (type == 0)
                trail.SetActive(false);
            else
                trail.SetActive(true);
        }

        if (trail2!=null)
        {
            if (type == 0)
                trail2.SetActive(false);
            else
                trail2.SetActive(true);
        }
    }
}

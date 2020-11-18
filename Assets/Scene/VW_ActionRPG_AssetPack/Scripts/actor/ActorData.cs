using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorData : MonoBehaviour
{
    public int maxHp;
    public int maxKp;
    public int hp;
    public int kp;
    public int attack;
    public int flyattack;
    public float defend=1;
    public int[] fattack = new int[3];
    public float[] fdefend = new float[3];
    public float moveSpeed;
    public float attackSpeed=1;
    public float attackRange=1;
    public float luck;
    //攻击方式，0红伤，1白伤，2蓝伤
    public int attackType = 0;

    void Start()
    {
        hp = maxHp;
        kp = maxKp;
    }

    public int damageNum(int type)
    {
        int k = 1;
        if (Random.Range(0f, 1f) + luck > 0.8f && type!=2)
            k = 2;
        return attack * fattack[type]*k;
    }

    public int flyDamageNum(int type)
    {
        int k = 1;
        if (Random.Range(0f, 1f) + luck > 0.8f && type != 2)
            k = 2;
        return flyattack * fattack[type] * k;
    }

    public float defendNum(int type)
    {
        return defend * fdefend[type];
    }

    public void getDamage(int damage)
    {
        if (kp > damage)
            kp -= damage;
        else
        {
            kp = 0;
            if (hp > damage - kp)
                hp -= (damage - kp);
            else
                hp = 0;
        }
    }

    public void addHp(int num)
    {
        if (hp + num > maxHp)
            hp = maxHp;
        else
            hp += num;
    }

    public void addKp(int num)
    {
        if (kp + num > maxKp)
            kp = maxKp;
        else
            kp += num;
    }
}

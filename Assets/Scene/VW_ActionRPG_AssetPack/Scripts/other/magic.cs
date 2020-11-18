using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic : MonoBehaviour {

    [HideInInspector]
    public ActorData data;

    public GameObject[] particles;
    public float destroyTime = 3;
    public float delayTime = 0.3f;

    private SphereCollider collider;

    void Start()
    {
        collider=gameObject.GetComponent<SphereCollider>();
        collider.enabled = false;
        StartCoroutine(doS(delayTime));
    }

    IEnumerator doS(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].GetComponentInChildren<ParticleSystem>().Play();
        }
        collider.enabled = true;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        int damage= data.damageNum(data.attackType);
        if (col.gameObject.tag == Tags.player)
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
        }

    }
}

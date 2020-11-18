using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly : MonoBehaviour {

    public float speed;
    public float timer;

    private Rigidbody rb;

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
            Destroy(gameObject);
    }

	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = -transform.forward.normalized * speed;
        timer += 3;
    }

}

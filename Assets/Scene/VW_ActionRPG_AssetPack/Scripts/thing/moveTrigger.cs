using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTrigger : MonoBehaviour {

    public bool hide = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag== Tags.player)
        {
            if (hide)
                other.gameObject.SetActive(false);
            gameManager.instance.nextLevel();
        }
    }
}

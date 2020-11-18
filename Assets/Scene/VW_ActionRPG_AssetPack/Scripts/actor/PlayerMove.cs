using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //第三人称的人物移动
    public float turnSmoothing = 5f;   // A smoothing value for turning the player.
    public float speedDampTime = 0.2f;  // The damping for the speed parameter

    private Rigidbody rb;
    private Animator anima;
    private HashIDs hash;               // Reference to the HashIDs.
    private AudioSource audio;

    private Vector3 turn=Vector3.zero;
    private ActorData data;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        anima = gameObject.GetComponentInChildren<Animator>();
        hash = UIManager.instance.gameObject.GetComponent<HashIDs>();
        audio = gameObject.GetComponent<AudioSource>();
        data = PlayerManager.instance.data;
    }

    public void Move3(float horizontal, float vertical)
    {
        if (!PlayerManager.instance.canDo || gameObject.GetComponentInChildren<PlayerBattle>().death)
            return;

        if (horizontal != 0 || vertical != 0)
        {
            // ... set the players rotation and set the speed parameter to moveSpeed.
            Rotating(-horizontal, -vertical);
            rb.velocity = new Vector3(-horizontal, 0, -vertical).normalized * data.moveSpeed+ new Vector3(0, rb.velocity.y,0);

            Vector3 a = transform.eulerAngles;

            // v是z,h是x
            if (vertical > 0)
            {
                    if (horizontal > 0)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(hash.speedF, vertical * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(hash.speedC, -horizontal * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(hash.speedF, horizontal * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(hash.speedC, vertical * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else if (horizontal < 0)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(hash.speedF, -horizontal * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(hash.speedC, -vertical * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(hash.speedF, vertical * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(hash.speedC, -horizontal * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else
                    {
                        anima.SetFloat(hash.speedF, vertical * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(hash.speedC,0);
                    }
            }
            else if (vertical < 0)
            {
                if (horizontal > 0)
                {
                    if (a.y > turn.y)//右转
                    {
                        anima.SetFloat(hash.speedF, horizontal * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(hash.speedC, vertical * 2, speedDampTime, Time.deltaTime);
                    }
                    else if (a.y < turn.y)//左转
                    {
                        anima.SetFloat(hash.speedF, -vertical * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(hash.speedC, horizontal * 2, speedDampTime, Time.deltaTime);
                    }
                }
                else if (horizontal < 0)
                {
                    if (a.y > turn.y)//右转
                    {
                        anima.SetFloat(hash.speedF, -vertical * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(hash.speedC, horizontal * 2, speedDampTime, Time.deltaTime);
                    }
                    else if (a.y < turn.y)//左转
                    {
                        anima.SetFloat(hash.speedF, -horizontal * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(hash.speedC, -vertical * 2, speedDampTime, Time.deltaTime);
                    }
                }
                else
                {
                    anima.SetFloat(hash.speedF, -vertical * 3, speedDampTime, Time.deltaTime);
                    anima.SetFloat(hash.speedC, 0);
                }
            }
            else
            {
                anima.SetFloat(hash.speedF, Math.Abs(horizontal) * 3, speedDampTime, Time.deltaTime);
                anima.SetFloat(hash.speedC, 0);
            }
            turn = a;
            AudioManagement(true);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            anima.SetFloat(hash.speedF, 0);
            anima.SetFloat(hash.speedC, 0);
            AudioManagement(false);
        }
    }

    void Rotating(float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

        // Create a rotation based on this new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, turnSmoothing * Time.deltaTime);

        // Change the players rotation to this new rotation.
        rb.MoveRotation(newRotation);
    }

    public void Move1()
    {
        if (!PlayerManager.instance.canDo || gameObject.GetComponentInChildren<PlayerBattle>().death)
            return;

    }

    void AudioManagement(bool run)
    {
        // If the player is currently in the run state...
        if (run)
        //if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        {
            // ... and if the footsteps are not playing...
            if (!audio.isPlaying)
                // ... play them.
                audio.Play();
        }
        else
            // Otherwise stop the footsteps.
            audio.Stop();
    }
}

using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	// Here we store the hash tags for various strings used in our animators.
	public int locomotionState;
	public int speedF,speedC;
	public int angularSpeedFloat;
    public int attackKnife, attackSword, knifeThrow;
    public int shot,defend,hit,die;
    public int takeOutBow, putBow, takeOutSword, putSword;
    public int inWalk,inRun, attackFar;
    public int attackSpeed;

	void Awake ()
	{
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		speedF = Animator.StringToHash("speedF");
        speedC = Animator.StringToHash("speedC");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        attackSword = Animator.StringToHash("attackSword");
        attackKnife = Animator.StringToHash("attackKnife");
        attackSpeed = Animator.StringToHash("attackSpeed");
        knifeThrow = Animator.StringToHash("knifeThrow");
        shot = Animator.StringToHash("shot");
        defend = Animator.StringToHash("defend");
        hit = Animator.StringToHash("hit");
        die = Animator.StringToHash("die");
        takeOutBow = Animator.StringToHash("takeOutBow");
        takeOutSword = Animator.StringToHash("takeOutSword");
        putBow = Animator.StringToHash("putBow");
        putSword = Animator.StringToHash("putSword");

        inWalk = Animator.StringToHash("inWalk");
        inRun = Animator.StringToHash("inRun");
        attackFar = Animator.StringToHash("attackFar");
    }

    public int getAttackNum(int i)
    {
        return Animator.StringToHash("attackNear"+i);
    }
}

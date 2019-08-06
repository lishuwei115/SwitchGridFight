using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimatorScript : MonoBehaviour
{
	public Animator Anim;
	public PlayerChar Parent;

    public void SetAnimToIdle()
	{
		Anim.SetInteger("State", 0);
	}

    public void ParticlesAttack()
	{
		Parent.Attacking();
		AudioManagerScript.Instance.AS.PlayOneShot(Parent.AttackAudio);
	}
}

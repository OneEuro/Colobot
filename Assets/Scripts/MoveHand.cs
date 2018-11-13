using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHand : MonoBehaviour {

	Animator animator;
	public Transform cube;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	void OnAnimatorIK()
	{
		animator.SetLookAtWeight (1f);
		animator.SetLookAtPosition (cube.position);
		animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1f);
		animator.SetIKPosition (AvatarIKGoal.LeftHand, transform.position + transform.forward);
	}
}

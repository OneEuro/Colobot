using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMechanics : MonoBehaviour {


	//General parameters
	public float speedMove;
	public float jumpPower;
	public float turnSpeed;

	// GamePlay Parameter for characters
	private float gravityForce;
	private Vector3 moveVector;

	//Component links
	private CharacterController ch_Controller;
	private Animator ch_animator;
	private MobileControl mContr;
	//private ActionButton AB;

	// Use this for initialization
	void Start () {
		
		ch_Controller = GetComponent<CharacterController> ();
		mContr = GameObject.FindGameObjectWithTag ("Joystick").GetComponent<MobileControl> ();
		ch_animator = GetComponent<Animator> ();
		//AB = GameObject.FindGameObjectWithTag ("AB").GetComponent<ActionButton> ();

	}
	
	// Update is called once per frame
	void Update () {
		CharecterMove ();
		GamingGravity ();
	}

	// Method for ch.move
	private void CharecterMove()
	{
		if (ch_Controller.isGrounded) {
			
			//ch_animator.ResetTrigger ("Jump");
			ch_animator.SetBool ("Falling", false);

			moveVector = Vector3.zero;
			moveVector.x = mContr.Horizontal () * speedMove;
			moveVector.z = mContr.Vertical () * speedMove;

			//анимация передвижения персонажа
			if (moveVector.x != 0 || moveVector.z != 0)
				ch_animator.SetBool ("Move", true);
			else
				ch_animator.SetBool ("Move", false);


			if (Vector3.Angle (Vector3.forward, moveVector) > 1f || Vector3.Angle (Vector3.forward, moveVector) == 0) {
		
				Vector3 direct = Vector3.RotateTowards (transform.forward, moveVector, speedMove, 0.0f);
				transform.rotation = Quaternion.LookRotation (direct);
			}
		} else {
			if (gravityForce < -3f)
				ch_animator.SetBool ("Falling", true);

		}
		moveVector.y = gravityForce;
		ch_Controller.Move (moveVector * Time.deltaTime);
	}

	private void GamingGravity()
	{
		if (!ch_Controller.isGrounded) {
		
			gravityForce -= 20f * Time.deltaTime;
		} else {
			gravityForce = -1f;
		}
		if (Input.GetKeyDown(KeyCode.Space) && ch_Controller.isGrounded) { //keyboard
			gravityForce = jumpPower;
			ch_animator.SetTrigger ("Jump");
		}
	}

	public void Jump()
	{
		ch_animator.ResetTrigger ("Jump");
		if ( ch_Controller.isGrounded) {
			gravityForce = jumpPower;
			ch_animator.SetTrigger ("Jump");
		}
	}



	
}

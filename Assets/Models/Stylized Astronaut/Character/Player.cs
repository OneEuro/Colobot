using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;
		private MobileControl mContr;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
	private float gravity;
	public float jumpPower;
	private float animSpeed;
	public GameObject PointOfObjects;
	public GameObject Box;

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
			mContr = GameObject.FindGameObjectWithTag ("Joystick").GetComponent<MobileControl> ();
		}

	IEnumerator perenos()
	{
		Vector3 pos = Vector3.zero;
		while(true){
			if (PointOfObjects.transform.localPosition != Box.transform.position) {
				pos = Box.transform.position;

				pos.x = Mathf.LerpUnclamped (Box.transform.position.x, PointOfObjects.transform.localPosition.x, 1f * Time.deltaTime);
				pos.y = Mathf.LerpUnclamped (Box.transform.position.y, PointOfObjects.transform.localPosition.y, 1f * Time.deltaTime);
				pos.z = Mathf.LerpUnclamped (Box.transform.position.z, PointOfObjects.transform.localPosition.z, 1f * Time.deltaTime);
				Box.transform.position = pos;
				//yield return new WaitForSeconds (0.1f);
				yield return new WaitForEndOfFrame ();
			} else {
				PointOfObjects.transform.localPosition = PointOfObjects.transform.position;
				print ("STOP");
				StopCoroutine (perenos ());

			}

		}

	}

		void Update (){

		CharacterMove ();
		GamingGravity ();

		if (Input.GetKeyDown (KeyCode.E)) {

			StartCoroutine (perenos());

		}
		}

	private void CharacterMove()
	{
		if(controller.isGrounded){	
				//ch_animator.ResetTrigger ("Jump");
				anim.SetBool ("Falling", false);

				//анимация передвижения персонажа
			if (moveDirection.x != 0 || moveDirection.z != 0) {
				anim.SetBool ("Move", true);
				anim.SetFloat ("Speed",mContr.Vertical());
			
					
			} else {
				anim.SetBool ("Move", false);
			}
				moveDirection = transform.forward * mContr.Vertical () * speed;// Input.GetAxis("Vertical") * speed;
		
			} else {
				if (gravity < -3f)
					anim.SetBool ("Falling", true);

			}



			float turn = mContr.Horizontal ();//Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		moveDirection.y = gravity;
			controller.Move(moveDirection * Time.deltaTime);
			//moveDirection.y -= gravity * Time.deltaTime;

	}

	private void GamingGravity()
	{
		// if (!controller.isGrounded) {

		// 	gravity -= 20f * Time.deltaTime;
		// } else {
		// 	gravity = -1f;
		// }
		if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded) { //keyboard
			gravity = jumpPower;
			anim.SetTrigger ("Jump");
		}
	}


	public void Jump()
	{
		anim.ResetTrigger ("Jump");
		if ( controller.isGrounded) {
			gravity = jumpPower;
			anim.SetTrigger ("Jump");
		}
	}
}
